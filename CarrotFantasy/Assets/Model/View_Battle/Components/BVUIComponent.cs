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
        private GameObject nodeTargetSignal;

        private BattleUnit tranTarget;

        private Text txtButtonSell;

        private MapUIConfigReader reader;

        private GameObject nodeCarrot;
        private GameObject nodeMonsterPoint;
        private Carrot carrot;

        private GameObject rootGameObject;

        public BVUIComponent(BattleView_base battleView) : base(battleView)
        {
            this.towerComponent = (BattleTowerComponent)this.battle.getComponent(BattleComponentType.TowerComponent);
            this.dataComponent = (BattleDataComponent)this.battle.getComponent(BattleComponentType.DataComponent);
            this.mapComponent = (BattleMapComponent)this.battle.getComponent(BattleComponentType.MapComponent);
            this.buttonTowerList = new ButtonTower[this.towerComponent.canBuildTowerListLength];
            this.spriteButtonUpList = new Sprite[3];
            this.componentType = BattleViewComponentType.UI;

            this.reader = new MapUIConfigReader();
            this.reader.init();
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
            this.eventDispatcher.addListener<String, BattleUnit>(BattleEvent.BATTLE_UNIT_REMOVE, this.updateTargetSignal);

            this.eventDispatcher.addListener<BattleUnit>(BattleEvent.TARGET_CHANGE, this.setTargetSignal);
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
            this.eventDispatcher.removeListener<String, BattleUnit>(BattleEvent.BATTLE_UNIT_REMOVE, this.updateTargetSignal);

            this.eventDispatcher.removeListener<BattleUnit>(BattleEvent.TARGET_CHANGE, this.setTargetSignal);
        }

        public override void init()
        {
            BVSceneComponent scene = (BVSceneComponent)this.battleView.getComponent(BattleViewComponentType.SCENE);
            this.rootGameObject = scene.registerGameContainer("UIContainer");

            this.nodeTowerList = GameObject.Instantiate(ResourceLoader.getInstance().getGameObject("Prefabs/Game/UI/tower_list"));
            this.nodeTowerList.transform.SetParent(this.rootGameObject.transform);
            this.nodeTowerList.transform.position = this.battleView.initTran;
            this.nodeTowerList.transform.GetComponent<Canvas>().sortingOrder = 20;

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

            this.nodeHandleTowerCanvas = GameObject.Instantiate(ResourceLoader.getInstance().getGameObject("Prefabs/Game/UI/handle_tower_canvas"));
            this.nodeHandleTowerCanvas.transform.SetParent(this.rootGameObject.transform);
            this.nodeHandleTowerCanvas.transform.position = this.battleView.initTran;
            this.nodeHandleTowerCanvas.transform.GetComponent<Canvas>().sortingOrder = 20;

            this.spriteButtonUpList[0] = ResourceLoader.getInstance().loadRes<Sprite>("Pictures/NormalMordel/Game/Tower/Btn_CantUpLevel"); //不能升级
            this.spriteButtonUpList[1] = ResourceLoader.getInstance().loadRes<Sprite>("Pictures/NormalMordel/Game/Tower/Btn_CanUpLevel"); //能升级
            this.spriteButtonUpList[2] = ResourceLoader.getInstance().loadRes<Sprite>("Pictures/NormalMordel/Game/Tower/Btn_ReachHighestLevel"); //满级

            this.nodeMap = GameObject.Instantiate(ResourceLoader.getInstance().getGameObject("Prefabs/Game/nodeMap"));
            this.nodeMap.transform.SetParent(this.rootGameObject.transform);
            this.nodeMap.transform.position = new Vector3(6, 4.35f, 0);
            Dictionary<String, int> map = this.reader.getMapUIConfig(dataComponent.bigLevel, dataComponent.level);
            this.nodeMap.transform.Find("img_bg").GetComponent<SpriteRenderer>().sprite = ResourceLoader.getInstance().
                loadRes<Sprite>(String.Format("Pictures/NormalMordel/Game/{0}/BG{1}", dataComponent.bigLevel, map["mapBg"]));
            this.nodeMap.transform.Find("img_road").GetComponent<SpriteRenderer>().sprite = ResourceLoader.getInstance().
                loadRes<Sprite>(String.Format("Pictures/NormalMordel/Game/{0}/Road{1}", dataComponent.bigLevel, map["mapRoad"]));
            this.nodeMap.transform.Find("img_bg").GetComponent<SpriteRenderer>().sortingOrder = 0;
            this.nodeMap.transform.Find("img_road").GetComponent<SpriteRenderer>().sortingOrder = 4;

            this.nodeTargetSignal = GameObject.Instantiate(ResourceLoader.getInstance().getGameObject("Prefabs/Game/node_target_signal"));
            this.nodeTargetSignal.transform.SetParent(this.rootGameObject.transform);
            this.nodeTargetSignal.transform.position = this.battleView.initTran;

            this.loadInfo();
            this.setStartPoint();
            this.setCarrot();

            this.addListener();
        }

        private void setStartPoint()
        {
            this.nodeMonsterPoint = GameObject.Instantiate(ResourceLoader.getInstance().getGameObject("Prefabs/Game/startPoint"));
            this.nodeMonsterPoint.transform.SetParent(this.rootGameObject.transform);
            Fix64Vector2 startPosition = this.mapComponent.monsterPathList[0];

            bool isRight = this.mapComponent.monsterPathList[1].X - this.mapComponent.monsterPathList[0].X > Fix64.Zero ? true : false;
            bool isUP = this.mapComponent.monsterPathList[1].Y - this.mapComponent.monsterPathList[0].Y > Fix64.Zero ? true : false;

            if (this.mapComponent.monsterPathList[1].X - this.mapComponent.monsterPathList[0].X != Fix64.Zero) //左或者右
            {
                if (isRight == true)
                {
                    this.nodeMonsterPoint.transform.position = new Vector3((float)startPosition.X , (float)startPosition.Y + 0.5f, 0);
                }
                else
                {
                    this.nodeMonsterPoint.transform.position = new Vector3((float)startPosition.X, (float)startPosition.Y + 0.3f, 0);
                }
            }
            else //上或下
            {
                if (isUP == true)
                {
                    this.nodeMonsterPoint.transform.position = new Vector3((float)startPosition.X - 0.1f, (float)startPosition.Y - 0.5f, 0);
                }
                else
                {
                    this.nodeMonsterPoint.transform.position = new Vector3((float)startPosition.X - 0.1f, (float)startPosition.Y + 0.5f, 0);
                }
            }
        }

        private void setCarrot()
        {
            this.nodeCarrot = GameObject.Instantiate(ResourceLoader.getInstance().getGameObject("Prefabs/Game/Carrot"));
            this.nodeCarrot.transform.SetParent(this.rootGameObject.transform);
            this.carrot = this.nodeCarrot.transform.GetComponent<Carrot>();
            this.carrot.init();
            Fix64Vector2 endPosition = this.mapComponent.monsterPathList[this.mapComponent.monsterPathList.Count - 1];
            this.carrot.transform.position = new Vector3((float)endPosition.X + 0.1f, (float)endPosition.Y + 0.5f, 0);
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
                if(dataComponent.CoinCount >= tower.price[tower.curLevel + 1])
                {
                    this.imgButtonUp.sprite = this.spriteButtonUpList[1];
                }
                else
                {
                    this.imgButtonUp.sprite = this.spriteButtonUpList[0];
                }
                this.txtButtonUp.text = tower.price[tower.curLevel + 1].ToString();
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

        private void updateTargetSignal(String type, BattleUnit unit)
        {
            if (type.Equals(BattleUnitType.ITEM))
            {
                if(unit == this.tranTarget)
                {
                    this.fadeTargetSignal();
                }
            }
        }

        private void setTargetSignal(BattleUnit unit)
        {
            if (this.tranTarget == null)
            {
                this.tranTarget = unit;
                this.showTargetSignal();
            }
            //转换新目标
            else if (this.tranTarget != unit)
            {
                this.tranTarget = unit;
                this.showTargetSignal();
            }
            //两次点击的是同一个目标
            else if (this.tranTarget == unit)
            {
                this.tranTarget = null;
                this.fadeTargetSignal();
            }
        }

        public override void clearGameInfo()
        {
            this.carrot.dispose();
            for (int i = 0; i < this.buttonTowerList.Length - 1; i++)
            {
                this.buttonTowerList[i].dispose();
            }
            this.removeListener();
            this.selectGrid = null;
            GameObject.Destroy(this.nodeHandleTowerCanvas);
            GameObject.Destroy(this.nodeTowerList);
            GameObject.Destroy(this.nodeCarrot);
            GameObject.Destroy(this.nodeMap);
        }

        private void showTargetSignal()
        {
            UIServer.getInstance().audioManager.playEffect("AudioClips/NormalMordel/Tower/ShootSelect");
            UnitTransformComponent tranComponent = (UnitTransformComponent)this.tranTarget.getComponent(UnitComponentType.TRANSFORM);
            Fix64Vector2 pos = tranComponent.getLastPosition();
            Vector3 position = new Vector3((float)pos.X, (float)pos.Y, 0);
            this.nodeTargetSignal.transform.position = position + new Vector3(0, BattleConfig.MAP_RATIO / 2, 0);
            //nodeTargetSignal.transform.SetParent(targetTrans);
        }

        private void fadeTargetSignal()
        {
            this.nodeTargetSignal.transform.position = this.battleView.initTran;
        }

        public override void dispose()
        {
            this.clearGameInfo();
            base.dispose();
        }

    }
}
