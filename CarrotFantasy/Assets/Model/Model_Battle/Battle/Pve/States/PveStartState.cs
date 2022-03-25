using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETModel
{
    public class PveStartState : BaseBattleState
    {
        private bool isTimeToPreFighting;
        private int scheId;
        public PveStartState(BaseStateMachine bstateMachine, string btype) : base(bstateMachine, btype)
        {
            this.isTimeToPreFighting = false;
        }

        public override void stateIn()
        {
            base.stateIn();
            BattleSchedulerComponent sch =  (BattleSchedulerComponent)GameManager.getInstance().baseBattle.getComponent(BattleComponentType.SchedulerComponent);
            this.scheId = sch.delayExeOnceTimes(() => { this.isTimeToPreFighting = true; }, 4.0f);
            GameManager.getInstance().baseBattle.eventDispatcher.dispatchEvent(BattleEvent.START_GAME);
        }

        public override string onTick(Fix64 time)
        {
            if(this.isTimeToPreFighting == true)
            {
                return BattleStateType.PRE_FIGHTINT;
            }
            return BattleStateType.START_GAME;
        }

        public override void stateOut()
        {
            //双重保险
            BattleSchedulerComponent sch = (BattleSchedulerComponent)GameManager.getInstance().baseBattle.getComponent(BattleComponentType.SchedulerComponent);
            sch.silenceSingleSche(this.scheId);
        }

        public override void dispose()
        {
            base.dispose();

        }

    }
}
