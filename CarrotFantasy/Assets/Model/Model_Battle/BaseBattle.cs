using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ETModel
{
    public abstract class BaseBattle
    {
        public EventDispatcher eventDispatcher { get; private set; }
        public int curFrameId { get; private set; }
        public Fix64 curClock = Fix64.Zero;
        private int uid;
        public bool isPause { get; set; }
        public bool isDoulbSpeed { get; private set; }

        public Dictionary<String, BaseBattleComponent> componentDic = new Dictionary<string, BaseBattleComponent>();
        public List<BaseBattleComponent> componentList = new List<BaseBattleComponent>();

        public BaseStateMachine stateMachine;

        public Fix64 oneFrameTime = new Fix64(0.2f);
        public Fix64 lastFrameTime = Fix64.Zero;

        public bool isStart = false;
        public bool isIgnoreViewListener = false; //目前用于视图监听器广播过程，服务端为true

        public BaseBattle()
        {
            this.eventDispatcher = new EventDispatcher();
            this.isPause = false;
            this.isDoulbSpeed = false;
            this.curFrameId = 0;
        }

        protected virtual void addListener() { }

        protected virtual void removeListener() { }

        public abstract void init();

        public abstract void initComponent(); //子类实现

        public BaseBattleComponent getComponent(String type)
        {
            if (type != null)
            {
                return componentDic[type];
            }
            return null;
        }

        public void addComponent(BaseBattleComponent component)
        {
            if (component != null)
            {
                if (componentDic.ContainsKey(component.componentType))
                {
                    //报错
                }
                else
                {
                    componentDic.Add(component.componentType, component);
                    componentList.Add(component);
                }
            }
        }

        public void removeComponent(BaseBattleComponent component)
        {
            if (component != null)
            {
                componentDic.Remove(component.componentType);
                componentList.Remove(component);
            }
        }

        public virtual void tick(Fix64 deltaTime)
        {
            this.onTick(deltaTime);
        }

        protected virtual void onTick(Fix64 time) //未来优化
        {
            if (this.isPause == true) return;
            curClock += time;
            this.lastFrameTime += time;
            //if (this.lastFrameTime < this.oneFrameTime)
            //{
            //    return;
            //}
            curFrameId += 1;
            for (int i = 0; i < componentList.Count; i++)
            {
                componentList[i].onTick(this.lastFrameTime);
            }
            stateMachine.onTick(time);
            for (int i = 0; i < componentList.Count; i++)
            {
                componentList[i].lateTick(this.lastFrameTime);
            }
            eventDispatcher.dispatchEvent(BattleEvent.AFTER_TICK);
            this.lastFrameTime = Fix64.Zero;
            //Debug.Log(String.Format("当前帧数为{0},游戏时间为{1}", this.curFrameId, this.curClock));
        }

        public virtual void startGame()
        {
            if (isStart == true) return;
            isStart = true;
            for (int i = 0; i < componentList.Count; i++)
            {
                componentList[i].start();
            }

            this.stateMachine.setCurrentState(BattleStateType.START_GAME);
        }

        public virtual void ClearGameInfo()
        {
            for (int i = componentList.Count - 1; i >= 0; i++)
            {
                componentList[i].clearInfo();
            }
            this.isStart = false;
            this.isDoulbSpeed = false;
            this.isPause = false;
            this.curFrameId = 0;
            this.lastFrameTime = Fix64.Zero;
            this.curClock = Fix64.Zero;
            this.uid = 0;
        }

        protected virtual void pauseTheGame()
        {
            this.isPause = !this.isPause;
        }

        public int getUid()
        {
            this.uid += 1;
            return this.uid;
        }

        public virtual void dispose()
        {
            this.stateMachine.dispose();
            for (int i = componentList.Count - 1; i >= 0; i--) //倒着去除
            {
                this.componentList[i].dispose();
            }
            this.componentList.Clear();
            this.componentDic.Clear();
            this.removeListener();
            GameObjectPool.getInstance().dispose();
            this.eventDispatcher.dispose();
        }
    }
}
