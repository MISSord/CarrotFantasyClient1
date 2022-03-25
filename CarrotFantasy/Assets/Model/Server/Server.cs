using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETModel
{
    public class Server
    {
        private static Server _server;
        public static PanelServer panelServer;
        public static SceneServer sceneServer;
        public static ConnectionServer connectionServer;

        public static Server getInstance()
        {
            if(_server == null)
            {
                _server = new Server();
            }
            return _server;
        }

        public void init()
        {
            connectionServer = new ConnectionServer();
            connectionServer.init();
            sceneServer = new SceneServer();
            sceneServer.init();
            panelServer = new PanelServer();
            panelServer.init();
        }
    }
}
