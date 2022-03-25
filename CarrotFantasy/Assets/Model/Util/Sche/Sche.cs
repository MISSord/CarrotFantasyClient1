using System;
using System.Collections.Generic;
using UnityEngine;

namespace ETModel
{
    public delegate void callBack();

    public static class Sche //延时调用
    {
        private static int curIndex = 0;
        private static Fix64 curTime = Fix64.Zero; //游戏开始的时间
        private static Fix64 lastResetTime = Fix64.Zero;

        private static Fix64 RESET_SCHE_INTERVAL_TIME = new Fix64(5); //每隔5秒清理一次
        private static List<ScheObject> scheList = new List<ScheObject>();
        private static Dictionary<int, ScheObject> scheDic = new Dictionary<int, ScheObject>();

        public static void tick(Fix64 timeDeltaTime)
        {
            curTime += timeDeltaTime;
            int curUnscheCount = 0; //当前不参与延时调用方法的数量
            for (int i = 0; i < scheList.Count; i++)
            {
                if (scheList[i].isUnscheduled == true)
                {
                    curUnscheCount += 1;
                    continue;
                }
                Fix64 evenLastTime = scheList[i].lastStartTime;
                Fix64 deltaClock = curTime - evenLastTime;
                if (deltaClock > scheList[i].intervalTime && scheList[i].ucallBack != null)
                {
                    scheList[i].ucallBack();
                    scheList[i].lastStartTime = curTime;
                    if (scheList[i].isOnce == true)
                    {
                        silenceSingleSche(scheList[i].uid);
                        curUnscheCount += 1;
                    }
                }
            }

            if (curTime - lastResetTime > RESET_SCHE_INTERVAL_TIME || curUnscheCount * 20 >= scheList.Count)
            {
                lastResetTime = curTime;
                if(curUnscheCount > 0)
                {
                    List<ScheObject> schelist = new List<ScheObject>();
                    foreach(ScheObject even in scheList)
                    {
                        if(even.isUnscheduled == false)
                        {
                            schelist.Add(even);
                        }
                    }
                    scheList = null;
                    scheList = schelist;
                }
            }
        }

        public static int delayExeOnceTimes(callBack call, float interval)
        {
            ScheObject sche = addSingleSche(call, interval);
            sche.isOnce = true;
            return sche.uid;
        }

        public static int delayExeMultipleTimes(callBack call, float interval)
        {
            ScheObject sche = addSingleSche(call, interval);
            sche.isOnce = false;
            return sche.uid;
        }

        private static ScheObject addSingleSche(callBack call, float interval)
        {
            int id = getUniqueId();
            ScheObject sche = new ScheObject(id, call, interval, curTime);
            scheList.Add(sche);
            scheDic.Add(id, sche);
            return sche;
        }

        public static void silenceSingleSche(int id)
        {
            ScheObject sche;
            if(scheDic.TryGetValue(id, out sche))
            {
                sche.isUnscheduled = true;
                scheDic.Remove(id);
            }
        }

        private static int getUniqueId()
        {
            curIndex += 1;
            return curIndex;
        }
    }
}
