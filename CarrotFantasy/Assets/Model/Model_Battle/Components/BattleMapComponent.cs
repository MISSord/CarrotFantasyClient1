using System;
using System.Collections.Generic;

namespace ETModel
{
    public class BattleMapComponent : BaseBattleComponent
    {
        private float heightMultiplyByRatio;
        private float widthMultiplyByRatio;

        private float MAP_RATIO = BattleConfig.MAP_RATIO;

        public int xColumn { get; private set; } //地图格子宽度数量
        public int yRow { get; private set; } //地图格子长度数量

        public BattleMapGrid[,] gridsList { get; private set; }

        public Fix64Vector2 startPoint { get; private set; }
        public Fix64Vector2 endPoint { get; private set; }

        public LevelInfo levelInfo { get; private set; }

        public List<Fix64Vector2> monsterPathList;

        public Fix64Vector2 mapLeftBottomPosition { get; private set; }
        public Fix64Vector2 mapRightTopPosition { get; private set; }

        //private Dictionary<int, BattleMapGrid> uid2Grid = new Dictionary<int, BattleMapGrid>();
        //public Dictionary<int, BattleMapGrid> moveLine = new Dictionary<int, BattleMapGrid>();

        public BattleMapComponent(BaseBattle bBattle) : base(bBattle)
        {
            this.componentType = BattleComponentType.MapComponent;
        }

        public override void init()
        {
            BattleDataComponent dataOne = (BattleDataComponent)this.baseBattle.getComponent(BattleComponentType.DataComponent);
            this.xColumn = dataOne.xColumn;
            this.yRow = dataOne.yRow;
            this.gridsList = new BattleMapGrid[this.xColumn, this.yRow];

            this.widthMultiplyByRatio = this.xColumn * MAP_RATIO;
            this.heightMultiplyByRatio = this.yRow * MAP_RATIO;

            this.levelInfo = BattleParamServer.getInstance().info;

            this.loadMapGrid();
            this.loadMapInfo();
        }

        private void loadMapGrid()
        {
            for (int x = 0; x < this.xColumn; x++)
            {
                for (int y = 0; y < this.yRow; y++)
                {
                    this.gridsList[x, y] = new BattleMapGrid(this.baseBattle.getUid(), x, y);
                }
            }
        }

        private void loadMapInfo()
        {
            this.monsterPathList = new List<Fix64Vector2>();
            for(int i = 0; i < this.levelInfo.monsterPath.Count; i++)
            {
                BattleMapGrid mapGrid = this.gridsList[this.levelInfo.monsterPath[i].xIndex, this.levelInfo.monsterPath[i].yIndex];
                monsterPathList.Add(new Fix64Vector2(mapGrid.realX, mapGrid.realY));
            }
            for (int x = 0; x < this.xColumn; x++)
            {
                for (int y = 0; y < this.yRow; y++)
                {
                    this.gridsList[x, y].loadGridInfo(this.levelInfo.gridPoints[y + x * yRow]);
                }
            }
            this.startPoint = monsterPathList[0];

            Fix64 ratio = new Fix64(BattleConfig.MAP_RATIO / (float)2);
            this.mapLeftBottomPosition = new Fix64Vector2(this.gridsList[0, 0].realX - ratio, this.gridsList[0, 0].realY - ratio);
            this.mapRightTopPosition = new Fix64Vector2(this.gridsList[xColumn - 1, yRow - 1].realX + ratio, this.gridsList[xColumn - 1, yRow - 1].realY + ratio);
        }

        public bool isCanBuildTower(int x, int y) //视图层调用
        {
            return this.levelInfo.gridPoints[this.getListNumber(x, y)].canBuild;
        }

        private int getListNumber(int x, int y)
        {
            return (x - 1) * this.xColumn + (y - 1) * this.yRow;
        }

        public Fix64Vector2 getMapGridPosition(int x, int y)
        {
            return new Fix64Vector2(this.gridsList[x, y].realX, this.gridsList[x, y].realY);
        }

        public void exePlayerOrder(InputOrder order)
        {
            BattleTowerComponent towerComponet = (BattleTowerComponent)this.baseBattle.getComponent(BattleComponentType.TowerComponent);
            BattleUnit_Tower tower = towerComponet.getTowerInfo(order.x, order.y);
            if (tower != null)
            {
                this.gridsList[order.x, order.y].changeTowerState(true);
            }
            else
            {
                this.gridsList[order.x, order.y].changeTowerState(false);
            }
        }

        public override void clearInfo()
        {
            this.monsterPathList.Clear();
            this.gridsList = null;
        }

    }
}
