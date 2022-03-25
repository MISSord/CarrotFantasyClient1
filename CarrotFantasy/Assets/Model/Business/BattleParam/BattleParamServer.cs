using System;
using System.Collections.Generic;
using LitJson;
using System.IO;
using UnityEngine;

namespace ETModel
{
    public class BattleParamServer : BaseServer
    {
        public static BattleParamServer battleParamServer;
        public SingleMapInfo curSingleMapInfo;
        public Stage curStage;

        public LevelInfo info;

        public int curBigLevel;
        public int curLevel;

        public bool isPVE = false;

        public static BattleParamServer getInstance()
        {
            if(battleParamServer == null)
            {
                battleParamServer = new BattleParamServer();
            }
            return battleParamServer;
        }

        public override void loadModule()
        {
            base.loadModule();
            this.addListener();
        }

        private void addListener()
        {
            Business.getInstance().eventDispatcher.addListener(CommonEventType.READY_START_PVE_GAME, this.getPVEBattleParams);
            Business.getInstance().eventDispatcher.addListener(CommonEventType.READY_START_PVP_GAME, this.getPVPBattleParams);
        }

        private void getPVEBattleParams()
        {
            this.isPVE = true;
            this.curBigLevel = MapServer.getInstance().curBigLevel;
            this.curLevel = MapServer.getInstance().curLevel;

            this.curStage = MapServer.getInstance().mapModel.getStage(this.curBigLevel, curLevel);
            this.curSingleMapInfo = MapServer.getInstance().mapModel.getSingleMapInfo(this.curBigLevel, curLevel);

            String path = "Level" + this.curBigLevel.ToString() + "_" + this.curLevel.ToString() + ".json";
            this.info = LoadLevelInfoFile(path);
        }

        private void getPVPBattleParams()
        {

        }

        private void removeListener()
        {
            Business.getInstance().eventDispatcher.removeListener(CommonEventType.READY_START_PVE_GAME, this.getPVEBattleParams);
            Business.getInstance().eventDispatcher.removeListener(CommonEventType.READY_START_PVP_GAME, this.getPVPBattleParams);
        }

        public override void dispose()
        {
            this.removeListener();
            base.dispose();
        }

        //读取关卡文件解析json转化为LevelInfo对象
        public LevelInfo LoadLevelInfoFile(string fileName)
        {
            LevelInfo levelInfo = new LevelInfo();
            string filePath = Application.streamingAssetsPath + "/Json/Level/" + fileName;
            if (File.Exists(filePath))
            {
                StreamReader sr = new StreamReader(filePath);
                string jsonStr = sr.ReadToEnd();
                sr.Close();
                levelInfo = JsonMapper.ToObject<LevelInfo>(jsonStr);
                return levelInfo;
            }
            Debug.Log("文件加载失败，加载路径是" + filePath);
            return null;
        }
    }
}
