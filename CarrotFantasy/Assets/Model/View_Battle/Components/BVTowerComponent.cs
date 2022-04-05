using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ETModel
{
    public class BVTowerComponent : BaseBattleViewComponent
    {
        private String prefabUrl;

        private GameObject rootGameObject;

        public Dictionary<BattleUnit_Tower, BattleUnitView_Tower> towerViewDic = new Dictionary<BattleUnit_Tower, BattleUnitView_Tower>();

        private GameObject buildGameObject;
        private GameObject sellGameObject;

        private BattleSchedulerComponent scheComponent;

        public BVTowerComponent(BattleView_base battleView) : base(battleView)
        {
            this.componentType = BattleViewComponentType.TOWER;
        }

        public override void init()
        {
            this.prefabUrl = "Prefabs/Game/Tower/ID{0}/TowerSet/{1}";
            BVSceneComponent scene = (BVSceneComponent)this.battleView.getComponent(BattleViewComponentType.SCENE);
            this.rootGameObject = scene.registerGameContainer("TowerContainer");

            this.buildGameObject = ResourceLoader.getInstance().getGameObject("Prefabs/Game/BuildEffect");
            this.sellGameObject = ResourceLoader.getInstance().getGameObject("Prefabs/Game/DestoryEffect");

            this.scheComponent = (BattleSchedulerComponent)this.battle.getComponent(BattleComponentType.SchedulerComponent);

            this.addListener();
        }

        private void addListener()
        {
            this.eventDispatcher.addListener<String, BattleUnit>(BattleEvent.BATTLE_UNIT_ADD, this.registerTowerView);
            this.eventDispatcher.addListener<String, BattleUnit>(BattleEvent.BATTLE_UNIT_REMOVE, this.removeTowerView);
        }

        private void removeListener()
        {
            this.eventDispatcher.removeListener<String, BattleUnit>(BattleEvent.BATTLE_UNIT_ADD, this.registerTowerView);
            this.eventDispatcher.removeListener<String, BattleUnit>(BattleEvent.BATTLE_UNIT_REMOVE, this.removeTowerView);
        }

        private void registerTowerView(String type, BattleUnit unit)
        {
            if (type.Equals(BattleUnitType.TOWER) == false) return;
            BattleUnit_Tower tower = (BattleUnit_Tower)unit;
            BattleUnitView_Tower towerView = GameViewObjectPool.getInstance().getNewBattleUnitView<BattleUnitView_Tower>(BattleUnitViewType.Tower);
            if(towerView == null)
            {
                towerView = new BattleUnitView_Tower();
            }
            GameObject towerObj = GameObject.Instantiate(
                ResourceLoader.getInstance().getGameObject(String.Format(this.prefabUrl, tower.towerID, tower.curLevel + 1)));
            towerObj.transform.SetParent(this.rootGameObject.transform);
            towerView.loadInfo(this.battleView, tower);
            towerView.initTransform(towerObj.transform);
            tower.eventDipatcher.addListener<BattleUnit_Tower>(BattleEvent.TOWER_LEVEL_UP, this.reloadTran);
            towerView.init();
            this.towerViewDic.Add(tower, towerView);
            UIServer.getInstance().audioManager.playEffect("AudioClips/NormalMordel/Tower/TowerBulid");
            GameObject build = GameObject.Instantiate(this.buildGameObject);
            UnitTransformComponent tran = (UnitTransformComponent)unit.getComponent(UnitComponentType.TRANSFORM);
            build.transform.position = new Vector3((float)tran.lastFrameX, (float)tran.lastFrameY, 0);

            scheComponent.delayExeOnceTimes(() => { GameObject.Destroy(build); }, 0.5f);
        }

        private void removeTowerView(String type, BattleUnit unit)
        {
            if (type.Equals(BattleUnitType.TOWER) == false) return;
            BattleUnit_Tower tower = (BattleUnit_Tower)unit;
            BattleUnitView_Tower towerView;
            if (!this.towerViewDic.TryGetValue(tower, out towerView))
            {
                Debug.Log("移除防御塔视图出错");
                return;
            }

            GameObject.Destroy(towerView.transform.gameObject);
            towerView.clearUnitInfo();
            tower.eventDipatcher.removeListener<BattleUnit_Tower>(BattleEvent.TOWER_LEVEL_UP, this.reloadTran);

            this.towerViewDic.Remove(tower);
            GameViewObjectPool.getInstance().pushViewObjectToPool(BattleUnitViewType.Tower, towerView);
            UIServer.getInstance().audioManager.playEffect("AudioClips/NormalMordel/Tower/TowerSell");

            GameObject sell = GameObject.Instantiate(this.sellGameObject);
            UnitTransformComponent tran = (UnitTransformComponent)tower.getComponent(UnitComponentType.TRANSFORM);
            sell.transform.position = new Vector3((float)tran.lastFrameX, (float)tran.lastFrameY, 0);

            scheComponent.delayExeOnceTimes(() => { GameObject.Destroy(sell); }, 0.5f);
        }

        private void reloadTran(BattleUnit_Tower tower)
        {
            BattleUnitView_Tower towerView = this.towerViewDic[tower];
            GameObject.Destroy(towerView.transform.gameObject);
            GameObject towerObj = GameObject.Instantiate(
                ResourceLoader.getInstance().getGameObject(String.Format(this.prefabUrl, tower.towerID, tower.curLevel + 1)));
            towerView.initTransform(towerObj.transform);
            towerView.reloadInfo();
            UIServer.getInstance().audioManager.playEffect("AudioClips/NormalMordel/Tower/TowerUpdata");

            GameObject build = GameObject.Instantiate(this.buildGameObject);
            UnitTransformComponent tran = (UnitTransformComponent)tower.getComponent(UnitComponentType.TRANSFORM);
            build.transform.position = new Vector3((float)tran.lastFrameX, (float)tran.lastFrameY, 0);

            scheComponent.delayExeOnceTimes(() => { GameObject.Destroy(build); }, 0.5f);
        }

        public override void clearGameInfo()
        {
            foreach (KeyValuePair<BattleUnit_Tower, BattleUnitView_Tower> info in this.towerViewDic)
            {
                GameObject.Destroy(info.Value.transform.gameObject);
                info.Value.clearUnitInfo();
                info.Key.eventDipatcher.removeListener<BattleUnit_Tower>(BattleEvent.TOWER_LEVEL_UP, this.reloadTran);
                GameViewObjectPool.getInstance().pushViewObjectToPool(BattleUnitViewType.Tower, info.Value);
            }
            this.towerViewDic.Clear();
        }

        public override void dispose()
        {
            this.removeListener();
            base.dispose();
        }
    }
}
