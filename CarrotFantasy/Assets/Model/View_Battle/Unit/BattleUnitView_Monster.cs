using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace ETModel
{
    public class BattleUnitView_Monster : BattleUnitView
    {
        private Slider slider;
        private SpriteRenderer spriteRender;
        private Animator animator;

        private String spriteUrl = "Pictures/NormalMordel/Game/{0}/Monster/{1}-1";
        private String animatorUrl = "Animator/AnimatorController/Monster/{0}/{1}";

        public override void initTransform(Transform node)
        {
            base.initTransform(node);
            this.slider = this.transform.Find("MonsterCanvas/HPSlider").GetComponent<Slider>();
            this.spriteRender = this.transform.GetComponent<SpriteRenderer>();
            this.animator = this.transform.GetComponent<Animator>();
            this.slider.value = 1;
        }

        public override void init()
        {
            base.init();
            BattleUnit_Monster monster = (BattleUnit_Monster)this.unit;
            this.spriteRender.sprite = ResourceLoader.getInstance().loadRes<Sprite>(
                String.Format(this.spriteUrl, monster.curLevel, monster.monsterId));
            this.animator.runtimeAnimatorController = ResourceLoader.getInstance().loadRes<RuntimeAnimatorController>(
                String.Format(this.animatorUrl, monster.curLevel, monster.monsterId));
            this.animator.Play(monster.monsterId.ToString());

            this.unitEventDispatcher.addListener(BattleEvent.MONSTER_LIVE_REDUCE, this.updateLiveNumber);
        }

        private void updateLiveNumber()
        {
            this.slider.value = ((float)((BattleUnit_Monster)this.unit).curLive / (float)((BattleUnit_Monster)this.unit).totalLive);
        }

        protected override void updateFaceDirection()
        {
            base.updateFaceDirection();
            this.slider.gameObject.transform.eulerAngles = Vector3.zero;
        }

        public override void clearUnitInfo()
        {
            if(this.unitEventDispatcher != null)
            {
                this.unitEventDispatcher.removeListener(BattleEvent.MONSTER_LIVE_REDUCE, this.updateLiveNumber);
            }
            base.clearUnitInfo();
            this.slider = null;
            this.animator = null;
            this.spriteRender = null;
        }
    }
}
