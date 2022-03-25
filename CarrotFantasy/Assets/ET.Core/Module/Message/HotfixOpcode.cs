using ETModel;
namespace ETModel
{
//测试向服务器发送消息
	[Message(HotfixOpcode.C2G_TestMessage)]
	public partial class C2G_TestMessage : IRequest {}

//测试向服务器返回消息
	[Message(HotfixOpcode.G2C_TestMessage)]
	public partial class G2C_TestMessage : IResponse {}

//ET----
	[Message(HotfixOpcode.C2R_Login)]
	public partial class C2R_Login : IRequest {}

	[Message(HotfixOpcode.R2C_Login)]
	public partial class R2C_Login : IResponse {}

	[Message(HotfixOpcode.C2G_LoginGate)]
	public partial class C2G_LoginGate : IRequest {}

	[Message(HotfixOpcode.G2C_LoginGate)]
	public partial class G2C_LoginGate : IResponse {}

	[Message(HotfixOpcode.G2C_TestHotfixMessage)]
	public partial class G2C_TestHotfixMessage : IMessage {}

	[Message(HotfixOpcode.C2M_TestActorRequest)]
	public partial class C2M_TestActorRequest : IActorLocationRequest {}

	[Message(HotfixOpcode.M2C_TestActorResponse)]
	public partial class M2C_TestActorResponse : IActorLocationResponse {}

	[Message(HotfixOpcode.PlayerInfo)]
	public partial class PlayerInfo : IMessage {}

	[Message(HotfixOpcode.C2G_PlayerInfo)]
	public partial class C2G_PlayerInfo : IRequest {}

	[Message(HotfixOpcode.G2C_PlayerInfo)]
	public partial class G2C_PlayerInfo : IResponse {}

	[Message(HotfixOpcode.C2G_LoginGate_Req)]
	public partial class C2G_LoginGate_Req : IRequest {}

	[Message(HotfixOpcode.G2C_LoginGate_Back)]
	public partial class G2C_LoginGate_Back : IResponse {}

//客户端登陆网关请求
	[Message(HotfixOpcode.A0003_LoginGate_C2G)]
	public partial class A0003_LoginGate_C2G : IRequest {}

//客户端登陆网关返回
	[Message(HotfixOpcode.A0003_LoginGate_G2C)]
	public partial class A0003_LoginGate_G2C : IResponse {}

//客户端登陆认证请求
	[Message(HotfixOpcode.A0002_Login_C2R)]
	public partial class A0002_Login_C2R : IRequest {}

//客户端登陆认证返回
	[Message(HotfixOpcode.A0002_Login_R2C)]
	public partial class A0002_Login_R2C : IResponse {}

//客户端注册请求
	[Message(HotfixOpcode.A0001_Register_C2R)]
	public partial class A0001_Register_C2R : IRequest {}

//客户端注册请求回复
	[Message(HotfixOpcode.A0001_Register_R2C)]
	public partial class A0001_Register_R2C : IResponse {}

//获取用户信息
	[Message(HotfixOpcode.A1001_GetUserInfo_C2G)]
	public partial class A1001_GetUserInfo_C2G : IRequest {}

//返回用户信息
//RpcID,Error,Message是actor RPC消息固有的
	[Message(HotfixOpcode.A1001_GetUserInfo_G2C)]
	public partial class A1001_GetUserInfo_G2C : IResponse {}

//设置用户信息
	[Message(HotfixOpcode.A1002_SetSingleMapInfo_C2G)]
	public partial class A1002_SetSingleMapInfo_C2G : IRequest {}

//返回设置用户信息 //大小等级有可能为零
	[Message(HotfixOpcode.A1002_SetSingleMapInfo_G2C)]
	public partial class A1002_SetSingleMapInfo_G2C : IResponse {}

//开始游戏前发送游戏信息
	[Message(HotfixOpcode.A1003_GameMapInfo_C2G)]
	public partial class A1003_GameMapInfo_C2G : IRequest {}

