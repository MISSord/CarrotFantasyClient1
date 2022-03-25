using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETModel
{
    public class PveFightingState : BaseBattleState
    {
        private BattleMonsterComponent monsterComponent;
        private BattleDataComponent dataComponent;

        public PveFightingState(BaseStateMachine bstateMachine, string btype = null) : base(bstateMachine, btype)
        {

        }

        public override void init()
        {
            base.init();
            this.monsterComponent = (BattleMonsterComponent)GameManager.getInstance().baseBattle.getComponent(BattleComponentType.MonsterComponent);
            this.dataComponent = (BattleDataComponent)GameManager.getInstance().baseBattle.getComponent(BattleComponentType.DataComponent);
        }

        public override void stateIn()
        {
            base.stateIn();
        }

        public override string onTick(Fix64 time)
        {
            if (this.dataComponent.CarrotIsDead())
            {
                return BattleStateType.END_GAME;
            }
            if (this.monsterComponent.CheckIsHaveAnyMonsterSurvive())
            {
                return BattleStateType.FIGHTINT;
            }
            else
            {
                if (this.monsterComponent.isCanNewMonsterWaves()) //还有怪物需要生产
                {
                    return BattleStateType.PRE_FIGHTINT;
                }
                else
                {
                    return BattleStateType.END_GAME;
                }
            }
        }

        public override void dispose()
        {
            base.dispose();
            this.monsterComponent = null;
            this.dataComponent = null;
        }
    }
}
