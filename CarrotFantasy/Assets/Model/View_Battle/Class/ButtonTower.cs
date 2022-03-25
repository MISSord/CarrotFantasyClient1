using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace ETModel
{
    public class ButtonTower
    {
        public int towerID;
        public int price;
        private Button button;
        private Sprite canClickSprite;
        private Sprite cantClickSprite;
        private Image image;

        private Transform transform;
        private BVUIComponent uiComponent;
        private int curPrice;

        public void initInfo(Transform transform, int towerId)
        {
            this.transform = transform;
            this.towerID = towerId;
            this.canClickSprite = ResourceLoader.getInstance().loadRes<Sprite>("Pictures/NormalMordel/Game/Tower/" + towerID.ToString() + "/CanClick1");
            this.cantClickSprite = ResourceLoader.getInstance().loadRes<Sprite>("Pictures/NormalMordel/Game/Tower/" + towerID.ToString() + "/CanClick0");

            this.image = this.transform.GetComponent<Image>();
            this.button = this.transform.GetComponent<Button>();

            this.image.sprite = this.canClickSprite;
            this.button.onClick.AddListener(this.buildTower);

            this.curPrice = (int)(((BattleTowerComponent)this.uiComponent.battle.getComponent(BattleComponentType.TowerComponent)).
                configReader.getSingleTowerConfig(this.towerID)["price0"]);
        }

        public void loadInfo(BVUIComponent baseView)
        {
            this.uiComponent = baseView;
        }

        public void updateButtonSprite(int coin)
        {
            if(coin >= this.curPrice)
            {
                this.image.sprite = this.canClickSprite;
            }
            else
            {
                this.image.sprite = this.cantClickSprite;
            }
        }

        
        public void buildTower()
        {
            if(this.uiComponent.selectGrid != null)
            {
                InputOrder curOrder = new InputOrder();
                curOrder.setOrder(this.uiComponent.battle.curFrameId + 1,
                    this.uiComponent.selectGrid.mapGrid.x, this.uiComponent.selectGrid.mapGrid.y, InputOrderType.ADD_ORDER);
                curOrder.setTowerId(this.towerID);

                ((BattleInputComponent)GameManager.getInstance().baseBattle.getComponent(BattleComponentType.InputComponent)).addOrder(curOrder);
            }
            this.uiComponent.HandleGrid(this.uiComponent.selectGrid);
        }

        public void dispose()
        {
            this.button.onClick.RemoveAllListeners();
        }
    }
}
