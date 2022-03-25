using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace ETModel
{
    public class ConnectionServer
    {
        private Session sessionRealm;

        public void addListener(ushort opcode, CallBack<IMessage> callBack)
        {
            sessionRealm.addListener(opcode, callBack);
        }

        public void removeListener(ushort opcode, CallBack<IMessage> callBack)
        {
            sessionRealm.removeListener(opcode, callBack);
        }

        public void init()
        {
           sessionRealm  = Game.Scene.GetComponent<NetOuterComponent>().Create(GlobalConfigComponent.Instance.GlobalProto.Address);
        }

        public void start()
        {

        }

        public int getMessageNumber(IMessage message)
        {
            return this.sessionRealm.getMessageNumber(message);
        }

        public void Dispose()
        {
            this.sessionRealm.Dispose();
        }

        public void Send(IMessage message)
        {
            this.sessionRealm.Send(message);
        }

    }
}
