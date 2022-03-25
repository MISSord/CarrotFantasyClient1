using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETModel
{
    public static class MapInfoHelper
    {
        //格式
        // #1,1,1,1,1#1,2,1,1,1 #
        // 第一个为大关卡id 第二个为小关卡id，
        // 第三个为萝卜状态 1为正常 2为银 3为金, 0为没有
        // 第四个为是否全部清除道具 1为是，2为否
        // 第五个为是否解锁 1为是，2为否

        private static char bigInfoSplitSign = '#';
        private static String normalInfoSplitSign = ",";
        private static String blankMapInfo = "#{0},{1},{2},{3},{4}";

        private static String[] newInfo = new String[15] {
            "#1,1,0,2,1","#1,2,0,2,2","#1,3,0,2,2","#1,4,0,2,2","#1,5,0,2,2",
            "#2,1,0,2,1","#2,2,0,2,2","#2,3,0,2,2","#2,4,0,2,2","#2,5,0,2,2",
            "#3,1,0,2,1","#3,2,0,2,2","#3,3,0,2,2","#3,4,0,2,2","#3,5,0,2,2",
        };

        public static String getInitMapInfo()
        {
            StringBuilder allInfo = new StringBuilder(newInfo[0]);
            for (int i = 1; i <= newInfo.Length - 1; i++)
            {
                allInfo.Append(newInfo[i]);
            }
            return allInfo.ToString();
        }

        public static String mapInfoConvertString(SingleMapInfo mapInfo)
        {
            return String.Format(blankMapInfo, mapInfo.bigLevelId, mapInfo.levelId, mapInfo.carrotState, mapInfo.isAllClear, mapInfo.unLocked);
        }

        public static String getNewMapInfo(String oldMapInfo, SingleMapInfo mapInfo)
        {
            StringBuilder newAllInfo = new StringBuilder();
            String[] allInfo = oldMapInfo.Split(bigInfoSplitSign);
            int level = (mapInfo.bigLevelId - 1) * 5 + mapInfo.levelId;
            String newInfo = mapInfoConvertString(mapInfo);
            allInfo[level] = newInfo;
            for (int i = 1; i <= allInfo.Length - 1; i++)
            {
                newAllInfo.Append(newInfo[i]);
            }
            return newAllInfo.ToString();
        }

        public class SingleMapInfo
        {
            public int isAllClear;//是否清空此关卡道具 1为清空2为没有清空
            public int carrotState;//萝卜状态
            public int levelId;//小关卡ID
            public int bigLevelId;//大关卡ID
            public int unLocked;//此关卡是否解锁 1为解锁 2为未解锁
        }
    }
}
