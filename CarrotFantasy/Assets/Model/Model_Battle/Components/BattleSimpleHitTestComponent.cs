using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ETModel
{
    public class BattleSimpleHitTestComponent : BaseBattleComponent
    {
        private Dictionary<string, List<BattleUnit>> registerUnitDic = new Dictionary<string, List<BattleUnit>>();
        private Dictionary<string, List<UnitTransformComponent>> registerHitTestShapeDic = new Dictionary<string, List<UnitTransformComponent>>();

        private Dictionary<BattleUnit, List<BattleUnit>> curShouldCallBackDic = new Dictionary<BattleUnit, List<BattleUnit>>();

        private BattleUnit targetUnit = null;

        public BattleSimpleHitTestComponent(BaseBattle bBattle) : base(bBattle)
        {
            this.componentType = BattleComponentType.HitTestComponent;
        }

        public override void init()
        {
            this.registerList(BattleUnitType.BULLET);
            this.registerList(BattleUnitType.MONSTER);
            this.registerList(BattleUnitType.TOWER);
            this.registerList(BattleUnitType.ITEM);

            this.addListener();
        }

        private void registerList(String type)
        {
            if (!this.registerUnitDic.ContainsKey(type))
            {
                this.registerUnitDic.Add(type, new List<BattleUnit>());
                this.registerHitTestShapeDic.Add(type, new List<UnitTransformComponent>());
            }
        }

        private void addListener()
        {
            this.eventDispatcher.addListener<String, BattleUnit>(BattleEvent.BATTLE_UNIT_ADD, this.registerNewBattleUnit);
            this.eventDispatcher.addListener<String, BattleUnit>(BattleEvent.BATTLE_UNIT_REMOVE, this.removeBattleUnit);
            this.eventDispatcher.addListener<BattleUnit>(BattleEvent.TARGET_CHANGE, this.setTarget);
        }

        private void removeListener()
        {
            this.eventDispatcher.removeListener<String, BattleUnit>(BattleEvent.BATTLE_UNIT_ADD, this.registerNewBattleUnit);
            this.eventDispatcher.removeListener<String, BattleUnit>(BattleEvent.BATTLE_UNIT_REMOVE, this.removeBattleUnit);
            this.eventDispatcher.removeListener<BattleUnit>(BattleEvent.TARGET_CHANGE, this.setTarget);
        }

        private void registerNewBattleUnit(String type, BattleUnit battle)
        {
            UnitBeHitComponent beHit = (UnitBeHitComponent)battle.getComponent(UnitComponentType.BEHIT);
            if (beHit == null) return;
            if (!this.registerUnitDic.ContainsKey(type))
            {
                Debug.Log(String.Format("没有注册{0}的碰撞链表", type));
                return;
            }
            this.registerHitTestShapeDic[type].Add((UnitTransformComponent)battle.getComponent(UnitComponentType.TRANSFORM));
            this.registerUnitDic[type].Add(battle);
            if (type.Equals(BattleUnitType.MONSTER) || type.Equals(BattleUnitType.ITEM))
            {
                this.curShouldCallBackDic[battle] = new List<BattleUnit>();
            }
        }

        private void removeBattleUnit(String type, BattleUnit battle)
        {
            UnitBeHitComponent beHit = (UnitBeHitComponent)battle.getComponent(UnitComponentType.BEHIT);
            if (beHit == null) return;
            if (!this.registerUnitDic.ContainsKey(type))
            {
                Debug.Log(String.Format("移除{0}的碰撞信息失败", type));
                return;
            }
            this.registerHitTestShapeDic[type].Remove((UnitTransformComponent)battle.getComponent(UnitComponentType.TRANSFORM));
            this.registerUnitDic[type].Remove(battle);
            if (this.curShouldCallBackDic.ContainsKey(battle))
            {
                this.curShouldCallBackDic.Remove(battle);
            }
            if(this.targetUnit == battle)
            {
                this.setTarget(null);
            }
        }

        public override void onTick(Fix64 time)
        {
            this.chooseSingleBeHit(BattleUnitType.MONSTER, BattleUnitType.BULLET);
            this.chooseSingleBeHit(BattleUnitType.MONSTER, BattleUnitType.TOWER);
            this.chooseSingleBeHit(BattleUnitType.ITEM, BattleUnitType.BULLET);

            if(this.targetUnit != null)
            {
                this.chooseSingleBeHit();
            }

            this.exeTheCallBack();
        }

        private void chooseSingleBeHit(String type1, String type2)
        {
            UnitTransformComponent unit1;
            UnitTransformComponent unit2;
            for (int i = 0; i <= this.registerHitTestShapeDic[type1].Count - 1; i++)
            {
                unit1 = this.registerHitTestShapeDic[type1][i];
                for (int j = 0; j <= this.registerHitTestShapeDic[type2].Count - 1; j++)
                {
                    unit2 = this.registerHitTestShapeDic[type2][j];
                    bool isHit = HitTestHandler.hitTest(unit1.bodyHitTestShape, unit2.bodyHitTestShape);
                    if (isHit == true)
                    {
                        this.curShouldCallBackDic[unit1.unit].Add(unit2.unit);
                    }
                }
            }
        }

        private void chooseSingleBeHit()
        {
            UnitTransformComponent unit1;
            UnitTransformComponent unit2 = (UnitTransformComponent)this.targetUnit.getComponent(UnitComponentType.TRANSFORM);
            for (int i = 0; i <= this.registerHitTestShapeDic[BattleUnitType.TOWER].Count - 1; i++)
            {
                unit1 = this.registerHitTestShapeDic[BattleUnitType.TOWER][i];
                bool isHit = HitTestHandler.hitTest(unit1.bodyHitTestShape, unit2.bodyHitTestShape);
                if (isHit == true)
                {
                    ((BattleUnit_Tower)this.registerUnitDic[BattleUnitType.TOWER][i]).targetUnit = this.targetUnit;
                }
            }
        }

        private void exeTheCallBack()
        {
            if (this.curShouldCallBackDic.Count == 0) return;
            foreach (KeyValuePair<BattleUnit, List<BattleUnit>> info in this.curShouldCallBackDic)
            {
                UnitBeHitComponent tranBeHit = (UnitBeHitComponent)info.Key.getComponent(UnitComponentType.BEHIT);
                for (int i = 0; i <= info.Value.Count - 1; i++)
                {
                    UnitBeHitComponent beHit = (UnitBeHitComponent)info.Value[i].getComponent(UnitComponentType.BEHIT);
                    beHit.beHitCallBack(info.Key);
                    tranBeHit.beHitCallBack(info.Value[i]);
                }
                info.Value.Clear();
            }
        }

        private void setTarget(BattleUnit unit)
        {
            if (this.targetUnit == null)
            {
                this.targetUnit = unit;
            }
            //转换新目标
            else if (this.targetUnit != unit)
            {
                this.targetUnit = unit;
            }
            //两次点击的是同一个目标
            else if (this.targetUnit == unit)
            {
                this.targetUnit = null;
            }
            this.setCallBackList(unit);
        }

        private void setCallBackList(BattleUnit unit)
        {
            if (unit == null) return;
            if (!this.curShouldCallBackDic.ContainsKey(unit))
            {
                this.curShouldCallBackDic[unit] = new List<BattleUnit>();
            }
            this.curShouldCallBackDic[unit].Clear();
        }

        public override void clearInfo()
        {
            this.curShouldCallBackDic.Clear();
            this.registerHitTestShapeDic.Clear();
            this.registerUnitDic.Clear();
            this.targetUnit = null;
            this.removeListener();
        }

        public override void dispose()
        {
            this.clearInfo();
            base.dispose();
        }
    }
}
