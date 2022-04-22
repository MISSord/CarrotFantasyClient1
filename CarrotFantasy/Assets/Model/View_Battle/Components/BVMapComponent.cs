using System;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
using System.IO;

namespace ETModel
{
    public class BVMapComponent : BaseBattleViewComponent
    {
        public Sprite sprGirdNoramlState;
        public Sprite sprGirdStartState;
        public Sprite sprGirdCantBuildState;

        public GridPoint[,] gridPointList;
        private GameObject rootGameObject;

        private GameObject node_bg;

        //地图的有关属性
        //地图
        public int xColumn;
        public int yRow;

        public BVMapComponent(BattleView_base battleView) : base(battleView)
        {
            BattleDataComponent dataOne = (BattleDataComponent)this.battle.getComponent(BattleComponentType.DataComponent);
            this.xColumn = dataOne.xColumn;
            this.yRow = dataOne.yRow;
            this.gridPointList = new GridPoint[this.xColumn, this.yRow];
            this.componentType = BattleViewComponentType.MAP;
        }

        public override void init()
        {
            this.sprGirdNoramlState = ResourceLoader.getInstance().loadRes<Sprite>("Pictures/NormalMordel/Game/Grid");
            this.sprGirdStartState = ResourceLoader.getInstance().loadRes<Sprite>("Pictures/NormalMordel/Game/StartSprite");
            this.sprGirdCantBuildState = ResourceLoader.getInstance().loadRes<Sprite>("Pictures/NormalMordel/Game/cantBuild");
            this.loadMapGrid();
        }

        private void loadMapGrid()
        {
            GameObject item = ResourceLoader.getInstance().getGameObject("Prefabs/Game/Grid");

            BattleMapComponent mapComponent = (BattleMapComponent)this.battle.getComponent(BattleComponentType.MapComponent);
            BattleMapGrid[,] mapGridInfo = mapComponent.gridsList;

            BVSceneComponent scene = (BVSceneComponent)this.battleView.getComponent(BattleViewComponentType.SCENE);
            GameObject gridList = scene.registerGameContainer("GridContainer");

            for (int x = 0; x < this.xColumn; x++)
            {
                for (int y = 0; y < this.yRow; y++)
                {
                    GameObject itemGo = GameObject.Instantiate(item);
                    itemGo.transform.position = new Vector3((float)mapGridInfo[x, y].realX, (float)mapGridInfo[x, y].realY, 0);
                    itemGo.transform.SetParent(gridList.transform);
                    this.gridPointList[x, y] = itemGo.transform.GetComponent<GridPoint>();
                    this.gridPointList[x, y].initTrans(this.battleView);
                    this.gridPointList[x, y].initInfo(x,y);
                }
            }
        }


        public override void start()
        {
            for (int x = 0; x < this.xColumn; x++)
            {
                for (int y = 0; y < this.yRow; y++)
                {
                    this.gridPointList[x, y].startGame();
                }
            }
        }

        public override void clearGameInfo()
        {
            base.clearGameInfo();
        }

    }
}
