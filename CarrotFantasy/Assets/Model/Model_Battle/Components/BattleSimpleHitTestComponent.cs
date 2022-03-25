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

        public BattleSimpleHitTestComponent(BaseBattle bBattle) : base(bBattle)
        {
            this.componentType = BattleComponentType.HitTestComponent;
        }

        public override void init()
        {
            this.registerList(BattleUnitType.BULLET);
            this.registerList(BattleUnitType.MONSTER);
            this.registerList(BattleUnitType.TOWER);

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
        }

        private void removeListener()
        {
            this.eventDispatcher.removeListener<String, BattleUnit>(BattleEvent.BATTLE_UNIT_ADD, this.registerNewBattleUnit);
            this.eventDispatcher.removeListener<String, BattleUnit>(BattleEvent.BATTLE_UNIT_REMOVE, this.removeBattleUnit);
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
            if (type.Equals(BattleUnitType.MONSTER))
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
            if (type.Equals(BattleUnitType.MONSTER))
            {
                this.curShouldCallBackDic.Remove(battle);
            }
        }

        public override void onTick(Fix64 time)
        {
            this.chooseSingleBeHit(BattleUnitType.MONSTER, BattleUnitType.BULLET);
            this.chooseSingleBeHit(BattleUnitType.MONSTER, BattleUnitType.TOWER);

            this.exeTheCallBack();
        }

        private void chooseSingleBeHit(String type1,String type2)
        {
            UnitTransformComponent unit1;
            UnitTransformComponent unit2;
            for (int i = 0; i <= this.registerHitTestShapeDic[type1].Count - 1; i++)
            {
                unit1 = this.registerHitTestShapeDic[type1][i];
                for (int j = 0; j <= this.registerHitTestShapeDic[type2].Count - 1; j++)
                {
                    unit2 = this.registerHitTestShapeDic[type2][j];
                    if(unit2.unit.unitType == BattleUnitType.BULLET)
                    {
                        bool isHit = HitTestHandler.hitTest(unit1.bodyHitTestShape, unit2.bodyHitTestShape);
                        if (isHit == true)
                        {
                            this.curShouldCallBackDic[unit1.unit].Add(unit2.unit);
                        }
                    }
                    else
                    {
                        bool isHit = HitTestHandler.hitTest(unit1.bodyHitTestShape, unit2.bodyHitTestShape);
                        if (isHit == true)
                        {
                            this.curShouldCallBackDic[unit1.unit].Add(unit2.unit);
                        }
                    }
                }
            }
        }

        private void exeTheCallBack()
        {
            if (this.curShouldCallBackDic.Count == 0) return;
            foreach(KeyValuePair<BattleUnit, List<BattleUnit>> info in this.curShouldCallBackDic)
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

        public override void clearInfo()
        {
            this.curShouldCallBackDic.Clear();
            this.registerHitTestShapeDic.Clear();
            this.registerUnitDic.Clear();
            this.removeListener();
        }

        public override void dispose()
        {
            this.clearInfo();
            base.dispose();
        }
    }
}
