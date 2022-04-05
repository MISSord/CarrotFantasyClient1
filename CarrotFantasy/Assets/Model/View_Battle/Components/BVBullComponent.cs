using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace ETModel
{
    public class BVBulletComponent : BaseBattleViewComponent
    {
        private String prefabUrl;
        private GameObject rootGameObject;

        private Dictionary<BattleUnit_Bullet, BattleUnitView_Bullet> bulletDic = new Dictionary<BattleUnit_Bullet, BattleUnitView_Bullet>();
        public BVBulletComponent(BattleView_base battleView) : base(battleView)
        {
            this.prefabUrl = "Prefabs/Game/Tower/ID{0}/Bullect/{1}";
            this.componentType = BattleViewComponentType.BULLET;
        }

        public override void init()
        {
            BVSceneComponent scene = (BVSceneComponent)this.battleView.getComponent(BattleViewComponentType.SCENE);
            this.rootGameObject = scene.registerGameContainer("BulletContainer");

            BattleDataComponent dataComponent = (BattleDataComponent)this.battle.getComponent(BattleComponentType.DataComponent);
            for (int i = 0; i < dataComponent.towerIDListLength; i++)
            {
                GameViewObjectPool.getInstance().registerGameObject(String.Format("{0}_1", dataComponent.curTowerIDList[i]));
                GameViewObjectPool.getInstance().registerGameObject(String.Format("{0}_2", dataComponent.curTowerIDList[i]));
                GameViewObjectPool.getInstance().registerGameObject(String.Format("{0}_3", dataComponent.curTowerIDList[i]));
            }
            this.addListener();
        }

        private void addListener()
        {
            this.eventDispatcher.addListener<String, BattleUnit>(BattleEvent.BATTLE_UNIT_ADD, this.registerNewBulletView);
            this.eventDispatcher.addListener<String, BattleUnit>(BattleEvent.BATTLE_UNIT_REMOVE, this.removeBulletView);
        }

        private void removeListener()
        {
            this.eventDispatcher.removeListener<String, BattleUnit>(BattleEvent.BATTLE_UNIT_ADD, this.registerNewBulletView);
            this.eventDispatcher.removeListener<String, BattleUnit>(BattleEvent.BATTLE_UNIT_REMOVE, this.removeBulletView);
        }

        private void registerNewBulletView(String type, BattleUnit unit)
        {
            if (type.Equals(BattleUnitType.BULLET))
            {
                BattleUnit_Bullet bullet = (BattleUnit_Bullet)unit;
                BattleUnitView_Bullet bulletView = GameViewObjectPool.getInstance().getNewBattleUnitView<BattleUnitView_Bullet>(BattleUnitViewType.Bullet);
                if (bulletView == null)
                {
                    bulletView = new BattleUnitView_Bullet();
                }
                GameObject bulletNode = GameViewObjectPool.getInstance().getNewGameObject(String.Format("{0}_{1}", bullet.towerId, bullet.towerLevel + 1));
                if (bulletNode == null)
                {
                    bulletNode = GameObject.Instantiate(ResourceLoader.getInstance().getGameObject(String.Format(this.prefabUrl, bullet.towerId, bullet.towerLevel + 1)));
                }
                bulletNode.transform.SetParent(this.rootGameObject.transform);
                bulletView.initTransform(bulletNode.transform);
                bulletView.loadInfo(this.battleView, bullet);
                bulletView.init();

                this.bulletDic.Add(bullet, bulletView);
            }
        }

        public override void onTick(float time)
        {
            foreach (KeyValuePair<BattleUnit_Bullet, BattleUnitView_Bullet> info in this.bulletDic)
            {
                info.Value.onTick(time);
            }
        }

        private void removeBulletView(String type, BattleUnit unit)
        {
            if (type.Equals(BattleUnitType.BULLET) == false) return;
            BattleUnit_Bullet bullet = (BattleUnit_Bullet)unit;
            BattleUnitView_Bullet bulletView;
            if (!this.bulletDic.TryGetValue(bullet, out bulletView))
            {
                Debug.Log("移除子弹视图出错");
                return;
            }
            GameViewObjectPool.getInstance().pushGameObjectToPool(String.Format("{0}_{1}", bullet.towerId, bullet.towerLevel + 1), bulletView.transform.gameObject);
            bulletView.clearUnitInfo();
            this.bulletDic.Remove(bullet);
            GameViewObjectPool.getInstance().pushViewObjectToPool(BattleUnitViewType.Bullet, bulletView);
        }

        public override void clearGameInfo()
        {
            foreach (KeyValuePair<BattleUnit_Bullet, BattleUnitView_Bullet> info in this.bulletDic)
            {
                GameViewObjectPool.getInstance().pushGameObjectToPool(String.Format("{0}_{1}", info.Key.towerId, info.Key.towerLevel + 1), info.Value.transform.gameObject);
                info.Value.clearUnitInfo();
                GameViewObjectPool.getInstance().pushViewObjectToPool(BattleUnitViewType.Bullet, info.Value);
            }
            this.bulletDic.Clear();
            this.removeListener();
        }
    }
}
