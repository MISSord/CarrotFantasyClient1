using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ETModel
{
    public class BVItemComponent : BaseBattleViewComponent
    {
        public Dictionary<BattleUnit_Item, BattleUnitView_Item> itemDic = new Dictionary<BattleUnit_Item, BattleUnitView_Item>();
        public BattleItemComponent itemComponent;
        private String itemPrefabUrl;
        private GameObject rootGameObject;

        public BVItemComponent(BattleView_base battleView) : base(battleView)
        {
            this.itemComponent = (BattleItemComponent)this.battleView.battle.getComponent(BattleComponentType.ItemComponent);
            BattleDataComponent dataComponent = (BattleDataComponent)this.battleView.battle.getComponent(BattleComponentType.DataComponent);
            this.itemPrefabUrl = "Prefabs/Game/Item/" + dataComponent.bigLevel + "/";
            this.componentType = BattleViewComponentType.Item;
        }

        public override void init()
        {
            GameViewObjectPool.getInstance().registerGameObject(BattleUnitViewType.DestroyEffect);

            BVSceneComponent scene = (BVSceneComponent)this.battleView.getComponent(BattleViewComponentType.SCENE);
            this.rootGameObject = scene.registerGameContainer("ItemContainer");
            List<BattleUnit_Item> itemList = this.itemComponent.battleItemList;
            for (int i = 0; i <= itemList.Count - 1; i++)
            {
                this.createItemView(itemList[i]);
            }
            this.addListener();
        }

        private void addListener()
        {
            this.eventDispatcher.addListener<String, BattleUnit>(BattleEvent.BATTLE_UNIT_REMOVE, this.removeItemView);
        }

        private void removeListener()
        {
            this.eventDispatcher.removeListener<String, BattleUnit>(BattleEvent.BATTLE_UNIT_REMOVE, this.removeItemView);
        }

        private void createItemView(BattleUnit_Item item)
        {
            BattleUnitView_Item itemView = new BattleUnitView_Item();
            GameObject itemGo = GameObject.Instantiate(ResourceLoader.getInstance().getGameObject(this.itemPrefabUrl + item.itemId));
            itemGo.transform.SetParent(this.rootGameObject.transform);
            itemView.initTransform(itemGo.transform);
            itemView.loadInfo(this.battleView, item);
            itemView.init();
            this.itemDic.Add(item, itemView);
        }

        private void removeItemView(String type, BattleUnit obj)
        {
            if (type.Equals(BattleUnitType.ITEM))
            {
                BattleUnit_Item item = (BattleUnit_Item)obj;
                BattleUnitView_Item itemView = this.itemDic[item];
                GameObject.Destroy(itemView.transform.gameObject);
                itemView.clearUnitInfo();
                this.itemDic.Remove(item);
                GameViewObjectPool.getInstance().pushViewObjectToPool(BattleUnitViewType.Item, itemView);

                //特效
                GameObject sell = GameViewObjectPool.getInstance().getNewGameObject(BattleUnitViewType.DestroyEffect);
                if (sell == null)
                {
                    sell = GameObject.Instantiate(ResourceLoader.getInstance().getGameObject("Prefabs/Game/DestoryEffect"));
                }
                sell.transform.GetComponent<Animator>().enabled = true;
                UnitTransformComponent tran = (UnitTransformComponent)obj.getComponent(UnitComponentType.TRANSFORM);
                sell.transform.position = new Vector3((float)tran.lastFrameX, (float)tran.lastFrameY, 0);
                Sche.delayExeOnceTimes(() => {
                    sell.transform.GetComponent<Animator>().enabled = false;
                    GameViewObjectPool.getInstance().pushGameObjectToPool(BattleUnitViewType.DestroyEffect, sell);
                }, 0.5f);
            }
        }

        public override void clearGameInfo()
        {
            foreach(KeyValuePair<BattleUnit_Item, BattleUnitView_Item> info in this.itemDic)
            {
                GameObject.Destroy(info.Value.transform.gameObject);
                info.Value.clearUnitInfo();
                GameViewObjectPool.getInstance().pushViewObjectToPool(BattleUnitViewType.Item, info.Value);
            }
            this.itemDic.Clear();
            this.removeListener();
        }

        public override void dispose()
        {
            this.clearGameInfo();
            base.dispose();
        }

    }
}
