using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ETModel
{
    public class PveFinishState : BaseBattleState
    {
        private BattleMonsterComponent monsterComponent;
        private BattleDataComponent dataComponent;

        public PveFinishState(BaseStateMachine bstateMachine, string btype = null) : base(bstateMachine, btype)
        {

        }

        public override void init()
        {
            this.monsterComponent = (BattleMonsterComponent)GameManager.getInstance().baseBattle.getComponent(BattleComponentType.MonsterComponent);
            this.dataComponent = (BattleDataComponent)GameManager.getInstance().baseBattle.getComponent(BattleComponentType.DataComponent);
        }

        public override void stateIn()
        {
            if (this.dataComponent.CarrotIsDead())
            {
                this.dataComponent.gameOverByCarrotDead();
            }
            else
            {
                if (!this.monsterComponent.isCanNewMonsterWaves()) //击杀全部怪物了
                {
                    this.dataComponent.gameOverByMonsterDead();
                }
                else
                {
                    Debug.Log("结算状态出现错误");
                    return;
                }
            }
        }

        public override string onTick(Fix64 time)
        {
            return BattleStateType.END_GAME;
        }

        public override void dispose()
        {
            this.monsterComponent = null;
            this.dataComponent = null;
            base.dispose();
        }
    }
}
