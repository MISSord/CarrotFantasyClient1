using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ETModel
{
    public class BattleBulletComponent : BaseBattleComponent
    {
        private List<BattleUnit_Bullet> curBulletList = new List<BattleUnit_Bullet>();
        private List<BattleUnit_Bullet> bulletDeadList = new List<BattleUnit_Bullet>();

        private BulletConfigReader configReader;

        public BattleBulletComponent(BaseBattle bBattle) : base(bBattle)
        {
            this.componentType = BattleComponentType.BulletComponent;
        }

        public override void init()
        {
            this.configReader = new BulletConfigReader();
            this.configReader.init();
            this.addListener();
        }

        private void addListener()
        {
            this.eventDispatcher.addListener<BattleUnit_Tower, Fix64Vector2>(BattleEvent.BULLET_BUILD, this.buildNewBullet);
        }

        private void removeListener()
        {
            this.eventDispatcher.removeListener<BattleUnit_Tower,Fix64Vector2>(BattleEvent.BULLET_BUILD, this.buildNewBullet);
        }

        public void buildNewBullet(BattleUnit_Tower tower, Fix64Vector2 target)
        {
            BattleUnit_Bullet bullet = GameObjectPool.getInstance().getNewBattleUnit<BattleUnit_Bullet>(BattleUnitType.BULLET);
            if (bullet == null)
            {
                bullet = new BattleUnit_Bullet(this.baseBattle);
            }
            bullet.eventDipatcher.addListener<BattleUnit_Bullet>(BattleEvent.BULLET_REMOVE, this.addDeadList);
            bullet.loadInfo(this.baseBattle.getUid(), this.configReader.getSingleBulletConfig(tower.towerID * 100 + tower.curLevel + 1), tower.birthPosition);
            bullet.loadInfo2(tower, target);
            bullet.init();
            bullet.initComponents();
            this.curBulletList.Add(bullet);
            this.eventDispatcher.dispatchEvent<String, BattleUnit>(BattleEvent.BATTLE_UNIT_ADD, BattleUnitType.BULLET, bullet);
            //Debug.Log("注册新的子弹");
        }

        private void addDeadList(BattleUnit_Bullet monster)
        {
            this.bulletDeadList.Add(monster);
        }

        public override void onTick(Fix64 time)
        {
            base.onTick(time);
            this.updateCurBulletState(time);
        }

        public override void lateTick(Fix64 time)
        {
            base.lateTick(time);
            this.updateCurMonsterWaveStateLateTick(time);
        }

        public void updateCurBulletState(Fix64 time)
        {
            if (this.curBulletList.Count != 0)
            {
                for (int i = 0; i < this.curBulletList.Count; i++)
                {
                    this.curBulletList[i].onTick(time);
                }
            }
            if (this.bulletDeadList.Count != 0)
            {
                for (int i = 0; i < this.bulletDeadList.Count; i++)
                {
                    this.eventDispatcher.dispatchEvent<String, BattleUnit>(BattleEvent.BATTLE_UNIT_REMOVE, BattleUnitType.BULLET, this.bulletDeadList[i]);
                    this.bulletDeadList[i].ClearInfo();
                    GameObjectPool.getInstance().pushObjectToPool(BattleUnitType.BULLET, this.bulletDeadList[i]);
                    this.curBulletList.Remove(this.bulletDeadList[i]);
                }
                this.bulletDeadList.Clear();
            }
        }

        public void updateCurMonsterWaveStateLateTick(Fix64 time)
        {
            if (this.curBulletList.Count != 0)
            {
                for (int i = 0; i < this.curBulletList.Count; i++)
                {
                    this.curBulletList[i].lateTick(time);
                }
            }
        }
        public override void dispose()
        {
            this.removeListener();
            if (this.curBulletList.Count != 0)
            {
                for (int i = 0; i < this.curBulletList.Count; i++)
                {
                    this.curBulletList[i].ClearInfo();
                    GameObjectPool.getInstance().pushObjectToPool(BattleUnitType.BULLET, this.curBulletList[i]);
                }
            }
            this.curBulletList.Clear();
            this.bulletDeadList.Clear();
            base.dispose();
        }
    }
}
