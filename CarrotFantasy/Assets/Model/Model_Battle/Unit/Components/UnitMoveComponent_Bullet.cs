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
        private Fix64 moveSpeed;

        private Fix64 moveSpeedX;
        private Fix64 moveSpeedY;

        private UnitTransformComponent unitTrans;
        private Fix64Vector2 mapLeftBottomPosition;
        private Fix64Vector2 mapRightTopPosition;

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
        }

        public void registerMoveDirect(Fix64Vector2 target)
        {
            this.unitTrans = (UnitTransformComponent)this.unit.getComponent(UnitComponentType.TRANSFORM);
            Fix64Vector2 curDirect = target - this.unit.birthPosition;
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
            this.unitTrans.getLastFramePosition(out x, out y, out z);
            x += deltaTime * this.moveSpeedX;
            y += deltaTime * this.moveSpeedY;
            this.unitTrans.setPosition(x, y, z);

            if (x <= (this.mapLeftBottomPosition.X) || x >= (this.mapRightTopPosition.X) 
                || y <= (this.mapLeftBottomPosition.Y) || y >= (this.mapRightTopPosition.Y))
            {
                this.unit.eventDipatcher.dispatchEvent<BattleUnit_Bullet>(BattleEvent.BULLET_REMOVE, (BattleUnit_Bullet)this.unit);
            }
        }

        public override void dispose()
        {
            this.unitTrans = null;
        }
    }
}
