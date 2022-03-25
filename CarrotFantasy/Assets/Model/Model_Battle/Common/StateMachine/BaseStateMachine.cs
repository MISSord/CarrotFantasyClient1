using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ETModel
{
    public abstract class BaseStateMachine
    {
        private BaseBattleState currentState;
        private Dictionary<String, BaseBattleState> stateDic = new Dictionary<string, BaseBattleState>();
        public EventDispatcher eventDispatcher { get; private set; }

        public BaseStateMachine(BaseBattle battle)
        {
            this.eventDispatcher = battle.eventDispatcher;
        }

        protected abstract BaseBattleState createStateInstance(String type);//子类实现

        public BaseBattleState getState(String type)
        {
            BaseBattleState bBattle;
            if (stateDic.ContainsKey(type))
            {
                stateDic.TryGetValue(type, out bBattle);
            }
            else
            {
                bBattle = this.createStateInstance(type);
                bBattle.init();
                stateDic.Add(type, bBattle);
            }
            return bBattle;
        }

        protected virtual void leaveState()
        {
            if (currentState != null)
            {
                currentState.stateOut();
                Debug.Log(String.Format("退出旧的状态{0}", currentState.statetype));
                currentState = null;
            }
        }

        protected virtual void enterState(BaseBattleState lastState)
        {
            currentState = lastState;
            currentState.stateIn();
        }

        public virtual void setCurrentState(String stateType)
        {
            BaseBattleState lastState = this.getState(stateType);
            this.leaveState();
            this.enterState(lastState);
            Debug.Log(String.Format("进入新的状态{0}", lastState.statetype));
        }

        public BaseBattleState getCurrentState()
        {
            return currentState;
        }

        public void restartGame()
        {
            this.currentState.stateOut();
            foreach (KeyValuePair<String, BaseBattleState> stateInfo in stateDic)
            {
                stateInfo.Value.dispose();
            }
            this.stateDic.Clear();
        }

        public void onTick(Fix64 time)
        {
            if (currentState == null) return;
            String nextType = currentState.onTick(time);
            if (!String.Equals(nextType, currentState.getStateType()))
            {
                this.setCurrentState(nextType);
            }
        }

        public void dispose()
        {
            this.currentState.stateOut();
            foreach(KeyValuePair<String, BaseBattleState> stateInfo in stateDic)
            {
                stateInfo.Value.dispose();
            }
            this.stateDic.Clear();
            this.eventDispatcher = null;
        }
    }
}
