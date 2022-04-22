using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ETModel
{
    public class UnitMoveComponent_Bullet_One : UnitMoveComponent_Bullet
    {

        public UnitMoveComponent_Bullet_One() : base()
        {
            this.unitComponentType = UnitComponentType.MOVE_BULLET_ONE;
        }

        public override void onTick(Fix64 deltaTime)
        {
            this.calcuMoveSpeed();
            base.onTick(deltaTime);
        }
    }
}
