using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETModel
{
    public class PveStateMachine : BaseStateMachine
    {
        private bool isPauseGame;
        public PveStateMachine(BaseBattle battle):base(battle)
        {

        }

        protected override BaseBattleState createStateInstance(string type)
        {
            if (type.Equals(BattleStateType.START_GAME))
            {
                return new PveStartState(this,BattleStateType.START_GAME);
            }
            else if (type.Equals(BattleStateType.FIGHTINT))
            {
                return new PveFightingState(this,BattleStateType.FIGHTINT);
            }
            else if (type.Equals(BattleStateType.PRE_FIGHTINT))
            {
                return new PvePreFightingState(this,BattleStateType.PRE_FIGHTINT);
            }
            else if (type.Equals(BattleStateType.END_GAME))
            {
                return new PveFinishState(this,BattleStateType.END_GAME);
            }
            return null;
        }


    }
}