	[Message(HotfixOpcode.A1003_GameMapInfo_G2C)]
	public partial class A1003_GameMapInfo_G2C : IResponse {}

//==>匹配玩家并进入游戏房间
//玩家信息
	[Message(HotfixOpcode.GamerInfo)]
	public partial class GamerInfo {}

//返回大厅
	[Message(HotfixOpcode.C2G_ReturnLobby_Ntt)]
	public partial class C2G_ReturnLobby_Ntt : IMessage {}

//匹配模块
	[Message(HotfixOpcode.C2G_StartMatch_Req)]
	public partial class C2G_StartMatch_Req : IRequest {}

	[Message(HotfixOpcode.G2C_StartMatch_Back)]
	public partial class G2C_StartMatch_Back : IResponse {}

	[Message(HotfixOpcode.Actor_LandMatcherPlusOne_NTT)]
	public partial class Actor_LandMatcherPlusOne_NTT : IActorMessage {}

	[Message(HotfixOpcode.Actor_LandMatcherReduceOne_NTT)]
	public partial class Actor_LandMatcherReduceOne_NTT : IActorMessage {}

//进入房间(广播)
	[Message(HotfixOpcode.Actor_GamerEnterRoom_Ntt)]
	public partial class Actor_GamerEnterRoom_Ntt : IActorMessage {}

//退出房间(广播)
	[Message(HotfixOpcode.Actor_GamerExitRoom_Ntt)]
	public partial class Actor_GamerExitRoom_Ntt : IActorMessage {}

//匹配玩家并进入游戏房间 <==
//准备游戏消息
	[Message(HotfixOpcode.Actor_GamerReady_Landlords)]
	public partial class Actor_GamerReady_Landlords : IActorMessage {}

}
namespace ETModel
{
	public static partial class HotfixOpcode
	{
		 public const ushort C2G_TestMessage = 10001;
		 public const ushort G2C_TestMessage = 10002;
		 public const ushort C2R_Login = 10003;
		 public const ushort R2C_Login = 10004;
		 public const ushort C2G_LoginGate = 10005;
		 public const ushort G2C_LoginGate = 10006;
		 public const ushort G2C_TestHotfixMessage = 10007;
		 public const ushort C2M_TestActorRequest = 10008;
		 public const ushort M2C_TestActorResponse = 10009;
		 public const ushort PlayerInfo = 10010;
		 public const ushort C2G_PlayerInfo = 10011;
		 public const ushort G2C_PlayerInfo = 10012;
		 public const ushort C2G_LoginGate_Req = 10013;
		 public const ushort G2C_LoginGate_Back = 10014;
		 public const ushort A0003_LoginGate_C2G = 10015;
		 public const ushort A0003_LoginGate_G2C = 10016;
		 public const ushort A0002_Login_C2R = 10017;
		 public const ushort A0002_Login_R2C = 10018;
		 public const ushort A0001_Register_C2R = 10019;
		 public const ushort A0001_Register_R2C = 10020;
		 public const ushort A1001_GetUserInfo_C2G = 10021;
		 public const ushort A1001_GetUserInfo_G2C = 10022;
		 public const ushort A1002_SetSingleMapInfo_C2G = 10023;
		 public const ushort A1002_SetSingleMapInfo_G2C = 10024;
		 public const ushort A1003_GameMapInfo_C2G = 10025;
		 public const ushort A1003_GameMapInfo_G2C = 10026;
		 public const ushort GamerInfo = 10027;
		 public const ushort C2G_ReturnLobby_Ntt = 10028;
		 public const ushort C2G_StartMatch_Req = 10029;
		 public const ushort G2C_StartMatch_Back = 10030;
		 public const ushort Actor_LandMatcherPlusOne_NTT = 10031;
		 public const ushort Actor_LandMatcherReduceOne_NTT = 10032;
		 public const ushort Actor_GamerEnterRoom_Ntt = 10033;
		 public const ushort Actor_GamerExitRoom_Ntt = 10034;
		 public const ushort Actor_GamerReady_Landlords = 10035;
	}
}
