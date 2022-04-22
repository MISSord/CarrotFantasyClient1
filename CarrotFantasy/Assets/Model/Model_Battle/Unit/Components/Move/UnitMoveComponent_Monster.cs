using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ETModel
{
    public class UnitMoveComponent_Monster : BaseUnitComponent
    {
        private List<Fix64Vector2> monsterPointList;
        protected UnitTransformComponent unitTransform;
        public bool isReachCarrot { get; private set; }
        public Fix64 EndPointDistance { get; private set; }

        private int roadPointIndex;

        private Fix64 speed;

        private Fix64 moveTotalTime = Fix64.Zero;
        private Fix64 moveCurTime = Fix64.Zero;

        private Fix64 speedX;
        private Fix64 speedY;

        private Fix64 moveSpeed;

        public UnitMoveComponent_Monster()
        {
            this.unitComponentType = UnitComponentType.MOVE_MONSTER;
        }

        public override void init()
        {
            this.unitTransform = (UnitTransformComponent)this.unit.getComponent(UnitComponentType.TRANSFORM);
            this.roadPointIndex = 0;

            this.isReachCarrot = false;
            this.speed = this.unit.birthParam["speed"] != null ? this.unit.birthParam["speed"] : new Fix64(3);

            this.setSpeed();
        }

        public void loadInfo(List<Fix64Vector2> monsterPath, Fix64 distance)
        {
            monsterPointList = monsterPath;
            EndPointDistance = distance;
        }

        private void setSpeed()
        {
            Fix64 dicX = monsterPointList[roadPointIndex + 1].X - monsterPointList[roadPointIndex].X;
            Fix64 dicY = monsterPointList[roadPointIndex + 1].Y - monsterPointList[roadPointIndex].Y;

            Fix64 moveXTime = Fix64.Abs(dicX) / this.speed;
            Fix64 moveYTime = Fix64.Abs(dicY) / this.speed;

            this.moveTotalTime = moveXTime >= moveYTime ? moveXTime : moveYTime;

            Fix64 x, y, z;
            this.unitTransform.getLastFramePosition(out x, out y, out z);

            Fix64 dicXmove = monsterPointList[roadPointIndex + 1].X - x;
            Fix64 dicYmove = monsterPointList[roadPointIndex + 1].Y - y;

            this.speedX = dicXmove / this.moveTotalTime;
            this.speedY = dicYmove / this.moveTotalTime;

            if (this.moveTotalTime == moveXTime)
            {
                this.moveSpeed = this.speedX;
            }
            else if(this.moveTotalTime == moveXTime)
            {
                this.moveSpeed = this.speedY;
            }

            this.moveCurTime = Fix64.Zero;
        }

        public override void onTick(Fix64 deltaTime)
        {
            if (!this.isReachCarrot)
            {
                Fix64 x, y, z;
                this.unitTransform.getLastFramePosition(out x, out y, out z);
                Fix64 DiffDeltaTime = Fix64.Zero;

                if(this.moveCurTime + deltaTime >= this.moveTotalTime)
                {
                    deltaTime = this.moveTotalTime - this.moveCurTime;

                }
                x += (deltaTime * this.speedX);
                y += (deltaTime * this.speedY);

                this.moveCurTime += deltaTime;
                Fix64 speed = this.speed;
                if(speed >= Fix64.Zero)
                {
                    this.EndPointDistance -= this.speed;
                }
                else
                {
                    this.EndPointDistance += this.speed;
                }
                
                this.unitTransform.setPosition(x, y, z);
                if (this.moveCurTime >= this.moveTotalTime)
                {
                    //怪物的转向
                    if (roadPointIndex + 1 < monsterPointList.Count)
                    {
                        Fix64 xOffset = monsterPointList[roadPointIndex].X - monsterPointList[roadPointIndex + 1].X;
                        if (xOffset < Fix64.Zero)//右走
                        {
                            this.unitTransform.setFaceDirection(Fix64.Zero);
                        }
                        else if (xOffset > Fix64.Zero)
                        {
                            this.unitTransform.setFaceDirection(new Fix64(180));
                        }
                    }
                    roadPointIndex++;
                    if (roadPointIndex >= monsterPointList.Count - 1)
                    {
                        this.isReachCarrot = true;
                        this.unit.baseBattle.eventDispatcher.dispatchEvent(BattleEvent.CARROT_LIVE_REDUCE);
                        this.unit.eventDipatcher.dispatchEvent<BattleUnit_Monster>(BattleEvent.MONSTER_DIED, (BattleUnit_Monster)this.unit);
                        return;
                    }
                    this.setSpeed();
                }
            }
        }
    }
}
