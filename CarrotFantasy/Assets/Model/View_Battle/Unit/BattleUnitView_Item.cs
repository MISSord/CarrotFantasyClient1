using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace ETModel
{
    public class BattleUnitView_Item : BattleUnitView
    {
        private Slider slider;
        private SpriteRenderer spriteRender;
        private Item item;

        public override void initTransform(Transform node)
        {
            base.initTransform(node);
            this.slider = this.transform.Find("ItemCanvas/HpSlider").GetComponent<Slider>();
            this.spriteRender = this.transform.GetComponent<SpriteRenderer>();
            this.slider.value = 1;

            this.slider.gameObject.SetActive(false);

            this.item = this.transform.GetComponent<Item>();
            this.item.itemView = this;
        }

        public override void initListener()
        {
            base.initListener();
            this.unitEventDispatcher.addListener(BattleEvent.ITEM_LIVE_REDUCE, this.updateLiveNumber);
        }

        public override void removeListener()
        {
            base.removeListener();
            if(this.unitEventDispatcher != null)
            {
                this.unitEventDispatcher.removeListener(BattleEvent.ITEM_LIVE_REDUCE, this.updateLiveNumber);
            }
        }

        private void updateLiveNumber()
        {
            if (this.slider.gameObject.activeSelf == false) this.slider.gameObject.SetActive(true);
            this.slider.value = ((float)((BattleUnit_Item)this.unit).curLive / (float)((BattleUnit_Item)this.unit).totalLive);
        }

        public void refreshTarget()
        {
            this.battleView.battle.eventDispatcher.dispatchEvent<BattleUnit>(BattleEvent.TARGET_CHANGE, this.unit);
        }

        public override void clearUnitInfo()
        {
            base.clearUnitInfo();
            this.slider = null;
            this.spriteRender = null;
        }
    }
}
