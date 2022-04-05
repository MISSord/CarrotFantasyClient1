using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace ETModel
{
    public class GameViewObjectPool
    {
        private static GameViewObjectPool gamePool;
        private Dictionary<String, List<BattleUnitView>> curObjectDic = new Dictionary<String, List<BattleUnitView>>();
        private Dictionary<String, List<BaseUnitViewComponent>> curUnitObjectDic = new Dictionary<string, List<BaseUnitViewComponent>>();

        private Dictionary<String, List<GameObject>> curGameObjectDic = new Dictionary<string, List<GameObject>>();

        public static GameViewObjectPool getInstance()
        {
            if (gamePool == null)
            {
                gamePool = new GameViewObjectPool();
                gamePool.init();
            }
            return gamePool;
        }

        public void init()
        {
            this.registerBattleUnitView(BattleUnitViewType.Monster);
            this.registerBattleUnitView(BattleUnitViewType.Bullet);
            this.registerBattleUnitView(BattleUnitViewType.Tower);
            this.registerBattleUnitView(BattleUnitViewType.Item);
        }

        public void registerBattleUnitView(String name)
        {
            if (!curObjectDic.ContainsKey(name))
            {
                List<BattleUnitView> battleList = new List<BattleUnitView>();
                curObjectDic.Add(name, battleList);
            }
        }

        public void registerGameObject(String name)
        {
            if (!curGameObjectDic.ContainsKey(name))
            {
                List<GameObject> battleList = new List<GameObject>();
                curGameObjectDic.Add(name, battleList);
            }
        }

        public void registerUnitViewComponent(String name)
        {
            if (!curUnitObjectDic.ContainsKey(name))
            {
                List<BaseUnitViewComponent> battleList = new List<BaseUnitViewComponent>();
                curUnitObjectDic.Add(name, battleList);
            }
        }

        public GameObject getNewGameObject(String name)
        {
            List<GameObject> curList;
            if (!curGameObjectDic.TryGetValue(name, out curList))
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
                GameObject cur = curList[0];
                curList.RemoveAt(0);
                return cur;
            }
        }

        public T getNewBattleUnitView<T>(String name) where T : BattleUnitView
        {
            List<BattleUnitView> curList;
            if (!curObjectDic.TryGetValue(name, out curList))
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
                BattleUnitView cur = curList[0];
                curList.RemoveAt(0);
                return (T)cur;
            }
        }

        public T getNewUnitViewComponent<T>(String name) where T : BaseUnitViewComponent
        {
            List<BaseUnitViewComponent> curList;
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
                BaseUnitViewComponent cur = curList[0];
                curList.RemoveAt(0);
                return (T)cur;
            }
        }


        public void pushViewObjectToPool(String name, BattleUnitView unit)
        {
            List<BattleUnitView> curList = this.curObjectDic[name];
            curList.Add(unit);
            Debug.Log(String.Format("{0}放回到视图对象池，目前长度{1}", name, curList.Count));
        }

        public void pushViewObjectToPool(String name, BaseUnitViewComponent unit)
        {
            List<BaseUnitViewComponent> curList = this.curUnitObjectDic[name];
            curList.Add(unit);
            Debug.Log(String.Format("{0}放回到视图组件对象池，目前长度{1}", name, curList.Count));
        }

        public void pushGameObjectToPool(String name, GameObject node)
        {
            List<GameObject> curList = this.curGameObjectDic[name];
            node.transform.position = GameManager.getInstance().baseBattleView.initTran;
            curList.Add(node);
            Debug.Log(String.Format("{0}放回到视图游戏对象池，目前长度{1}", name, curList.Count));

        }

        public void dispose()
        {
            foreach(KeyValuePair<String, List<BattleUnitView>>info in this.curObjectDic)
            {
                for(int i = 0; i < info.Value.Count; i++)
                {
                    info.Value[i].dispose();
                }
            }
            this.curObjectDic.Clear();
            foreach (KeyValuePair<String, List<BaseUnitViewComponent>> info in this.curUnitObjectDic)
            {
                for (int i = 0; i < info.Value.Count; i++)
                {
                    info.Value[i].dispose();
                }
            }
            this.curUnitObjectDic.Clear();
            this.curGameObjectDic.Clear();
            GC.Collect();
        }
    }
}
