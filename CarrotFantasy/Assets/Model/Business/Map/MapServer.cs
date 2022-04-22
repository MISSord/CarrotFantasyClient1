using System;


namespace ETModel
{
    public class MapServer : BaseServer
    {
        private static MapServer mapServer;
        public String account;
        public MapModel mapModel;
        public EventDispatcher eventDispatcher;

        public SingleMapInfo unSaveMapInfo;

        public int curBigLevel { get; private set; }
        public int curLevel { get; private set; }

        public static MapServer getInstance()
        {
            if (mapServer == null)
            {
                mapServer = new MapServer();
                mapServer.eventDispatcher = new EventDispatcher();
                mapServer.mapModel = new MapModel(mapServer.eventDispatcher);
            }
            return mapServer;
        }

        public override void loadModule()
        {
            base.loadModule();
            this.addListener();

            this.curBigLevel = 0;
            this.curLevel = 0;
        }

        private void addListener()
        {
            AccountServer.getInstance().eventDispatcher.addListener(AccountServer.LOGIN_SUCCESS, this.sendGetUserInfo);
        }

        private void removeListener()
        {
            AccountServer.getInstance().eventDispatcher.removeListener(AccountServer.LOGIN_SUCCESS, this.sendGetUserInfo);
        }

        public override void addSocketListener()
        {
            Server.connectionServer.addListener(HotfixOpcode.A1001_GetUserInfo_G2C, this.notifyUserInfo);
            Server.connectionServer.addListener(HotfixOpcode.A1002_SetSingleMapInfo_G2C, this.notifySetSingleMapInfo);
            Server.connectionServer.addListener(HotfixOpcode.A1003_GameMapInfo_G2C, this.notifyGameMapInfo);
        }

        public override void removeSocketListener()
        {
            Server.connectionServer.removeListener(HotfixOpcode.A1001_GetUserInfo_G2C, this.notifyUserInfo);
            Server.connectionServer.removeListener(HotfixOpcode.A1002_SetSingleMapInfo_G2C, this.notifySetSingleMapInfo);
            Server.connectionServer.removeListener(HotfixOpcode.A1003_GameMapInfo_G2C, this.notifyGameMapInfo);
        }

        private void notifyUserInfo(IMessage message)
        {
            A1001_GetUserInfo_G2C msg = (A1001_GetUserInfo_G2C)message;
            if (msg.Error == ErrorCode.ERR_Success)
            {
                this.mapModel.parseMapInfo(msg.MapInfo);
            }
        }

        private void notifySetSingleMapInfo(IMessage message)
        {
            A1002_SetSingleMapInfo_G2C msg = (A1002_SetSingleMapInfo_G2C)message;
            if (msg.Error == ErrorCode.ERR_Success)
            {
                this.mapModel.updateSingleMapInfo(this.unSaveMapInfo);
                this.mapModel.updateSingleMapInfoUnLockState(msg.BigLevelId, msg.LevelId, msg.UnLocked);
                this.unSaveMapInfo = null;
            }
        }

        private void notifyGameMapInfo(IMessage message)
        {
            A1003_GameMapInfo_G2C msg = (A1003_GameMapInfo_G2C)message;
            if (msg.Error == ErrorCode.ERR_Success)
            {
                //this.eventDispatcher.dispatchEvent(MapEventType.CAN_START_GAME);
                Business.getInstance().eventDispatcher.dispatchEvent(CommonEventType.READY_START_PVE_GAME);
                Server.sceneServer.loadScene(BaseSceneType.BattleScene, null);
                UIServer.getInstance().showLoadingPanel();
            }
            else
            {
                //this.eventDispatcher.dispatchEvent(MapEventType.CANT_START_GAME);
            }
        }


        public override void dispose()
        {
            this.mapModel.dispose();
            this.removeListener();
        }

        public void sendGetUserInfo()
        {
            Server.connectionServer.Send(new A1001_GetUserInfo_C2G());
        }

        public void sendSetSingleMapInfo(SingleMapInfo unSaveMapInfo)
        {
            A1002_SetSingleMapInfo_C2G msg = new A1002_SetSingleMapInfo_C2G();
            this.unSaveMapInfo = unSaveMapInfo;
            msg.BigLevelId = unSaveMapInfo.bigLevelId;
            msg.LevelId = unSaveMapInfo.levelId;
            msg.CarrotState = unSaveMapInfo.carrotState;
            msg.IsAllClear = unSaveMapInfo.isAllClear;
            msg.UnLocked = unSaveMapInfo.unLocked;
            Server.connectionServer.Send(msg);
        }

        public void sendGameMapInfo(int bigLevel, int level)
        {
            A1003_GameMapInfo_C2G msg = new A1003_GameMapInfo_C2G();
            msg.BigLevelId = bigLevel;
            msg.LevelId = level;

            this.curBigLevel = bigLevel;
            this.curLevel = level;

            Server.connectionServer.Send(msg);
        }
    }
}
