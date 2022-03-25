using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ETModel
{
    public class BattleUnitView_Tower : BattleUnitView
    {
        private Animator animator;
        private GameObject nodeAttackRange;

        public override void initTransform(Transform node)
        {
            base.initTransform(node);
            transform.Find("tower").TryGetComponent<Animator>(out this.animator);
            float scale = (float)((BattleUnit_Tower)this.unit).towerAttackRadius;
            this.nodeAttackRange = this.transform.Find("attackRange").gameObject;
            this.nodeAttackRange.transform.localScale = new Vector3(scale - 0.2f, scale - 0.2f, 1);
            this.nodeAttackRange.SetActive(false);
        }

        public override void initListener()
        {
            base.initListener();
            this.unit.eventDipatcher.addListener(BattleEvent.TOWER_ATTACK, this.playAnimation);
            this.battleView.bvEventDispatcher.addListener<GridPoint>(BattleEvent.TOWER_RANGE_SHOW, this.showRange);
            this.battleView.bvEventDispatcher.addListener(BattleEvent.TOWER_RANGE_FADE, this.fadeRange);
        }

        public override void removeListener()
        {
            this.battleView.bvEventDispatcher.removeListener<GridPoint>(BattleEvent.TOWER_RANGE_SHOW, this.showRange);
            this.battleView.bvEventDispatcher.removeListener(BattleEvent.TOWER_RANGE_FADE, this.fadeRange);
            base.removeListener();
            if (this.unit == null) return;
            this.unit.eventDipatcher.removeListener(BattleEvent.TOWER_ATTACK, this.playAnimation);
        }

        private void showRange(GridPoint grid)
        {
            if(grid.mapGrid.x == ((BattleUnit_Tower)this.unit).x && grid.mapGrid.y == ((BattleUnit_Tower)this.unit).y)
            {
                this.nodeAttackRange.SetActive(true);
            }
        }

        private void fadeRange()
        {
            if(this.nodeAttackRange.activeSelf == true)
            {
                this.nodeAttackRange.SetActive(false);
            }
        }

        private void playAnimation()
        {
            if (this.animator == null) return;
            this.animator.Play("Attack");
        }

        public override void clearUnitInfo()
        {
            this.transform = null;
            this.nodeAttackRange = null;
            this.animator = null;
            base.clearUnitInfo();
        }
    }
}
