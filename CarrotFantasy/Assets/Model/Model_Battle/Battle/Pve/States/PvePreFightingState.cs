using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ETModel
{
    public class PvePreFightingState : BaseBattleState
    {
        private bool isEnterFighting;

        private BattleMonsterComponent monsterComponent;
        private BattleSchedulerComponent scheComponent;
        private BattleDataComponent dataComponent; 

        public PvePreFightingState(BaseStateMachine bstateMachine, string btype) : base(bstateMachine, btype)
        {

        }

        public override void init()
        {
            base.init();
            this.monsterComponent = (BattleMonsterComponent)GameManager.getInstance().baseBattle.getComponent(BattleComponentType.MonsterComponent);
            this.scheComponent = (BattleSchedulerComponent)GameManager.getInstance().baseBattle.getComponent(BattleComponentType.SchedulerComponent);
            this.dataComponent = (BattleDataComponent)GameManager.getInstance().baseBattle.getComponent(BattleComponentType.DataComponent);
        }

        public override void stateIn()
        {
            if (this.monsterComponent.CheckIsHaveAnyMonsterSurvive() == true)
            {
                Debug.Log("进入战斗准备状态有问题");
                isEnterFighting = false;
                return;
            }
            this.dataComponent.WavesNumberChange();
            this.monsterComponent.buildNewWavesMonster();
            this.monsterComponent.scheId = scheComponent.delayExeMultipleTimes(this.monsterComponent.registerNewMonster, 2.0f);
            this.isEnterFighting = true;
        }

        public override string onTick(Fix64 time)
        {
            if(this.isEnterFighting == true)
            {
                return BattleStateType.FIGHTINT;
            }
            return BattleStateType.PRE_FIGHTINT;
        }

        public override void dispose()
        {
            base.dispose();
            this.monsterComponent = null;
            this.scheComponent = null;
            this.dataComponent = null;
        }
    }
}
