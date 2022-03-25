using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace ETModel
{
	[ObjectSystem]
	public class SessionAwakeSystem : AwakeSystem<Session, AChannel>
	{
		public override void Awake(Session self, AChannel b)
		{
			self.Awake(b);
			SessionCallbackComponent sessionComponent = self.AddComponent<SessionCallbackComponent>();
			sessionComponent.MessageCallback = (s, flag, opcode, memoryStream) => { self.Run(s, flag, opcode, memoryStream); };
			sessionComponent.DisposeCallback = s => { self.Dispose(); };
		}
	}

	public sealed class Session : Entity
	{
		private static int RpcId { get; set; }
		private AChannel channel;

		private readonly Dictionary<int, Action<IResponse>> requestCallback = new Dictionary<int, Action<IResponse>>();
		private readonly byte[] opcodeBytes = new byte[2];

		private Dictionary<ushort, Delegate> eventTable = new Dictionary<ushort, Delegate>();

		public NetworkComponent Network
		{
			get
			{
				return this.GetParent<NetworkComponent>();
			}
		}

		public int Error
		{
			get
			{
				return this.channel.Error;
			}
			set
			{
				this.channel.Error = value;
			}
		}

		public void Awake(AChannel aChannel)
		{
			this.channel = aChannel;
			this.requestCallback.Clear();
			long id = this.Id;
			channel.ErrorCallback += (c, e) =>
			{
				this.Network.Remove(id); 
			};
			channel.ReadCallback += this.OnRead;
		}
		
		public override void Dispose()
		{
			if (this.IsDisposed)
			{
				return;
			}

			this.Network.Remove(this.Id);

			base.Dispose();
			
			foreach (Action<IResponse> action in this.requestCallback.Values.ToArray())
			{
				action.Invoke(new ErrorResponse { Error = this.Error });
			}

			int error = this.channel.Error;
			if (this.channel.Error != 0)
			{
				Log.Info($"session dispose: {this.Id} ErrorCode: {error}, please see ErrorCode.cs!");
			}
			
			this.channel.Dispose();
			
			this.requestCallback.Clear();
		}

		public void Start()
		{
			this.channel.Start();
		}

		public IPEndPoint RemoteAddress
		{
			get
			{
				return this.channel.RemoteAddress;
			}
		}

		public ChannelType ChannelType
		{
			get
			{
				return this.channel.ChannelType;
			}
		}

		public MemoryStream Stream
		{
			get
			{
				return this.channel.Stream;
			}
		}

		public void OnRead(MemoryStream memoryStream)
		{
			try
			{
				this.Run(memoryStream);
			}
			catch (Exception e)
			{
				Log.Error(e);
			}
		}

		private void onListenerAdding(ushort opcode, Delegate callBack)
        {
			if (!eventTable.ContainsKey(opcode))
			{
				eventTable.Add(opcode, null);
			}
			Delegate d = eventTable[opcode];
			if (d != null && d.GetType() != callBack.GetType())
			{
				throw new Exception(string.Format("尝试为事件{0}添加不同类型的委托，当前事件所对应的委托是{1}，要添加的委托类型为{2}", opcode, d.GetType(), callBack.GetType()));
			}
		}

		public void addListener(ushort opcode, CallBack<IMessage> callBack)
		{
			this.onListenerAdding(opcode, callBack);
			this.eventTable[opcode] = (CallBack<IMessage>)eventTable[opcode] + callBack;
		}

		private void onListenerRemoving(ushort opcode, Delegate callBack)
		{
			if (eventTable.ContainsKey(opcode))
			{
				Delegate d = eventTable[opcode];
				if (d == null)
				{
					throw new Exception(string.Format("移除监听错误：事件{0}没有对应的委托", opcode));
				}
				else if (d.GetType() != callBack.GetType())
				{
					throw new Exception(string.Format("移除监听错误：尝试为事件{0}移除不同类型的委托，当前委托类型为{1}，要移除的委托类型为{2}", opcode, d.GetType(), callBack.GetType()));
				}
			}
			else
			{
				throw new Exception(string.Format("移除监听错误：没有事件码{0}", opcode));
			}
		}

		private void onListenerRemoved(ushort opcode)
		{
			if (eventTable[opcode] == null)
			{
				eventTable.Remove(opcode);
			}
		}

		public void removeListener(ushort opcode, CallBack<IMessage> callBack)
		{
			this.onListenerRemoving(opcode, callBack);
			eventTable[opcode] = (CallBack<IMessage>)eventTable[opcode] - callBack;
			this.onListenerRemoved(opcode);
		}

		private void dispatchEvent(ushort opcode, IMessage msg)
		{
			Delegate d;
			if (eventTable.TryGetValue(opcode, out d))
			{
				CallBack<IMessage> callBack = d as CallBack<IMessage>;
				if (callBack != null)
				{
					callBack(msg);
				}
				else
				{
					throw new Exception(string.Format("广播事件错误：事件{0}对应委托具有不同的类型", opcode));
				}
			}
		}

		public void Run(Session s, byte flag, ushort opcode, MemoryStream memoryStream)
		{
			OpcodeTypeComponent opcodeTypeComponent = Game.Scene.GetComponent<OpcodeTypeComponent>();
			object instance = opcodeTypeComponent.GetInstance(opcode);
			object message = this.Network.MessagePacker.DeserializeFrom(instance, memoryStream);

			if (OpcodeHelper.IsNeedDebugLogMessage(opcode))
			{
				Log.Msg(message);
			}

			if ((flag & 0x01) > 0)
			{
				IResponse response = message as IResponse;
				if (response == null)
				{
					throw new Exception($"flag is response, but hotfix message is not! {opcode}");
				}
				
				Action<IResponse> action;
				if (!this.requestCallback.TryGetValue(response.RpcId, out action))
				{
					return;
				}
				this.requestCallback.Remove(response.RpcId);

				action(response);
				return;
			}

			Game.Scene.GetComponent<MessageDispatcherComponent>().Handle(s, new MessageInfo(opcode, message));
		}
 
		private void Run(MemoryStream memoryStream)
		{
			memoryStream.Seek(Packet.MessageIndex, SeekOrigin.Begin);
			ushort opcode = BitConverter.ToUInt16(memoryStream.GetBuffer(), Packet.OpcodeIndex);
			
// #if !SERVER
// 			if (OpcodeHelper.IsClientHotfixMessage(opcode))
// 			{
// 				this.GetComponent<SessionCallbackComponent>().MessageCallback.Invoke(this, opcode, memoryStream);
// 				return;
// 			}
// #endif
			
			object message;
			try
			{
				OpcodeTypeComponent opcodeTypeComponent = this.Network.Entity.GetComponent<OpcodeTypeComponent>();
				object instance = opcodeTypeComponent.GetInstance(opcode);
				message = this.Network.MessagePacker.DeserializeFrom(instance, memoryStream);
				
				if (OpcodeHelper.IsNeedDebugLogMessage(opcode))
				{
					Log.Msg(message);
				}
			}
			catch (Exception e)
			{
				// 出现任何消息解析异常都要断开Session，防止客户端伪造消息
				Log.Error($"opcode: {opcode} {this.Network.Count} {e}, ip: {this.RemoteAddress}");
				this.Error = ErrorCode.ERR_PacketParserError;
				this.Network.Remove(this.Id);
				return;
			}

			RunMessage(opcode, message);
		}

		private void RunMessage(ushort opcode, object message)
		{
			this.dispatchEvent(opcode, (IMessage)message);
			/*//this.LastRecvTime = TimeHelper.Now();

			if (!(message is IResponse response))
			{
				this.Network.MessageDispatcher.Dispatch(this, opcode, message);
				return;
			}
			
#if SERVER
			if (message is IActorResponse)
			{
				this.Network.MessageDispatcher.Dispatch(this, opcode, message);
				return;
			}
#endif
            
			Action<IResponse> action;
			if (!this.requestCallback.TryGetValue(response.RpcId, out action))
			{
				throw new Exception($"not found rpc, response message: {StringHelper.MessageToStr(response)}");
			}
			this.requestCallback.Remove(response.RpcId);
            
			action(response);*/
		}
		
		public ETTask<IResponse> CallWithoutException(IRequest request)
		{
			int rpcId = ++RpcId;
			var tcs = new ETTaskCompletionSource<IResponse>();

			this.requestCallback[rpcId] = (response) =>
			{
				if (response is ErrorResponse errorResponse)
				{
					tcs.SetException(new Exception($"session close, errorcode: {errorResponse.Error} {errorResponse.Message}"));
					return;
				}
				tcs.SetResult(response);
			};

			request.RpcId = rpcId;
			this.Send(request);
			return tcs.Task;
		}

		public ETTask<IResponse> Call(IRequest request)
		{
			int rpcId = ++RpcId;
			var tcs = new ETTaskCompletionSource<IResponse>();

			this.requestCallback[rpcId] = (response) =>
			{
				if (ErrorCode.IsRpcNeedThrowException(response.Error))
				{
					tcs.SetException(new Exception($"Rpc Error: {request.GetType().FullName} {response.Error}"));
					return;
				}

				tcs.SetResult(response);
			};

			request.RpcId = rpcId;
			this.Send(request);
			return tcs.Task;
		}

		public ETTask<IResponse> Call(IRequest request, CancellationToken cancellationToken)
		{
			int rpcId = ++RpcId;
			var tcs = new ETTaskCompletionSource<IResponse>();

			this.requestCallback[rpcId] = (response) =>
			{
				if (ErrorCode.IsRpcNeedThrowException(response.Error))
				{
					tcs.SetException(new Exception($"Rpc Error: {request.GetType().FullName} {response.Error}"));
				}

				tcs.SetResult(response);
			};

			cancellationToken.Register(() => this.requestCallback.Remove(rpcId));

			request.RpcId = rpcId;
			this.Send(request);
			return tcs.Task;
		}

		public void Reply(IResponse message)
		{
			if (this.IsDisposed)
			{
				throw new Exception("session已经被Dispose了");
			}

			this.Send(message);
		}

		public int getMessageNumber(IMessage message)
        {
			OpcodeTypeComponent opcodeTypeComponent = this.Network.Entity.GetComponent<OpcodeTypeComponent>();
			ushort opcode = opcodeTypeComponent.GetOpcode(message.GetType());
			return opcode;
		}

		public void Send(IMessage message)
		{
			OpcodeTypeComponent opcodeTypeComponent = this.Network.Entity.GetComponent<OpcodeTypeComponent>();
			ushort opcode = opcodeTypeComponent.GetOpcode(message.GetType());
			this.Send(opcode, message);
		}
		
		public void Send(ushort opcode, object message)
		{
			if (this.IsDisposed)
			{
				throw new Exception("session已经被Dispose了");
			}
			
			if (OpcodeHelper.IsNeedDebugLogMessage(opcode) )
			{
				Log.Msg(message);
			}

			MemoryStream stream = this.Stream;
			
			stream.Seek(Packet.MessageIndex, SeekOrigin.Begin);
			stream.SetLength(Packet.MessageIndex);
			this.Network.MessagePacker.SerializeTo(message, stream);
			stream.Seek(0, SeekOrigin.Begin);
			
			opcodeBytes.WriteTo(0, opcode);
			Array.Copy(opcodeBytes, 0, stream.GetBuffer(), 0, opcodeBytes.Length);
			
			this.Send(stream);
		}

		public void Send(MemoryStream stream)
		{
			channel.Send(stream);
		}
	}
}