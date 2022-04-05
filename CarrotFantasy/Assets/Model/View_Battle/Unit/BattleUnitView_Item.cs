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

        public override void initTransform(Transform node)
        {
            base.initTransform(node);
            this.slider = this.transform.Find("MonsterCanvas/HPSlider").GetComponent<Slider>();
            this.spriteRender = this.transform.GetComponent<SpriteRenderer>();
            this.slider.value = 1;
        }

        public override void initListener()
        {
            base.initListener();
            this.unitEventDispatcher.addListener(BattleEvent.ITEM_LIVE_REDUCE, this.updateLiveNumber);
        }

        public override void removeListener()
        {
            base.removeListener();
            this.unitEventDispatcher.removeListener(BattleEvent.ITEM_LIVE_REDUCE, this.updateLiveNumber);
        }

        private void updateLiveNumber()
        {
            this.slider.value = ((float)((BattleUnit_Item)this.unit).curLive / (float)((BattleUnit_Item)this.unit).totalLive);
        }

        public override void clearUnitInfo()
        {
            base.clearUnitInfo();
            this.slider = null;
            this.spriteRender = null;
        }
    }
}
