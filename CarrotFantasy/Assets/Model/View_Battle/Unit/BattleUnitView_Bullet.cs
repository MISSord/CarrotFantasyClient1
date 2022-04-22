using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace ETModel
{
    public class BattleUnitView_Bullet : BattleUnitView
    {
        private UnitMoveComponent_Bullet moveComponent;

        public override void init()
        {
            base.init();
            if(((BattleUnit_Bullet)this.unit).towerId == 4)
            {
                this.moveComponent = (UnitMoveComponent_Bullet)this.unit.getComponent(UnitComponentType.MOVE_BULLET);
            }
            else
            {
                this.moveComponent = (UnitMoveComponent_Bullet)this.unit.getComponent(UnitComponentType.MOVE_BULLET_ONE);
            }
            
        }

        public override void onTick(float deltaTime)
        {
            base.onTick(deltaTime);
            Fix64 arcsinOne = Fix64.Zero;
            if (moveComponent.moveSpeedX == Fix64.Zero)
            {
                if(moveComponent.moveSpeedY > Fix64.Zero)
                {
                    arcsinOne = Fix64.Pi / Fix64.Two;
                }
                else
                {
                    arcsinOne = -(Fix64.Pi / Fix64.Two);
                }
            }
            else
            {
                arcsinOne = Fix64.Atan(moveComponent.moveSpeedY / moveComponent.moveSpeedX);
                if (moveComponent.moveSpeedX < Fix64.Zero)
                {
                    arcsinOne = Fix64.Pi + arcsinOne;
                }
            }
            this.transform.eulerAngles = new Vector3(0, 0, (float)(arcsinOne * (Fix64.Semicircle / Fix64.Pi)));
        }
    }
}
