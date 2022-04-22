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
        private int towerId = 0;
        private Transform tran_tower;
        private UnitTransformComponent unitTran;

        public override void initTransform(Transform node)
        {
            base.initTransform(node);
            transform.Find("tower").TryGetComponent<Animator>(out this.animator);
            float scale = (float)((BattleUnit_Tower)this.unit).towerAttackRadius;
            this.towerId = ((BattleUnit_Tower)this.unit).towerID;
            if (this.towerId == 1)
            {
                this.tran_tower = this.transform.Find("tower").GetComponent<Transform>();
                this.tran_tower.eulerAngles = Vector3.zero;
                this.unitTran = (UnitTransformComponent)this.unit.getComponent(UnitComponentType.TRANSFORM);
            }
            this.nodeAttackRange = this.transform.Find("attackRange").gameObject;
            this.nodeAttackRange.transform.localScale = new Vector3(scale - 0.2f, scale - 0.2f, 1);
            this.nodeAttackRange.SetActive(false);
        }

        public override void initListener()
        {
            base.initListener();
            this.unit.eventDipatcher.addListener<BattleUnit>(BattleEvent.TOWER_ATTACK, this.playAnimation);
            this.battleView.bvEventDispatcher.addListener<GridPoint>(BattleEvent.TOWER_RANGE_SHOW, this.showRange);
            this.battleView.bvEventDispatcher.addListener(BattleEvent.TOWER_RANGE_FADE, this.fadeRange);
        }

        public override void removeListener()
        {
            this.battleView.bvEventDispatcher.removeListener<GridPoint>(BattleEvent.TOWER_RANGE_SHOW, this.showRange);
            this.battleView.bvEventDispatcher.removeListener(BattleEvent.TOWER_RANGE_FADE, this.fadeRange);
            base.removeListener();
            if (this.unit == null) return;
            this.unit.eventDipatcher.removeListener<BattleUnit>(BattleEvent.TOWER_ATTACK, this.playAnimation);
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

        private void playAnimation(BattleUnit unit)
        {
            if (this.animator == null) return;
            if (this.towerId == 1)
            {
                UnitTransformComponent unitTran = (UnitTransformComponent)unit.getComponent(UnitComponentType.TRANSFORM);
                Fix64 arcsinOne = Fix64.Zero;
                Fix64 x = (unitTran.lastFrameX - this.unitTran.lastFrameX);
                Fix64 y = (unitTran.lastFrameY - this.unitTran.lastFrameY);
                if (x == Fix64.Zero)
                {
                    if (y > Fix64.Zero)
                    {
                        arcsinOne = Fix64.Pi / Fix64.Two;
                    }
                    else
                    {
                        arcsinOne = -(Fix64.Pi / Fix64.Two);
                    }
                }
                else
                {
                    arcsinOne = Fix64.Atan(y / x);
                    if (x < Fix64.Zero)
                    {
                        arcsinOne = Fix64.Pi + arcsinOne;
                    }
                }
                this.tran_tower.eulerAngles = new Vector3(0, 0, (float)(arcsinOne * (Fix64.Semicircle / Fix64.Pi)));
            }
            this.animator.Play("Attack");
        }

        public override void clearUnitInfo()
        {
            this.transform = null;
            this.nodeAttackRange = null;
            this.animator = null;
            this.tran_tower = null;
            this.towerId = 0;
            base.clearUnitInfo();
        }
    }
}
