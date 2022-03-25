using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using LitJson;
using System.IO;

namespace ETModel
{
    public class MapConfigReader
    {
        public static List<Stage> stageList;
        public static String filePath;

        public static void initConfig()
        {
            filePath = Application.streamingAssetsPath + "/Json" + "/MapConfig.json";
            //正常解析
            if (File.Exists(filePath))
            {
                StreamReader sr = new StreamReader(filePath);
                string jsonStr = sr.ReadToEnd();
                sr.Close();
                PlayerManager playerManager = JsonMapper.ToObject<PlayerManager>(jsonStr);
                stageList = playerManager.unLockedNormalModelLevelList;
            }
            else
            {
                Debug.Log("MapConfig读取失败");
            }
        }

        public static Stage getSingleStage(int bigLevel, int level)
        {
            if(((bigLevel - 1) * 5 + level - 1) <= (stageList.Count - 1))
            {
                return stageList[(bigLevel - 1) * 5 + level - 1];
            }
            return null;
        }
    }

    public class Stage
    {
        public int[] mTowerIDList; //本关卡可以建的塔种类
        public int mTowerIDListLength; //建塔数组长度
        public int mTotalRound; //一共几波怪
    }


    public class PlayerManager
    {
        public List<Stage> unLockedNormalModelLevelList;//所有的小关卡
    }
}
