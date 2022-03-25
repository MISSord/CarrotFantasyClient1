using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ETModel
{
    public class BattleUnit_Bullet : BattleUnit
    {
        public int damage { get; private set; }
        public Fix64 moveSpeed;

        public int towerId = 0;
        public int towerLevel = 0;

        public UnitMoveComponent_Bullet moveComponent;
        public UnitTransformComponent tranComponent;
        public UnitBeHitComponent beHitComponent;

        private Fix64Vector2 target;

        public BattleUnit_Bullet(BaseBattle battle) : base(battle)
        {
            this.unitType = BattleUnitType.BULLET;
        }

        public override void loadInfo(int uid, Dictionary<string, Fix64> param, Fix64Vector2 birthPosition)
        {
            base.loadInfo(uid, param, birthPosition);
            this.damage = (int)this.birthParam["damage"];
            this.moveSpeed = this.birthParam["moveSpeed"];
        }

        public void loadInfo2(BattleUnit_Tower tower, Fix64Vector2 target)
        {
            this.towerId = tower.towerID;
            this.towerLevel = tower.curLevel;
            this.target = target;
        }

        public override void init()
        {
            this.moveComponent = GameObjectPool.getInstance().getNewUnitComponent<UnitMoveComponent_Bullet>(UnitComponentType.MOVE_BULLET);
            if (this.moveComponent == null)
            {
                this.moveComponent = new UnitMoveComponent_Bullet();
            }
            this.tranComponent = GameObjectPool.getInstance().getNewUnitComponent<UnitTransformComponent>(UnitComponentType.TRANSFORM);
            if (this.tranComponent == null)
            {
                this.tranComponent = new UnitTransformComponent();
            }
            this.beHitComponent = GameObjectPool.getInstance().getNewUnitComponent<UnitBeHitComponent>(UnitComponentType.BEHIT);
            if (this.beHitComponent == null)
            {
                this.beHitComponent = new UnitBeHitComponent();
            }

            this.addComponent(this.moveComponent);
            this.addComponent(this.tranComponent);
            this.addComponent(this.beHitComponent);

            this.beHitComponent.registerBeHitCallBack(this.beHitCallBack);
        }

        public override void initComponents()
        {
            base.initComponents();
            this.moveComponent.registerMoveDirect(this.target);
            this.tranComponent.setBodyRadius(new Fix64(0.2f));
        }

        private void beHitCallBack(BattleUnit unit)
        {
            if (unit.unitType.Equals(BattleUnitType.MONSTER) == false) return;
            Debug.Log("碰到怪兽了");
            if(this.birthParam["isRemove"] == Fix64.Zero)
            {
                this.eventDipatcher.dispatchEvent<BattleUnit_Bullet>(BattleEvent.BULLET_REMOVE, this);
            }
        }

        public override void onTick(Fix64 deltaTime)
        {
            this.moveComponent.onTick(deltaTime);
        }

        public override void lateTick(Fix64 deltaTime)
        {
            this.tranComponent.lateTick(deltaTime);
        }

        public override void dispose()
        {
            base.dispose();
        }
    }
}
