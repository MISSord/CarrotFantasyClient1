using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ETModel
{
    public class GameObjectPool
    {
        private static GameObjectPool gamePool;
        private Dictionary<String, List<BattleUnit>> curObjectDic = new Dictionary<String, List<BattleUnit>>();
        private Dictionary<String, List<BaseUnitComponent>> curUnitObjectDic = new Dictionary<string, List<BaseUnitComponent>>();

        public static GameObjectPool getInstance()
        {
            if(gamePool == null)
            {
                gamePool = new GameObjectPool();
                gamePool.init();
            }
            return gamePool;
        }

        public void init()
        {
            this.registerBattleUnit(BattleUnitType.MONSTER);
            this.registerBattleUnit(BattleUnitType.TOWER);
            this.registerBattleUnit(BattleUnitType.BULLET);

            this.registerUnitComponent(UnitComponentType.TRANSFORM);
            this.registerUnitComponent(UnitComponentType.MOVE_MONSTER);
            this.registerUnitComponent(UnitComponentType.MOVE_BULLET);
            this.registerUnitComponent(UnitComponentType.BEHIT);
        }

        public void registerBattleUnit(String name)
        {
            if (!curObjectDic.ContainsKey(name))
            {
                List<BattleUnit> battleList = new List<BattleUnit>();
                curObjectDic.Add(name, battleList);
            }
        }

        public void registerUnitComponent(String name)
        {
            if (!curUnitObjectDic.ContainsKey(name))
            {
                List<BaseUnitComponent> battleList = new List<BaseUnitComponent>();
                curUnitObjectDic.Add(name, battleList);
            }
        }

        public T getNewBattleUnit<T>(String name) where T :BattleUnit
        {
            List<BattleUnit> curList;
            if(!curObjectDic.TryGetValue(name, out curList))
            {
                Debug.LogError(String.Format("没有注册{0}", name));
                return null;
            }
            if (curList.Count == 0)
            {
                return null;
            }
            else
            {
                BattleUnit cur = curList[0];
                curList.RemoveAt(0);
                //Debug.Log(String.Format("{0}取出对象池，目前长度{1}", name, curList.Count));
                return (T)cur;
            }
        }

        public T getNewUnitComponent<T>(String name) where T : BaseUnitComponent
        {
            List<BaseUnitComponent> curList;
            if (!curUnitObjectDic.TryGetValue(name, out curList))
            {
                Debug.LogError(String.Format("没有注册{0}", name));
                return null;
            }
            if (curList.Count == 0)
            {
                return null;
            }
            else
            {
                BaseUnitComponent cur = curList[0];
                curList.RemoveAt(0);
                //Debug.Log(String.Format("{0}取出组件对象池，目前长度{1}", name, curList.Count));
                return (T)cur;
            }
        }


        public void pushObjectToPool(String name, BattleUnit unit)
        {
            List<BattleUnit> curList = this.curObjectDic[name];
            curList.Add(unit);
            //Debug.Log(String.Format("{0}放回到对象池，目前长度{1}", name, curList.Count));
        }

        public void pushObjectToPool(String name, BaseUnitComponent unit)
        {
            List<BaseUnitComponent> curList = this.curUnitObjectDic[name];
            curList.Add(unit);
            //Debug.Log(String.Format("{0}放回到组件对象池，目前长度{1}", name, curList.Count));
        }

        public void dispose()
        {
            foreach (KeyValuePair<String, List<BattleUnit>> info in this.curObjectDic)
            {
                for (int i = 0; i < info.Value.Count; i++)
                {
                    info.Value[i].dispose();
                }
            }
            this.curObjectDic.Clear();
            foreach (KeyValuePair<String, List<BaseUnitComponent>> info in this.curUnitObjectDic)
            {
                for (int i = 0; i < info.Value.Count; i++)
                {
                    info.Value[i].dispose();
                }
            }
            this.curUnitObjectDic.Clear();
        }
    }
}
