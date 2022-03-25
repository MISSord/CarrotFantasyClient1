using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ETModel
{
    public class BattleUnit_Monster : BattleUnit
    {
        public int curLive; //怪物血量
        public int totalLive; //怪物总血量
        private UnitMoveComponent_Monster moveTrans;
        protected UnitTransformComponent unitTransform;

        private List<int> haveBeHit;

        public int monsterId { get; private set; }

        public int curLevel { get; private set; }

        public Fix64 EndPointDistance { get; private set; }

        public BattleUnit_Monster(BaseBattle battle) : base(battle)
        {
            this.unitType = BattleUnitType.MONSTER;
            this.haveBeHit = new List<int>();
        }

        public override void loadInfo(int uid, Dictionary<string, Fix64> param, Fix64Vector2 birthPosition)
        {
            base.loadInfo(uid, param, birthPosition);
            this.curLive = (int)param["live"];
            this.totalLive = (int)param["live"];
        }

        public void loadInfo2(int curLevel, int monsterId)
        {
            this.curLevel = curLevel;
            this.monsterId = monsterId;
        }

        public void loadInfo3(List<Fix64Vector2> monsterPath, Fix64 distance)
        {
            this.moveTrans.loadInfo(monsterPath, distance);
        }

        public override void init()
        {
            this.unitTransform = GameObjectPool.getInstance().getNewUnitComponent<UnitTransformComponent>(UnitComponentType.TRANSFORM);
            if (this.unitTransform == null)
            {
                this.unitTransform = new UnitTransformComponent();
            }
            this.moveTrans = GameObjectPool.getInstance().getNewUnitComponent<UnitMoveComponent_Monster>(UnitComponentType.MOVE_MONSTER);
            if (this.moveTrans == null)
            {
                this.moveTrans = new UnitMoveComponent_Monster();
            }
            UnitBeHitComponent beHit = GameObjectPool.getInstance().getNewUnitComponent<UnitBeHitComponent>(UnitComponentType.BEHIT);
            if (beHit == null)
            {
                beHit = new UnitBeHitComponent();
            }
            this.addComponent(this.unitTransform);
            this.addComponent(this.moveTrans);
            this.addComponent(beHit);

            beHit.registerBeHitCallBack(this.beHitCallBack);
        }

        public override void initComponents()
        {
            base.initComponents();
            this.unitTransform.setBodyRadius(new Fix64(0.2f));
        }

        public void beHitCallBack(BattleUnit battleUnit)
        {
            if (battleUnit.unitType.Equals(BattleUnitType.BULLET))
            {
                BattleUnit_Bullet bullet = (BattleUnit_Bullet)battleUnit;
                if (this.haveBeHit.Contains(bullet.uid))
                {
                    return;
                }
                this.haveBeHit.Add(bullet.uid);
                this.curLive -= bullet.damage;
                this.eventDipatcher.dispatchEvent(BattleEvent.MONSTER_LIVE_REDUCE);
                if (this.curLive <= 0)
                {
                    this.eventDipatcher.dispatchEvent<BattleUnit_Monster>(BattleEvent.MONSTER_DIED, this);
                    return;
                }
            }
        }

        public bool isDead()
        {
            if (this.moveTrans.isReachCarrot == true)
            {
                return true;
            }
            if (this.curLive <= 0)
            {
                return true;
            }
            return false;
        }

        public override void onTick(Fix64 deltaTime)
        {
            this.moveTrans.onTick(deltaTime);
            this.EndPointDistance = this.moveTrans.EndPointDistance;
        }

        public override void lateTick(Fix64 deltaTime)
        {
            this.unitTransform.lateTick(deltaTime);
        }

        public override void ClearInfo()
        {
            base.ClearInfo();
            this.curLevel = 0;
            this.monsterId = 0;
            this.haveBeHit.Clear();
        }
    }
}
