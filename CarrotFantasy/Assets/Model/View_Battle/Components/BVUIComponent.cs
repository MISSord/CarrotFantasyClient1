using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace ETModel
{
    public class BVUIComponent : BaseBattleViewComponent
    {
        public GridPoint selectGrid { get; private set; }//上一个选择的格子

        private GameObject nodeHandleTowerCanvas;
        private int towerLength;

        private BattleTowerComponent towerComponent;
        private BattleDataComponent dataComponent;
        private BattleMapComponent mapComponent;

        private GameObject nodeTowerList;

        private ButtonTower[] buttonTowerList;

        private Vector3 upLevelButtonInitPos;//两个按钮的初始位置
        private Vector3 sellTowerButtonInitPos;

        private Sprite[] spriteButtonUpList;

        private Transform tranButtonUp;//两个按钮的trans引用
        private Transform tranButtonSell;

        private Image imgButtonUp;
        private Text txtButtonUp;

        private GameObject nodeMap;

        private Text txtButtonSell;

        private MapUIConfigReader reader;

        private GameObject nodeCarrot;
        private Carrot carrot;

        public BVUIComponent(BattleView_base battleView) : base(battleView)
        {
            this.towerComponent = (BattleTowerComponent)this.battle.getComponent(BattleComponentType.TowerComponent);
            this.dataComponent = (BattleDataComponent)this.battle.getComponent(BattleComponentType.DataComponent);
            this.mapComponent = (BattleMapComponent)this.battle.getComponent(BattleComponentType.MapComponent);
            this.buttonTowerList = new ButtonTower[this.towerComponent.canBuildTowerListLength];
            this.spriteButtonUpList = new Sprite[3];
            this.componentType = BattleViewComponentType.UI;
        }

        private void addListener()
        {
            this.battleView.bvEventDispatcher.addListener<GridPoint>(BattleViewEventType.Select_Grid, this.HandleGrid);

            this.battleView.bvEventDispatcher.addListener<GridPoint>(BattleViewEventType.Show_Handle_Tower, this.showHandleTowerCanvas);
            this.battleView.bvEventDispatcher.addListener(BattleViewEventType.Fade_Handle_Tower, this.fadeHandleTowerCanvas);
            this.battleView.bvEventDispatcher.addListener<GridPoint>(BattleViewEventType.Show_Tower_List, this.showTowerList);
            this.battleView.bvEventDispatcher.addListener(BattleViewEventType.Fade_Tower_List, this.fadeTowerList);

            this.eventDispatcher.addListener<int>(BattleEvent.COIN_CHANGE, this.refreshButtonInfo);

            this.tranButtonSell.GetComponent<Button>().onClick.AddListener(this.sellTower);
            this.tranButtonUp.GetComponent<Button>().onClick.AddListener(this.updateTower);

            this.eventDispatcher.addListener<String, BattleUnit>(BattleEvent.BATTLE_UNIT_ADD, this.updateNodeState);
        }

        private void removeListener()
        {
            this.battleView.bvEventDispatcher.removeListener<GridPoint>(BattleViewEventType.Select_Grid, this.HandleGrid);

            this.battleView.bvEventDispatcher.removeListener<GridPoint>(BattleViewEventType.Show_Handle_Tower, this.showHandleTowerCanvas);
            this.battleView.bvEventDispatcher.removeListener(BattleViewEventType.Fade_Handle_Tower, this.fadeHandleTowerCanvas);
            this.battleView.bvEventDispatcher.removeListener<GridPoint>(BattleViewEventType.Show_Tower_List, this.showTowerList);
            this.battleView.bvEventDispatcher.removeListener(BattleViewEventType.Fade_Tower_List, this.fadeTowerList);

            this.eventDispatcher.removeListener<int>(BattleEvent.COIN_CHANGE, this.refreshButtonInfo);

            this.tranButtonSell.GetComponent<Button>().onClick.RemoveAllListeners();
            this.tranButtonUp.GetComponent<Button>().onClick.RemoveAllListeners();

            this.eventDispatcher.removeListener<String, BattleUnit>(BattleEvent.BATTLE_UNIT_ADD, this.updateNodeState);
        }

        public override void init()
        {
            this.reader = new MapUIConfigReader();
            this.reader.init();

            this.nodeTowerList = GameObject.Instantiate(ResourceLoader.getInstance().getGameObject("Prefabs/Game/UI/tower_list"));
            this.nodeTowerList.transform.position = this.battleView.initTran;
            this.nodeTowerList.transform.GetComponent<Canvas>().sortingOrder = 20;
            this.nodeCarrot = GameObject.Instantiate(ResourceLoader.getInstance().getGameObject("Prefabs/Game/Carrot"));
            this.carrot = this.nodeCarrot.transform.GetComponent<Carrot>();


            Fix64Vector2 curPosition = this.mapComponent.monsterPathList[this.mapComponent.monsterPathList.Count - 1];
            this.carrot.transform.position = new Vector3((float)curPosition.X, (float)curPosition.Y, 0);

            for (int i = 0; i <= this.towerComponent.canBuildTowerListLength - 1; i++)
            {
                GameObject itemGo = GameObject.Instantiate(ResourceLoader.getInstance().getGameObject("Prefabs/Game/UI/btn_tower_build"));
                this.buttonTowerList[i] = new ButtonTower();
                this.buttonTowerList[i].loadInfo(this);
                this.buttonTowerList[i].initInfo(itemGo.transform, this.towerComponent.canBuildTowerList[i]);

                itemGo.transform.SetParent(this.nodeTowerList.transform);
                itemGo.transform.localPosition = Vector3.zero;
                itemGo.transform.localScale = Vector3.one;
            }

            nodeHandleTowerCanvas = GameObject.Instantiate(ResourceLoader.getInstance().getGameObject("Prefabs/Game/UI/handle_tower_canvas"));
            this.nodeHandleTowerCanvas.transform.position = this.battleView.initTran;
            this.nodeHandleTowerCanvas.transform.GetComponent<Canvas>().sortingOrder = 20;

            this.spriteButtonUpList[0] = ResourceLoader.getInstance().loadRes<Sprite>("Pictures/NormalMordel/Game/Tower/Btn_CantUpLevel"); //不能升级
            this.spriteButtonUpList[1] = ResourceLoader.getInstance().loadRes<Sprite>("Pictures/NormalMordel/Game/Tower/Btn_CanUpLevel"); //能升级
            this.spriteButtonUpList[2] = ResourceLoader.getInstance().loadRes<Sprite>("Pictures/NormalMordel/Game/Tower/Btn_ReachHighestLevel"); //满级

            this.nodeMap = GameObject.Instantiate(ResourceLoader.getInstance().getGameObject("Prefabs/Game/nodeMap"));
            this.nodeMap.transform.position = new Vector3(6, 4.35f, 0);
            Dictionary<String, int> map = this.reader.getMapUIConfig(dataComponent.bigLevel, dataComponent.level);
            this.nodeMap.transform.Find("img_bg").GetComponent<SpriteRenderer>().sprite = ResourceLoader.getInstance().
                loadRes<Sprite>(String.Format("Pictures/NormalMordel/Game/{0}/BG{1}", dataComponent.bigLevel, map["mapBg"]));
            this.nodeMap.transform.Find("img_road").GetComponent<SpriteRenderer>().sprite = ResourceLoader.getInstance().
                loadRes<Sprite>(String.Format("Pictures/NormalMordel/Game/{0}/Road{1}", dataComponent.bigLevel, map["mapRoad"]));
            this.nodeMap.transform.Find("img_bg").GetComponent<SpriteRenderer>().sortingOrder = 2;
            this.nodeMap.transform.Find("img_road").GetComponent<SpriteRenderer>().sortingOrder = 4;

            this.loadInfo();

            this.addListener();
        }

        private void loadInfo()
        {
            tranButtonUp = this.nodeHandleTowerCanvas.transform.Find("btn_up_level");
            tranButtonSell = this.nodeHandleTowerCanvas.transform.Find("btn_sell");

            this.imgButtonUp = this.nodeHandleTowerCanvas.transform.Find("btn_up_level").GetComponent<Image>();
            this.txtButtonUp = this.nodeHandleTowerCanvas.transform.Find("btn_up_level/txt_price").GetComponent<Text>();

            this.txtButtonSell = this.nodeHandleTowerCanvas.transform.Find("btn_sell/txt_price").GetComponent<Text>();

            this.upLevelButtonInitPos = tranButtonUp.localPosition;
            this.sellTowerButtonInitPos = tranButtonSell.localPosition;
        }

        private void showHandleTowerCanvas(GridPoint grid)
        {
            this.selectGrid = grid;
            this.nodeHandleTowerCanvas.transform.position = new Vector3((float)grid.mapGrid.realX, (float)grid.mapGrid.realY, 0);
            this.battleView.bvEventDispatcher.dispatchEvent<GridPoint>(BattleEvent.TOWER_RANGE_SHOW, grid);
            this.CorrectHandleTowerCanvasGoPosition(grid);
            this.refreshButtonInfo(0);
        }

        private void refreshButtonInfo(int coin)
        {
            for(int i = 0; i < this.buttonTowerList.Length; i++)
            {
                this.buttonTowerList[i].updateButtonSprite(dataComponent.CoinCount);
            }
            if (this.selectGrid == null) return;
            BattleUnit_Tower tower = towerComponent.getTowerInfo(this.selectGrid.mapGrid.x, this.selectGrid.mapGrid.y);
            if (tower == null) return;
            if(tower.isMaxLevel == true)
            {
                this.imgButtonUp.sprite = this.spriteButtonUpList[2];
                this.txtButtonUp.text = "";
            }
            else
            {
                if(dataComponent.CoinCount >= tower.price[tower.curLevel])
                {
                    this.imgButtonUp.sprite = this.spriteButtonUpList[1];
                }
                else
                {
                    this.imgButtonUp.sprite = this.spriteButtonUpList[0];
                }
                this.txtButtonUp.text = tower.price[tower.curLevel].ToString();
            }
            this.txtButtonSell.text = (tower.price[tower.curLevel] - 20).ToString();
        }

        //纠正操作塔UI画布的方法(纠正按钮位置的方法)
        private void CorrectHandleTowerCanvasGoPosition(GridPoint grid)
        {
            tranButtonUp.localPosition = Vector3.zero;
            tranButtonSell.localPosition = Vector3.zero;
            if (grid.mapGrid.y <= 0)
            {
                if (grid.mapGrid.x == 0)
                {
                    tranButtonSell.position += new Vector3(BattleConfig.MAP_RATIO * 3 / 4, 0, 0);
                }
                else
                {
                    tranButtonSell.position -= new Vector3(BattleConfig.MAP_RATIO * 3 / 4, 0, 0);
                }
                tranButtonUp.localPosition = upLevelButtonInitPos;
            }
            else if (grid.mapGrid.y >= 6)
            {
                if (grid.mapGrid.x == 0)
                {
                    tranButtonUp.position += new Vector3(BattleConfig.MAP_RATIO * 3 / 4, 0, 0);
                }
                else
                {
                    tranButtonUp.position -= new Vector3(BattleConfig.MAP_RATIO * 3 / 4, 0, 0);
                }
                tranButtonSell.localPosition = sellTowerButtonInitPos;
            }
            else
            {
                tranButtonUp.localPosition = upLevelButtonInitPos;
                tranButtonSell.localPosition = sellTowerButtonInitPos;
            }
        }

        private void fadeHandleTowerCanvas()
        {
            this.nodeHandleTowerCanvas.transform.position = this.battleView.initTran;
            this.battleView.bvEventDispatcher.dispatchEvent(BattleEvent.TOWER_RANGE_FADE);
        }

        private void showTowerList(GridPoint grid)
        {
            this.nodeTowerList.transform.position = new Vector3((float)grid.mapGrid.realX, (float)grid.mapGrid.realY, 0);
            this.nodeTowerList.transform.position += this.CorrectTowerListGoPosition(grid);
        }

        private Vector3 CorrectTowerListGoPosition(GridPoint grid)
        {
            Vector3 correctPosition = Vector3.zero;
            if (grid.mapGrid.x <= 3 && grid.mapGrid.x >= 0)
            {
                correctPosition += new Vector3(BattleConfig.MAP_RATIO, 0, 0);
            }
            else if (grid.mapGrid.x <= 11 && grid.mapGrid.x >= 8)
            {
                correctPosition -= new Vector3(BattleConfig.MAP_RATIO, 0, 0);
            }
            if (grid.mapGrid.y <= 3 && grid.mapGrid.y >= 0)
            {
                correctPosition += new Vector3(0, BattleConfig.MAP_RATIO, 0);
            }
            else if (grid.mapGrid.y <= 7 && grid.mapGrid.y >= 4)
            {
                correctPosition -= new Vector3(0, BattleConfig.MAP_RATIO, 0);
            }
            return correctPosition;
        }

        private void fadeTowerList()
        {
            this.nodeTowerList.transform.position = this.battleView.initTran;
        }

        public void HandleGrid(GridPoint grid)//当前选择的格子
        {
            if (grid.mapGrid.state.canBuild)
            {
                if (selectGrid == null)//没有上一个格子
                {
                    selectGrid = grid;
                    selectGrid.ShowGrid();
                    UIServer.getInstance().audioManager.playEffect("AudioClips/NormalMordel/Grid/GridSelect");
                }
                else if (grid == selectGrid)//选中同一个格子
                {
                    grid.HideGrid();
                    selectGrid = null;
                    this.nodeHandleTowerCanvas.transform.position = this.battleView.initTran;
                    UIServer.getInstance().audioManager.playEffect("AudioClips/NormalMordel/Grid/GridDeselect");
                }
                else if (grid != selectGrid)//选中不同格子
                {
                    selectGrid.HideGrid();
                    selectGrid = grid;
                    selectGrid.ShowGrid();
                    UIServer.getInstance().audioManager.playEffect("AudioClips/NormalMordel/Grid/GridSelect");
                }
            }
            else
            {
                grid.HideGrid();
                grid.ShowCantBuild();
                this.nodeHandleTowerCanvas.transform.position = this.battleView.initTran;
                UIServer.getInstance().audioManager.playEffect("AudioClips/NormalMordel/Grid/SelectFault");
                if (selectGrid != null)
                {
                    selectGrid.HideGrid();
                }
            }
        }

        private void updateTower()
        {
            if (this.selectGrid == null)
            {
                Debug.Log("没有当前格子，无法升级");
                return;
            }
            BattleUnit_Tower tower = towerComponent.getTowerInfo(this.selectGrid.mapGrid.x, this.selectGrid.mapGrid.y);
            if (tower.isMaxLevel == true) return;
            InputOrder order = new InputOrder();
            order.setOrder(this.battle.curFrameId + 1, this.selectGrid.mapGrid.x, this.selectGrid.mapGrid.y, InputOrderType.UPDATE_ORDER);
            ((BattleInputComponent)GameManager.getInstance().baseBattle.getComponent(BattleComponentType.InputComponent)).addOrder(order);
            this.selectGrid.HideGrid();
        }

        private void sellTower()
        {
            if (this.selectGrid == null)
            {
                Debug.Log("没有当前格子，无法出售");
                return;
            }
            InputOrder order = new InputOrder();
            order.setOrder(this.battle.curFrameId + 1, this.selectGrid.mapGrid.x, this.selectGrid.mapGrid.y, InputOrderType.REMOVE_ORDER);
            ((BattleInputComponent)GameManager.getInstance().baseBattle.getComponent(BattleComponentType.InputComponent)).addOrder(order);
            this.selectGrid.HideGrid();
        }

        private void updateNodeState(String type,BattleUnit unit)
        {
            if (type.Equals(BattleUnitType.TOWER))
            {
                this.fadeHandleTowerCanvas();
                this.fadeTowerList();
                if(this.selectGrid != null)
                {
                    this.selectGrid.HideGrid();
                    this.selectGrid = null;
                }
            }
        }

        public override void dispose()
        {
            this.carrot.dispose();
            for(int i = 0; i < this.buttonTowerList.Length - 1; i++)
            {
                this.buttonTowerList[i].dispose();
            }
            this.removeListener();
            this.selectGrid = null;
            GameObject.Destroy(this.nodeHandleTowerCanvas);
            GameObject.Destroy(this.nodeTowerList);
            GameObject.Destroy(this.nodeCarrot);
            GameObject.Destroy(this.nodeMap);
            base.dispose();
        }

    }
}
