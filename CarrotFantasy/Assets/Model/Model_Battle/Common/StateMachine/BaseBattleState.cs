using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETModel
{
    public abstract class BaseBattleState : BaseState
    {
        public BaseStateMachine stateMachine;
        public BaseBattleState(BaseStateMachine bstateMachine, String btype) : base(btype)
        {
            stateMachine = bstateMachine;
        }
    }
}
