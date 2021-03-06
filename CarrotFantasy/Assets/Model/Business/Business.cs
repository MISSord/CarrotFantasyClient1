using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ETModel
{
    public class Business
    {
        private Dictionary<string, BaseServer> businessDic = new Dictionary<string, BaseServer>();
        private List<BaseServer> businessList = new List<BaseServer>();

        public EventDispatcher eventDispatcher { get; private set; }
        private static Business business;

        public bool isGameQuit = false;
        public static Business getInstance()
        {
            if (business == null)
            {
                business = new Business();
                business.eventDispatcher = new EventDispatcher();
            }
            return business;
        }

        public void init()
        {
            this.businessDic.Add(BusinessType.UIServer, UIServer.getInstance());
            this.businessDic.Add(BusinessType.AccountServer, AccountServer.getInstance());
            this.businessDic.Add(BusinessType.MapServer, MapServer.getInstance());
            this.businessDic.Add(BusinessType.RoomServer, RoomServer.getInstance());
            this.businessDic.Add(BusinessType.BattleParamServer, BattleParamServer.getInstance());

            this.eventDispatcher.addListener(CommonEventType.GAME_QUIT, this.dispose);
        }

        public void loadBusiness()
        {
            foreach (KeyValuePair<String, BaseServer> info in businessDic)
            {
                info.Value.loadModule();
                info.Value.addSocketListener();
                this.businessList.Add(info.Value);
            }
        }

        public void reloadBusiness()
        {
            foreach (KeyValuePair<string, BaseServer> info in businessDic)
            {
                info.Value.reloadModule();
            }
        }



        public void dispose()
        {
            for(int i = this.businessList.Count - 1; i >= 0; i--)
            {
                this.businessList[i].removeSocketListener();
                this.businessList[i].dispose();
            }
            this.businessDic.Clear();
            this.businessList.Clear();

            Server.connectionServer.dispose();
            Server.panelServer.dispose();
            Server.sceneServer.dispose();

            this.isGameQuit = true;
        }
    }
}
