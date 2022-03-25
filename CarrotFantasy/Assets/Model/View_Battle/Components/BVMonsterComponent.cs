using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace ETModel
{
    public class BVMonsterComponent : BaseBattleViewComponent
    {
        private String pathUrl;
        private GameObject noInstanGameObject;
        private GameObject rootGameObject;

        private Dictionary<BattleUnit_Monster, BattleUnitView_Monster> monsterDic = new Dictionary<BattleUnit_Monster, BattleUnitView_Monster>();

        public BVMonsterComponent(BattleView_base battleView) : base(battleView)
        {
            this.componentType = BattleViewComponentType.MONSTER;
            this.pathUrl = "Prefabs/Game/MonsterPrefab";

            this.rootGameObject = new GameObject();
            this.rootGameObject.transform.SetParent(this.battleView.rootGameObject.transform);
            this.rootGameObject.transform.localPosition = Vector3.zero;
            this.rootGameObject.transform.localScale = Vector3.one;
        }

        public override void init()
        {
            BVSceneComponent scene = (BVSceneComponent)this.battleView.getComponent(BattleViewComponentType.SCENE);
            this.rootGameObject = scene.registerGameContainer("MonsterContainer");
            this.noInstanGameObject = ResourceLoader.getInstance().getGameObject(this.pathUrl);

            GameViewObjectPool.getInstance().registerGameObject(BattleUnitViewType.Monster);
            this.addListener();
        }

        private void addListener()
        {
            this.eventDispatcher.addListener<String, BattleUnit>(BattleEvent.BATTLE_UNIT_ADD, this.registerNewMonsterView);
            this.eventDispatcher.addListener<String, BattleUnit>(BattleEvent.BATTLE_UNIT_REMOVE, this.removeMonsterView);
        }

        private void removeListener()
        {
            this.eventDispatcher.removeListener<String, BattleUnit>(BattleEvent.BATTLE_UNIT_ADD, this.registerNewMonsterView);
            this.eventDispatcher.removeListener<String, BattleUnit>(BattleEvent.BATTLE_UNIT_REMOVE, this.removeMonsterView);
        }

        private void registerNewMonsterView(String type, BattleUnit unit)
        {
            if (type.Equals(BattleUnitType.MONSTER))
            {
                BattleUnit_Monster monster = (BattleUnit_Monster)unit;
                BattleUnitView_Monster monsterView = GameViewObjectPool.getInstance().getNewBattleUnitView<BattleUnitView_Monster>(BattleUnitViewType.Monster);
                GameObject node = GameViewObjectPool.getInstance().getNewGameObject(BattleUnitViewType.Monster);
                if (monsterView == null)
                {
                    monsterView = new BattleUnitView_Monster();
                }
                if (node == null)
                {
                    node = GameObject.Instantiate(this.noInstanGameObject);
                    node.transform.SetParent(this.rootGameObject.transform);
                }
                monsterView.initTransform(node.transform);
                monsterView.loadInfo(this.battleView, monster);
                monsterView.init();

                this.monsterDic.Add(monster, monsterView);
                UIServer.getInstance().audioManager.playEffect("AudioClips/NormalMordel/Monster/Create");
            }
        }

        public override void onTick(float time)
        {
            foreach(KeyValuePair<BattleUnit_Monster, BattleUnitView_Monster> info in this.monsterDic)
            {
                info.Value.onTick(time);
            }
        }

        private void removeMonsterView(String type, BattleUnit unit)
        {
            if (type.Equals(BattleUnitType.MONSTER) == false) return;
            BattleUnit_Monster monster = (BattleUnit_Monster)unit;
            BattleUnitView_Monster monsterView;
            if(!this.monsterDic.TryGetValue(monster, out monsterView))
            {
                Debug.Log("移除怪兽视图出错");
                return;
            }
            monsterView.clearUnitInfo();
            this.monsterDic.Remove(monster);
            GameViewObjectPool.getInstance().pushViewObjectToPool(BattleUnitViewType.Monster, monsterView);
            UIServer.getInstance().audioManager.playEffect(String.Format("AudioClips/NormalMordel/Monster/{0}/{1}",monster.curLevel,monster.monsterId));
        }

        public override void clearGameInfo()
        {
            foreach (KeyValuePair<BattleUnit_Monster, BattleUnitView_Monster> info in this.monsterDic)
            {
                GameViewObjectPool.getInstance().pushViewObjectToPool(BattleUnitViewType.Monster, info.Value);
            }
            this.monsterDic.Clear();
        }

        public override void dispose()
        {
            this.removeListener();
            base.dispose();
        }
    }
}
