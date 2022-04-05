using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ETModel
{
    public class BattleMonsterComponent : BaseBattleComponent
    {
        public Dictionary<int, BattleUnit_Monster> curMonsterDic { get; private set; }
        private List<BattleUnit_Monster> curNoRegisterList;

        private LevelInfo levelInfo;
        private List<Round.RoundInfo> roundInfo;

        private BattleDataComponent battleDataComponent;

        private MonsterConfigReader monsterConfigReader;

        private bool isHaveNoRegisterMonster = false;

        private List<BattleUnit_Monster> curDeadMonsterList;

        private Fix64Vector2 birthPoint;
        public int scheId { get; set; } //这个由状态机赋值

        private List<Fix64Vector2> monsterPointList; //怪兽路径

        private Fix64 distance;

        public BattleMonsterComponent(BaseBattle bBattle) : base(bBattle)
        {
            this.componentType = BattleComponentType.MonsterComponent;

            this.curMonsterDic = new Dictionary<int, BattleUnit_Monster>();
            this.curNoRegisterList = new List<BattleUnit_Monster>();
            this.curDeadMonsterList = new List<BattleUnit_Monster>();
            this.scheId = 0;
        }

        public override void init()
        {
            this.levelInfo = BattleParamServer.getInstance().info;
            this.roundInfo = levelInfo.roundInfo;

            this.monsterConfigReader = new MonsterConfigReader();
            this.monsterConfigReader.init();

            this.battleDataComponent = (BattleDataComponent)this.baseBattle.getComponent(BattleComponentType.DataComponent);
            BattleMapComponent map = (BattleMapComponent)this.baseBattle.getComponent(BattleComponentType.MapComponent);
            this.birthPoint = map.startPoint;

            this.calcaTheTotalDistance();
        }

        public void buildNewWavesMonster()
        {
            if(this.curNoRegisterList.Count != 0)
            {
                Debug.LogError("当前怪物注册列表没有清空");
                return;
            }
            else if(this.curMonsterDic.Count != 0)
            {
                Debug.LogError("当前怪物字典没有清空");
                return;
            }
            int curWaves = this.battleDataComponent.curWaves;
            Round.RoundInfo curMonsterList = this.roundInfo[curWaves - 1];

            for(int i = 0; i < curMonsterList.mMonsterIDList.Length; i++)
            {
                BattleUnit_Monster monster = GameObjectPool.getInstance().getNewBattleUnit<BattleUnit_Monster>(BattleUnitType.MONSTER);
                if (monster == null)
                {
                    monster = new BattleUnit_Monster(this.baseBattle);
                }
                monster.eventDipatcher.addListener<BattleUnit_Monster>(BattleEvent.MONSTER_DIED, this.addDeadList);
                monster.loadInfo(this.baseBattle.getUid(), this.monsterConfigReader.getSingleMonsterConfig(this.getMonsterId(curMonsterList.mMonsterIDList[i])), birthPoint);
                monster.loadInfo2(this.battleDataComponent.bigLevel, curMonsterList.mMonsterIDList[i]);
                monster.init();
                monster.loadInfo3(this.monsterPointList, this.distance);
                monster.initComponents();
                this.curNoRegisterList.Add(monster);
            }
            this.isHaveNoRegisterMonster = true;
        }

        private void calcaTheTotalDistance()
        {
            BattleMapComponent mapComponent = (BattleMapComponent)this.baseBattle.getComponent(BattleComponentType.MapComponent);
            this.monsterPointList = mapComponent.monsterPathList;
            for (int i = 0; i <= this.monsterPointList.Count - 1; i++)
            {
                if (i + 1 >= this.monsterPointList.Count) break;
                if (this.monsterPointList[i].X == this.monsterPointList[i + 1].X)
                {
                    if(this.monsterPointList[i].Y >= this.monsterPointList[i + 1].Y)
                    {
                        this.distance += this.monsterPointList[i].Y - this.monsterPointList[i + 1].Y;
                    }
                    else
                    {
                        this.distance += this.monsterPointList[i + 1].Y - this.monsterPointList[i].Y;
                    }
                }
                else
                {
                    if (this.monsterPointList[i].X >= this.monsterPointList[i + 1].X)
                    {
                        this.distance += this.monsterPointList[i].X - this.monsterPointList[i + 1].X;
                    }
                    else
                    {
                        this.distance += this.monsterPointList[i + 1].X - this.monsterPointList[i].X;
                    }
                }
            }
        }

        public int getMonsterId(int monsterId)
        {
            return this.battleDataComponent.bigLevel * 100 + monsterId;
        }

        public void registerNewMonster()
        {
            BattleUnit_Monster monster = this.curNoRegisterList[0];
            this.curNoRegisterList.RemoveAt(0);
            this.curMonsterDic.Add(monster.uid, monster);
            this.eventDispatcher.dispatchEvent<String, BattleUnit>(BattleEvent.BATTLE_UNIT_ADD, BattleUnitType.MONSTER, monster);
            //Debug.Log(String.Format("注册新的怪兽,怪兽id{0}_{1}",monster.curLevel,monster.monsterId));
            if(this.curNoRegisterList.Count == 0)
            {
                this.isHaveNoRegisterMonster = false;
                this.removeSchId();
                Debug.Log("注册新的怪兽工作完成");
            }
        }

        private void addDeadList(BattleUnit_Monster monster)
        {
            this.curDeadMonsterList.Add(monster);
        }

        public void checkSingleMonsterState(BattleUnit_Monster monster)
        {
            if (monster.isDead() == true)
            {
                //Debug.Log("怪兽死亡");
                this.eventDispatcher.dispatchEvent<String, BattleUnit>(BattleEvent.BATTLE_UNIT_REMOVE, BattleUnitType.MONSTER, monster);
                this.baseBattle.eventDispatcher.dispatchEvent<int>(BattleEvent.COIN_CHANGE, 50);
                //先从其他组件上除去，再从视图移除，最后再自己移除，确保顺序
                monster.ClearInfo();
                this.curMonsterDic.Remove(monster.uid);
                GameObjectPool.getInstance().pushObjectToPool(BattleUnitType.MONSTER, monster);
            }
        }

        public override void onTick(Fix64 time)
        {
            base.onTick(time);
            foreach (KeyValuePair<int, BattleUnit_Monster> info in this.curMonsterDic)
            {
                info.Value.onTick(time);
            }
            this.updateCurMonsterWavesState();
        }

        public override void lateTick(Fix64 time)
        {
            base.lateTick(time);
            this.updateCurMonsterWaveStateLateTick(time);
        }

        public void updateCurMonsterWavesState()
        {
            if(this.curDeadMonsterList.Count != 0)
            {
                for(int i = 0; i < this.curDeadMonsterList.Count; i++)
                {
                    this.checkSingleMonsterState(this.curDeadMonsterList[i]);
                }
                this.curDeadMonsterList.Clear();
            }
        }

        public void updateCurMonsterWaveStateLateTick(Fix64 time)
        {
            foreach (KeyValuePair<int, BattleUnit_Monster> info in this.curMonsterDic)
            {
                info.Value.lateTick(time);
            }
        }

        public void removeSchId()
        {
            if(this.scheId != 0)
            {
                BattleSchedulerComponent sche = (BattleSchedulerComponent)this.baseBattle.getComponent(BattleComponentType.SchedulerComponent);
                sche.silenceSingleSche(this.scheId);
            }
        }

        public bool CheckIsHaveAnyMonsterSurvive()
        {
            if(this.curMonsterDic.Count != 0)
            {
                return true;
            }
            if(this.curNoRegisterList.Count != 0)
            {
                return true;
            }
            return false;
        }

        public bool isCanNewMonsterWaves()
        {
            if(this.battleDataComponent.curWaves >= this.roundInfo.Count)
            {
                return false;
            }
            return true;
        }

        public override void dispose()
        {
            base.dispose();
        }

    }
}
