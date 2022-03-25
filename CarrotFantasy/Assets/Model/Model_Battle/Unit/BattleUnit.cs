using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ETModel
{
    public abstract class BattleUnit
    {
        public BattleEventDispatcher eventDipatcher;
        public int uid { get; private set; }
        public String unitType { get; protected set; }
        public Fix64Vector2 birthPosition;

        protected Dictionary<String, BaseUnitComponent> componentDic = new Dictionary<string, BaseUnitComponent>();
        protected List<BaseUnitComponent> componentList = new List<BaseUnitComponent>();
        public Dictionary<String, Fix64> birthParam;
        public BaseBattle baseBattle;

        protected bool isPause;

        public BattleUnit(BaseBattle battle)
        {
            this.eventDipatcher = new BattleEventDispatcher(this, battle); //可以优化
            this.baseBattle = battle;
            this.isPause = false;
        }

        public virtual void loadInfo(int uid, Dictionary<String, Fix64> param, Fix64Vector2 birthPosition)  //先loadInfo 再initComponent
        {
            this.uid = uid;
            this.birthParam = param;
            this.birthPosition = birthPosition;
        }

        public virtual void init()
        {

        }

        public virtual void initComponents()
        {
            for (int i = 0; i <= componentList.Count - 1; i++)
            {
                this.componentList[i].init();
            }
        }

        public void addComponent(BaseUnitComponent unitComponent)
        {
            if(this.componentDic.ContainsKey(unitComponent.unitComponentType))
            {
                Debug.Log(String.Format("添加单元组件出错{0}{1}", this.unitType, unitComponent.unitComponentType));
            }
            else
            {
                this.componentDic.Add(unitComponent.unitComponentType, unitComponent);
                this.componentList.Add(unitComponent);
                unitComponent.loadUnit(this);
            }
        }

        public void removeComponent(BaseUnitComponent unitComponent)
        {
            if(unitComponent == null)
            {
                //出错了
                Debug.Log(String.Format("移除单元组件出错{0}{1}", this.unitType, unitComponent.unitComponentType));
                return;
            }
            this.removeComponent(unitComponent.unitComponentType);
        }

        public void removeComponent(String type)
        {
            this.componentDic.Remove(type);
        }

        public virtual void start()
        {
            for(int i = 0; i <= componentList.Count - 1; i++)
            {
                componentList[i].start();
            }
            this.isPause = false;
        }

        public String getUnitType()
        {
            return this.unitType;
        }

        public BaseUnitComponent getComponent(String name)
        {
            BaseUnitComponent compon;
            this.componentDic.TryGetValue(name, out compon);
            return compon;
        }

        public abstract void onTick(Fix64 deltaTime);

        public virtual void lateTick(Fix64 deltaTime) { }

        public virtual void ClearInfo()
        {
            this.birthPosition = Fix64Vector2.Zero;
            for (int i = 0; i <= componentList.Count - 1; i++)
            {
                componentList[i].dispose();
                GameObjectPool.getInstance().pushObjectToPool(componentList[i].unitComponentType, componentList[i]);
            }
            componentList.Clear();
            componentDic.Clear();
            this.eventDipatcher.dispose();
        }

        public virtual void dispose()
        {
            this.ClearInfo();
            this.baseBattle = null;
        }

    }
}
