using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETModel
{
    public class BattleInputComponent : BaseBattleComponent
    {
        public List<InputOrder> curNoProcessDic = new List<InputOrder>();
        public List<int> shouldRemoveList = new List<int>();

        private BattleTowerComponent towerComponent;
        private BattleMapComponent mapComponent;

        public BattleInputComponent(BaseBattle bBattle) : base(bBattle)
        {
            this.componentType = BattleComponentType.InputComponent;
        }

        public override void init()
        {
            this.towerComponent = (BattleTowerComponent)this.baseBattle.getComponent(BattleComponentType.TowerComponent);
            this.mapComponent = (BattleMapComponent)this.baseBattle.getComponent(BattleComponentType.MapComponent);
        }

        public override void onTick(Fix64 time)
        {
            if (this.curNoProcessDic.Count == 0) return;
            //严格来说一帧只可能会有一个操作
            for (int i = 0; i < this.curNoProcessDic.Count; i++) 
            {
                if(this.curNoProcessDic[i].frameId == this.baseBattle.curFrameId)
                {
                    this.towerComponent.exePlayerOrder(this.curNoProcessDic[i]);
                    this.mapComponent.exePlayerOrder(this.curNoProcessDic[i]);
                    this.shouldRemoveList.Add(i);
                }
                else if(this.curNoProcessDic[i].frameId < this.baseBattle.curFrameId)
                {
                    this.shouldRemoveList.Add(i);
                }
            }
            if(this.shouldRemoveList.Count != 0)
            {
                for(int i = 0; i < this.shouldRemoveList.Count; i++)
                {
                    this.curNoProcessDic.RemoveAt(this.shouldRemoveList[i]);
                }
                this.shouldRemoveList.Clear();
            }
        }

        public void addOrder(InputOrder order)
        {
            this.curNoProcessDic.Add(order);
        }

        public override void clearInfo()
        {
            this.curNoProcessDic.Clear();
            this.shouldRemoveList.Clear();
        }

        public override void dispose()
        {
            this.clearInfo();
            base.dispose();
        }
    }
}
