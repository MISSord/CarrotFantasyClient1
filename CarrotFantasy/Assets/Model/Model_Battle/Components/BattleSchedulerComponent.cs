using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETModel
{
    public class BattleSchedulerComponent : BaseBattleComponent
    {
        private List<ScheObject> scheList = new List<ScheObject>();
        private Dictionary<int, ScheObject> scheDic = new Dictionary<int, ScheObject>();
        private BaseBattleComponent logComponent;

        private Fix64 lastResetTime = Fix64.Zero;
        private int curIndex = 0;
        private Fix64 RESET_SCHE_INTERVAL_TIME = new Fix64(5); //每隔5秒清理一次

        public BattleSchedulerComponent(BaseBattle bBattle) : base(bBattle)
        {
            this.componentType = BattleComponentType.SchedulerComponent;
        }

        public override void init()
        {
            lastResetTime = onClock();
            //logComponent = baseBattle.getComponent(BattleComponentType.LogComponent);
        }

        public Fix64 onClock() //战斗模块开始的时间
        {
            return baseBattle.curClock;
        }

        public override void onTick(Fix64 time)
        {
            Fix64 curTime = this.onClock();
            int curUnscheCount = 0; //当前不参与延时调用方法的数量
            for(int i = 0; i < scheList.Count; i++)
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
                if (curUnscheCount > 0)
                {
                    List<ScheObject> schelist = new List<ScheObject>();
                    foreach (ScheObject even in scheList)
                    {
                        if (even.isUnscheduled == false)
                        {
                            schelist.Add(even);
                        }
                    }
                    scheList = null;
                    scheList = schelist;
                }
            }
        }

        public int delayExeOnceTimes(callBack call, float interval)
        {
            ScheObject sche = this.addSingleSche(call, interval);
            sche.isOnce = true;
            return sche.uid;
        }

        public int delayExeMultipleTimes(callBack call, float interval)
        {
            ScheObject sche = this.addSingleSche(call, interval);
            sche.isOnce = false;
            return sche.uid;
        }

        private ScheObject addSingleSche(callBack call, float interval)
        {
            int id = getUniqueId();
            ScheObject sche = new ScheObject(id, call, interval, this.onClock());
            this.scheList.Add(sche);
            this.scheDic.Add(id, sche);
            return sche;
        }

        public void silenceSingleSche(int id)
        {
            ScheObject sche;
            if (scheDic.TryGetValue(id, out sche))
            {
                sche.isUnscheduled = true;
                scheDic.Remove(id);
            }
        }

        public override void clearInfo()
        {
            this.scheList.Clear();
            this.scheDic.Clear();
            this.curIndex = 0;
        }

        public override void dispose()
        {
            this.clearInfo();
            base.dispose();
        }

        private int getUniqueId()
        {
            curIndex += 1;
            return curIndex;
        }
    }
}
