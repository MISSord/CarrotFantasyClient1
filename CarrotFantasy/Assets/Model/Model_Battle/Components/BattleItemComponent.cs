using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETModel
{
    public class BattleItemComponent : BaseBattleComponent
    {
        public List<BattleUnit_Item> battleItemList = new List<BattleUnit_Item>();
        private BattleMapComponent mapComponent;
        private ItemConfigReader itemConfigReader;

        public List<BattleUnit_Item> deadItemList = new List<BattleUnit_Item>();

        public BattleItemComponent(BaseBattle bBattle) : base(bBattle)
        {
            this.itemConfigReader = new ItemConfigReader();
            this.itemConfigReader.init();
            this.componentType = BattleComponentType.ItemComponent;
        }

        public override void init()
        {
            this.mapComponent = (BattleMapComponent)(this.baseBattle.getComponent(BattleComponentType.MapComponent));
            BattleDataComponent dataOne = (BattleDataComponent)this.baseBattle.getComponent(BattleComponentType.DataComponent);
            BattleMapGrid[,] gridsList = mapComponent.gridsList;

            for (int x = 0; x <= dataOne.xColumn - 1; x++)
            {
                for(int y = 0; y <= dataOne.yRow - 1; y++)
                {
                    if (gridsList[x, y].state.hasItem)
                    {
                        this.createItem(gridsList[x, y]);
                    }
                }
            }
        }

        //创建物品
        private void createItem(BattleMapGrid mapGrid)
        {
            BattleUnit_Item item = new BattleUnit_Item(this.baseBattle);
            item.eventDipatcher.addListener<BattleUnit_Item>(BattleEvent.ITEM_DIED, this.addDeadList);
            int itemId = this.mapComponent.levelInfo.bigLevelID * 100 + mapGrid.state.itemID;
            item.loadInfo(this.baseBattle.getUid(), this.itemConfigReader.getSingleItemConfig(itemId), this.getPosition(mapGrid), mapGrid.state.itemID);
            item.init();
            item.initComponents();
            item.loadInfo1();
            this.battleItemList.Add(item);
            this.eventDispatcher.dispatchEvent<String, BattleUnit>(BattleEvent.BATTLE_UNIT_ADD,BattleUnitType.ITEM, item);
        }

        private Fix64Vector2 getPosition(BattleMapGrid mapGrid)
        {
            Fix64Vector2 tran = new Fix64Vector2(mapGrid.realX, mapGrid.realY);
            if (mapGrid.state.itemID <= 2)
            {
                tran += new Fix64Vector2(BattleConfig.MAP_RATIO, -BattleConfig.MAP_RATIO) / Fix64.Two;
            }
            else if (mapGrid.state.itemID <= 4)
            {
                tran += new Fix64Vector2(BattleConfig.MAP_RATIO, 0) / Fix64.Two;
            }
            return tran;
        }

        private void addDeadList(BattleUnit_Item item)
        {
            this.deadItemList.Add(item);
        }

        public override void onTick(Fix64 time)
        {
            base.onTick(time);
            this.updateItemState();
        }

        public void updateItemState()
        {
            if (this.deadItemList.Count != 0)
            {
                for (int i = 0; i < this.deadItemList.Count; i++)
                {
                    this.checkSingleItemState(this.deadItemList[i]);
                }
                this.deadItemList.Clear();
            }
        }

        public void checkSingleItemState(BattleUnit_Item item)
        {
            if (item.isDead() == true)
            {
                this.eventDispatcher.dispatchEvent<String, BattleUnit>(BattleEvent.BATTLE_UNIT_REMOVE, BattleUnitType.ITEM, item);
                this.baseBattle.eventDispatcher.dispatchEvent<int>(BattleEvent.COIN_CHANGE, (int)item.birthParam["money"]);
                //先从其他组件上除去，再从视图移除，最后再自己移除，确保顺序
                item.ClearInfo();
                this.battleItemList.Remove(item);
            }
        }

        public override void clearInfo()
        {
            base.clearInfo();
            this.deadItemList.Clear();
            for (int i = 0; i <= this.battleItemList.Count - 1; i++)
            {
                this.battleItemList[i].eventDipatcher.removeListener<BattleUnit_Item>(BattleEvent.ITEM_DIED, this.addDeadList);
                this.battleItemList[i].dispose();
            }
            this.battleItemList.Clear();
        }

        public override void dispose()
        {
            this.clearInfo();
            base.dispose();
        }
    }
}
