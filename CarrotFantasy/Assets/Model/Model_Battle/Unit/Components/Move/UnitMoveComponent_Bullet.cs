using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ETModel
{
    public class UnitMoveComponent_Bullet : BaseUnitComponent
    {
        protected Fix64 moveSpeed;

        public Fix64 moveSpeedX { get; protected set; }
        public Fix64 moveSpeedY { get; protected set; }

 
        protected Fix64Vector2 mapLeftBottomPosition;
        protected Fix64Vector2 mapRightTopPosition;

        protected UnitTransformComponent unitTran;

        protected BattleUnit unitTarget;
        protected UnitTransformComponent unitTranTarget;

        public UnitMoveComponent_Bullet()
        {
            this.unitComponentType = UnitComponentType.MOVE_BULLET;
            BattleMapComponent map = (BattleMapComponent)GameManager.getInstance().baseBattle.getComponent(BattleComponentType.MapComponent);
            mapLeftBottomPosition = map.mapLeftBottomPosition;
            mapRightTopPosition = map.mapRightTopPosition;
        }

        public override void init()
        {
            this.moveSpeed = ((BattleUnit_Bullet)unit).moveSpeed;
            this.unitTran = (UnitTransformComponent)this.unit.getComponent(UnitComponentType.TRANSFORM);
        }

        public virtual void registerMoveDirect(BattleUnit unit)
        {
            this.unitTarget = unit;
            this.unitTranTarget = (UnitTransformComponent)this.unitTarget.getComponent(UnitComponentType.TRANSFORM);
            this.calcuMoveSpeed();
        }

        public virtual void removeMoveDirect(BattleUnit unit) 
        {
            if (unit == unitTarget) this.unitTarget = null;
        }

        public virtual void calcuMoveSpeed()
        {
            if (this.unitTarget == null) return;
            if (this.unitTran == null) return;
            Fix64Vector2 targetPosition = new Fix64Vector2(this.unitTranTarget.lastFrameX, this.unitTranTarget.lastFrameY);
            Fix64Vector2 curPosition = new Fix64Vector2(this.unitTran.lastFrameX, this.unitTran.lastFrameY);

            Fix64Vector2 curDirect = targetPosition - curPosition;
            Fix64 longSide2 = curDirect.X * curDirect.X + curDirect.Y * curDirect.Y;
            Fix64 longSide = Fix64.Sqrt(longSide2);
            Fix64 sinOne = curDirect.X / longSide;
            Fix64 cosOne = curDirect.Y / longSide;
            this.moveSpeedX = sinOne * this.moveSpeed;
            this.moveSpeedY = cosOne * this.moveSpeed;
        }

        public override void onTick(Fix64 deltaTime)
        {
            Fix64 x, y, z;
            this.unitTran.getLastFramePosition(out x, out y, out z);
            x += deltaTime * this.moveSpeedX;
            y += deltaTime * this.moveSpeedY;
            this.unitTran.setPosition(x, y, z);

            if (x <= (this.mapLeftBottomPosition.X) || x >= (this.mapRightTopPosition.X) 
                || y <= (this.mapLeftBottomPosition.Y) || y >= (this.mapRightTopPosition.Y))
            {
                this.unit.eventDipatcher.dispatchEvent<BattleUnit_Bullet>(BattleEvent.BULLET_REMOVE, (BattleUnit_Bullet)this.unit);
            }
        }

        public override void dispose()
        {
            this.unitTran = null;
            this.unitTranTarget = null;
            this.unitTarget = null;
        }
    }
}
