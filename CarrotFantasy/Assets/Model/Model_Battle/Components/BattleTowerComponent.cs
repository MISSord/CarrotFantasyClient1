using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ETModel
{
    public class BattleTowerComponent : BaseBattleComponent
    {
        public Dictionary<int, BattleUnit_Tower> curTowerDic = new Dictionary<int, BattleUnit_Tower>();
        //这个int不是tower的uid，是根据坐标换算得到的

        private BattleDataComponent dataComponent;
        private BattleMapComponent mapComponent;

        public TowerConfigReader configReader { get; private set; }
        public int[] canBuildTowerList { get; private set; } //可以建造塔的id
        public int canBuildTowerListLength { get; private set; }

        public BattleTowerComponent(BaseBattle bBattle) : base(bBattle)
        {
            this.componentType = BattleComponentType.TowerComponent;
            this.canBuildTowerList = BattleParamServer.getInstance().curStage.mTowerIDList;
            this.canBuildTowerListLength = BattleParamServer.getInstance().curStage.mTowerIDListLength;
        }

        public override void init()
        {
            this.dataComponent = (BattleDataComponent)this.baseBattle.getComponent(BattleComponentType.DataComponent);
            this.mapComponent = (BattleMapComponent)this.baseBattle.getComponent(BattleComponentType.MapComponent);
            this.configReader = new TowerConfigReader();
            this.configReader.init();
        }

        private int getExChangeInt(int x, int y)
        {
            return x * 100 + y;
        }

        public bool isHaveTower(int x, int y) //地图模块用
        {
            return this.curTowerDic.ContainsKey(this.getExChangeInt(x,y));
        }

        public void exePlayerOrder(InputOrder order)
        {
            if(order.order == InputOrderType.ADD_ORDER)
            {
                int price = (int)this.configReader.getSingleTowerConfig(order.towerId)["price0"];
                if(price > dataComponent.CoinCount)
                {
                    Server.panelServer.showTip(LanguageUtil.getInstance().getString(1004));
                    return;
                }
                BattleUnit_Tower tower = GameObjectPool.getInstance().getNewBattleUnit<BattleUnit_Tower>(BattleUnitType.TOWER);
                if (tower == null)
                {
                    tower = new BattleUnit_Tower(this.baseBattle);
                }
                Fix64Vector2 birthPoint = mapComponent.getMapGridPosition(order.x, order.y);
                tower.loadInfo(this.baseBattle.getUid(), this.configReader.getSingleTowerConfig(order.towerId), birthPoint);
                tower.loadInfo1(order.x, order.y);
                tower.init();
                tower.initComponents();
                this.curTowerDic.Add(this.getExChangeInt(order.x, order.y),tower);
                this.eventDispatcher.dispatchEvent<String, BattleUnit>(BattleEvent.BATTLE_UNIT_ADD, BattleUnitType.TOWER, tower);
                this.eventDispatcher.dispatchEvent<int>(BattleEvent.COIN_CHANGE, -tower.price[tower.curLevel]);
            }
            else if(order.order == InputOrderType.UPDATE_ORDER)
            {
                BattleUnit_Tower tower;
                int id = this.getExChangeInt(order.x, order.y);
                if (this.curTowerDic.TryGetValue(id, out tower))
                {
                    if (tower.isMaxLevel == true) return;
                    if (dataComponent.CoinCount >= tower.price[tower.curLevel + 1])
                    {
                        this.eventDispatcher.dispatchEvent<int>(BattleEvent.COIN_CHANGE, -tower.price[tower.curLevel]);
                        tower.updateLevel();
                    }
                }
                else
                {
                    Debug.Log(String.Format("执行升级操作失败，没有{0}塔", id));
                }
            }
            else if(order.order == InputOrderType.REMOVE_ORDER)
            {
                BattleUnit_Tower tower;
                int id = this.getExChangeInt(order.x, order.y);
                if (this.curTowerDic.TryGetValue(id, out tower))
                {
                    this.eventDispatcher.dispatchEvent<String, BattleUnit>(BattleEvent.BATTLE_UNIT_REMOVE, BattleUnitType.TOWER, tower);
                    tower.ClearInfo();
                    GameObjectPool.getInstance().pushObjectToPool(BattleUnitType.TOWER, tower);
                    this.eventDispatcher.dispatchEvent<int>(BattleEvent.COIN_CHANGE, tower.price[tower.curLevel] - 20);
                    this.curTowerDic.Remove(this.getExChangeInt(order.x, order.y));
                }
                else
                {
                    Debug.Log(String.Format("执行移除操作失败，没有{0}塔", id));
                }
            }
        }

        public override void onTick(Fix64 time)
        {
            foreach(KeyValuePair<int, BattleUnit_Tower> info in this.curTowerDic)
            {
                info.Value.onTick(time);
            }

        }

        public BattleUnit_Tower getTowerInfo(int x, int y)
        {
            int id = this.getExChangeInt(x, y);
            if (this.curTowerDic.ContainsKey(id))
            {
                return this.curTowerDic[id];
            }
            else
            {
                Debug.Log(String.Format("视图层获取防御塔信息失败，没有{0}塔", id));
            }
            return null;
        }

        public override void dispose()
        {
            foreach(KeyValuePair<int, BattleUnit_Tower> info in this.curTowerDic)
            {
                info.Value.ClearInfo();
            }
            this.dataComponent = null;
            this.mapComponent = null;
        }

    }
}
