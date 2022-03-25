using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using System;

namespace ETModel
{
    public class GridPoint : MonoBehaviour
    { 
        //属性
        private SpriteRenderer spriteRenderer;

        private BattleView_base battleView;

        private string itemPrefabUrl;

        public BattleMapGrid mapGrid { get; private set; }

        private Sprite startSprite;
        private Sprite normalSprite;
        private Sprite cantBuildSprite;

        private BVMapComponent bvMapComponent;
        private GameObject levelUpSignalGo;//是否可升级信号



        public void initTrans(BattleView_base battleView)
        {
            this.battleView = battleView;
            this.spriteRenderer = transform.GetComponent<SpriteRenderer>();
            levelUpSignalGo = transform.Find("LevelUpSignal").gameObject;
            levelUpSignalGo.SetActive(false);
        }

        public void initInfo(int x, int y)
        {
            BVMapComponent bvMap = (BVMapComponent)battleView.getComponent(BattleViewComponentType.MAP);
            this.startSprite = bvMap.sprGirdStartState;
            this.normalSprite = bvMap.sprGirdNoramlState;
            this.cantBuildSprite = bvMap.sprGirdCantBuildState;

            BattleMapComponent map = (BattleMapComponent)(this.battleView.battle.getComponent(BattleComponentType.MapComponent));
            this.mapGrid = map.gridsList[x, y];
            this.itemPrefabUrl = "Prefabs/Game/Item/" + map.levelInfo.bigLevelID + "/" + mapGrid.state.itemID.ToString();
            this.updateGrid();
        }

        public void startGame()
        {
            this.spriteRenderer.sprite = this.startSprite;
            Tween t = DOTween.To(() => spriteRenderer.color, toColor => spriteRenderer.color = toColor, new Color(1, 1, 1, 0.2f), 3);
            t.OnComplete(ChangeSpriteToGrid);
        }

        //改回原来样式的Sprite
        private void ChangeSpriteToGrid()
        {
            spriteRenderer.enabled = false;
            spriteRenderer.color = new Color(1, 1, 1, 1);

            if (this.mapGrid.state.canBuild)
            {
                spriteRenderer.sprite = this.normalSprite;
            }
            else
            {
                spriteRenderer.sprite = this.cantBuildSprite;
            }
        }


        //更新格子状态
        public void updateGrid()
        {
            if (this.mapGrid.state.canBuild)
            {
                spriteRenderer.enabled = true;
                if (this.mapGrid.state.hasItem)
                {
                    this.createItem();
                }
            }
            else
            {
                spriteRenderer.enabled = false;
            }
        }

        //创建物品
        private void createItem()
        {
            GameObject itemGo = GameObject.Instantiate(ResourceLoader.getInstance().getGameObject(this.itemPrefabUrl));
            itemGo.transform.SetParent(transform);
            itemGo.transform.GetChild(0).gameObject.SetActive(false);

            Vector3 createPos = transform.position - new Vector3(0, 0, 3);
            if (this.mapGrid.state.itemID <= 2)
            {
                createPos += new Vector3(BattleConfig.MAP_RATIO, -BattleConfig.MAP_RATIO) / 2;
            }
            else if (this.mapGrid.state.itemID <= 4)
            {
                createPos += new Vector3(BattleConfig.MAP_RATIO, 0) / 2;
            }
            itemGo.transform.position = createPos;
        }

        /// <summary>
        /// 有关格子处理的方法
        /// </summary>

        private void OnMouseDown()
        {
            //选择的是UI则不发生交互
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return;
            this.battleView.bvEventDispatcher.dispatchEvent<GridPoint>(BattleViewEventType.Select_Grid, this);
        }

        public void ShowGrid()
        {
            if (this.mapGrid.hasTower == true)
            {
                spriteRenderer.enabled = true;
                this.battleView.bvEventDispatcher.dispatchEvent<GridPoint>(BattleViewEventType.Show_Handle_Tower, this);
            }
            else
            {
                //handleTowerGanvasGo.SetActive(true);
                //显示塔的攻击范围
                //towerGo.transform.Find("attackRange").gameObject.SetActive(true);

                spriteRenderer.enabled = true;
                //展示建塔列表
                this.battleView.bvEventDispatcher.dispatchEvent<GridPoint>(BattleViewEventType.Show_Tower_List, this);
            }
        }

        public void HideGrid()
        {
            if (this.mapGrid.hasTower == true)
            {
                //隐藏建塔列表
                this.battleView.bvEventDispatcher.dispatchEvent(BattleViewEventType.Fade_Handle_Tower);
            }
            else
            {
                //handleTowerGanvasGo.SetActive(false);
                //隐藏塔的范围
                //towerGo.transform.Find("attackRange").gameObject.SetActive(false);
                //隐藏建塔列表
                this.battleView.bvEventDispatcher.dispatchEvent(BattleViewEventType.Fade_Tower_List);
            }
            spriteRenderer.enabled = false;
        }

        //显示此格子不能够去建塔
        public void ShowCantBuild()
        {
            spriteRenderer.enabled = true;
            Tween t = DOTween.To(() => spriteRenderer.color, toColor => spriteRenderer.color = toColor, new Color(1, 1, 1, 0), 2f);
            t.OnComplete(() =>
            {
                spriteRenderer.enabled = false;
                spriteRenderer.color = new Color(1, 1, 1, 1);
            });
        }
    }
}

