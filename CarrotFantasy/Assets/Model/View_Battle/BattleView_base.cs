using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ETModel
{
    public class BattleView_base
    {
        public GameObject rootGameObject;

        public BaseBattle battle;
        public EventDispatcher eventDispatcher { get; private set; }

        public EventDispatcher bvEventDispatcher { get; private set; }

        protected Dictionary<String, BaseBattleViewComponent> componentDic = new Dictionary<string, BaseBattleViewComponent>();
        protected List<BaseBattleViewComponent> componentList = new List<BaseBattleViewComponent>();

        public bool isStart;
        public bool isGameObjectLoaded;

        public Vector3 initTran = new Vector3(1000, 1000, 0);

        public BattleView_base(BaseBattle battle)
        {
            battle.isIgnoreViewListener = false;
            this.battle = battle;

            this.eventDispatcher = this.battle.eventDispatcher;
            this.bvEventDispatcher = new EventDispatcher();
            this.isStart = false;
        }

        public virtual void addListener() { }

        public virtual void removeListener() { }

        public virtual void init() 
        {
            this.addListener();
        }

        public virtual void initComponents() //最后调用
        {
            for(int i = 0; i < this.componentList.Count; i++)
            {
               this.componentList[i].init();
            }
        }

        public void addComponent(BaseBattleViewComponent component)
        {
            if (component == null) return;
            this.componentDic.Add(component.componentType, component);
            this.componentList.Add(component);
        }

        public void removeComponent(BaseBattleViewComponent component)
        {
            if (component == null) return;
            component.dispose();
            bool isSuc1 = this.componentDic.Remove(component.componentType);
            bool isSuc2 = this.componentList.Remove(component);
            if(isSuc1 == false || isSuc2 == false)
            {
                //出问题
                
            }
        }

        public BaseBattleViewComponent getComponent(String type)
        {
            return this.componentDic[type];
        }

        public void onTick(float time)
        {
            if (this.battle.isPause == true) return;
            for(int i = 0; i <= componentList.Count - 1; i++)
            {
                this.componentList[i].onTick(time);
            }
        }

        public virtual void startGame()
        {
            if (this.isStart == true) return;
            for(int i = 0; i<= this.componentList.Count - 1; i++)
            {
                this.componentList[i].start();
            }
            this.isStart = true;
        }

        public virtual void clearGameInfo()
        {
            for (int i = this.componentList.Count - 1; i >= 0; i--)
            {
                this.componentList[i].clearGameInfo();
            }
        }

        public virtual void dispose()
        {
            this.removeListener();
            for(int i = this.componentList.Count - 1; i >= 0; i--)
            {
                this.componentList[i].dispose();
            }
            this.componentList.Clear();
            this.componentDic.Clear();
            AssetObjectPool.getInstance().dispose();
            GameViewObjectPool.getInstance().dispose();
            this.bvEventDispatcher.dispose();
        }
    }
}
