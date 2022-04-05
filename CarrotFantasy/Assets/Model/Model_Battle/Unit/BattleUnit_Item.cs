using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETModel
{
    public class BattleUnit_Item : BattleUnit
    {
        public int curLive; //物体血量
        public int totalLive; //物体总血量
        public UnitTransformComponent tranComponent { get; private set; }
        public UnitBeHitComponent beHitComponent { get; private set; }

        private List<int> haveBeHit;

        public BattleUnit_Item(BaseBattle battle) : base(battle)
        {

        }

        public override void init()
        {
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
            this.addComponent(this.tranComponent);
            this.addComponent(this.beHitComponent);

            this.beHitComponent.registerBeHitCallBack(this.beHitCallBack);
        }

        public override void loadInfo(int uid, Dictionary<string, Fix64> param, Fix64Vector2 birthPosition)
        {
            base.loadInfo(uid, param, birthPosition);
            this.curLive = (int)this.birthParam["live"];
            this.totalLive = (int)this.birthParam["live"];
        }

        public override void onTick(Fix64 deltaTime)
        {

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
                    this.eventDipatcher.dispatchEvent<BattleUnit_Item>(BattleEvent.ITEM_LIVE_REDUCE, this);
                    return;
                }
            }
        }

        public bool isDead()
        {
            if (this.curLive <= 0) return true;
            return false;
        }

        public override void dispose()
        {
            this.haveBeHit.Clear();
            base.dispose();
        }
    }
}
