using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETModel
{
    public class MainScene : BaseScene
    {
        
        public MainScene(BaseSceneType type, string name, Dictionary<string, dynamic> param) : base(type, name, param)
        {
            this.prefabUrl = null;
        }

        public override void init()
        {
            base.init();
            StartLoadPanel panel = new StartLoadPanel(null);
            Server.panelServer.showPanel(panel);
            Sche.delayExeOnceTimes(() => { 
                panel.autoClose();
                Server.panelServer.showPanel(new MainPanel(null));
                if (AccountServer.getInstance().userId == 0)
                {
                    Server.panelServer.showPanel(new LoginPanel(null));
                }
            }, 2f);

        }
    }
}
