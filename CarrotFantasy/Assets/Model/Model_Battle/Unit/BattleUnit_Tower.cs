using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ETModel
{
    public class BattleUnit_Tower : BattleUnit
    {
        public Fix64 towerAttackRadius { get; private set; }
        public int towerID { get; private set; }

        private Fix64 attackCD; //攻击CD
        private Fix64 timeVal;  //攻击时间计时

        public bool isCanUpdate { get; private set; }
        public bool isMaxLevel { get; private set; }

        public int curLevel { get; private set; }
        public int[] price { get; private set; }

        public int curPrice;

        public int x { get; private set; }
        public int y { get; private set; } //地图坐标

        private UnitBeHitComponent unitBeHit;
        private UnitTransformComponent unitTrans;

        private List<BattleUnit_Monster> monsterList;
        public BattleUnit targetUnit { get; set; }

        public BattleUnit_Tower(BaseBattle battle) : base(battle)
        {
            this.unitType = BattleUnitType.TOWER;
            this.monsterList = new List<BattleUnit_Monster>();
        }

        public override void loadInfo(int uid, Dictionary<string, Fix64> param, Fix64Vector2 birthPosition)
        {
            base.loadInfo(uid, param, birthPosition);
            this.towerID = (int)param["towerID"];
            this.price = new int[3];
            this.price[0] = (int)param["price0"];
            this.price[1] = (int)param["price1"];
            this.price[2] = (int)param["price2"];
            this.attackCD = param["attackCD"];
            this.isCanUpdate = true;
            this.isMaxLevel = false;
            this.curLevel = 0;
            this.towerAttackRadius = param["bodyRadius0"];
            this.timeVal = Fix64.Zero;
        }

        public void loadInfo1(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public override void init()
        {
            base.init();
            this.unitBeHit = GameObjectPool.getInstance().getNewUnitComponent<UnitBeHitComponent>(UnitComponentType.BEHIT);
            if (this.unitBeHit == null)
            {
                this.unitBeHit = new UnitBeHitComponent();
            }
            this.unitTrans = GameObjectPool.getInstance().getNewUnitComponent<UnitTransformComponent>(UnitComponentType.TRANSFORM);
            if (this.unitTrans == null)
            {
                this.unitTrans = new UnitTransformComponent();
            }
            this.addComponent(this.unitBeHit);
            this.addComponent(this.unitTrans);

            this.unitBeHit.registerBeHitCallBack(this.beHitCallBack);
        }

        public override void initComponents()
        {
            base.initComponents();
            this.unitTrans.setBodyRadius(this.towerAttackRadius);
        }

        private void beHitCallBack(BattleUnit unit)
        {
            if (unit.unitType.Equals(BattleUnitType.MONSTER) == false)
            {
                Debug.Log(String.Format("防御塔碰撞过程出错，被碰撞对象为{0}", unit.unitType));
                return;
            }
            this.monsterList.Add((BattleUnit_Monster)unit);
        }

        public void updateLevel()
        {
            this.curLevel = this.curLevel + 1;
            this.curPrice = this.price[this.curLevel];
            this.isMaxLevel = this.curLevel == this.price.Length - 1 ? true : false;
            this.towerAttackRadius = this.birthParam["bodyRadius" + this.curLevel.ToString()];

            this.unitTrans.setBodyRadius(this.towerAttackRadius);

            this.eventDipatcher.dispatchEvent<BattleUnit_Tower>(BattleEvent.TOWER_LEVEL_UP, this);
        }

        public override void onTick(Fix64 deltaTime)
        {
            this.timeVal += deltaTime;
            if (this.timeVal >= this.attackCD)
            {
                BattleUnit targetOne = null;
                if (this.targetUnit != null)
                {
                    targetOne = this.targetUnit;
                }
                else
                {
                    if(this.monsterList.Count != 0)
                    {
                        BattleUnit_Monster curMonster = this.monsterList[0];
                        for (int i = 0; i <= monsterList.Count - 1; i++)
                        {
                            if (curMonster.EndPointDistance >= this.monsterList[i].EndPointDistance)
                            {
                                curMonster = this.monsterList[i];
                            }
                        }
                        targetOne = curMonster;
                    }
                }
                if(targetOne != null)
                {
                    this.eventDipatcher.dispatchEvent<BattleUnit>(BattleEvent.TOWER_ATTACK, targetOne);
                    this.baseBattle.eventDispatcher.dispatchEvent<BattleUnit_Tower, BattleUnit>(BattleEvent.BULLET_BUILD, this, targetOne);
                    this.timeVal = Fix64.Zero;
                }
            }
            this.monsterList.Clear();
            this.targetUnit = null;
        }

        public override void ClearInfo()
        {
            this.targetUnit = null;
            this.monsterList.Clear();
            base.ClearInfo();
        }

        public override void dispose()
        {
            this.ClearInfo();
            base.dispose();
        }
    }
}
