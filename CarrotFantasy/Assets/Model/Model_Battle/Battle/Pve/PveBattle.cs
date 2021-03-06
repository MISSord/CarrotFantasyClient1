using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETModel
{
    public class PveBattle : BaseBattle
    {
        public PveBattle() : base()
        {
            
        }

        public override void init()
        {
            this.stateMachine = new PveStateMachine(this);
            this.addComponent(new BattleDataComponent(this));
            this.addComponent(new BattleSimpleHitTestComponent(this));
            this.addComponent(new BattleMapComponent(this)); //依赖 data
            this.addComponent(new BattleItemComponent(this)); //依赖map
            this.addComponent(new BattleTowerComponent(this)); //依赖map data
            this.addComponent(new BattleMonsterComponent(this)); //依赖map
            this.addComponent(new BattleBulletComponent(this)); //依赖tower
            this.addComponent(new BattleInputComponent(this)); //依赖map tower
            this.addComponent(new BattleSchedulerComponent(this));

            this.addListener();
        }

        protected override void addListener()
        {
            this.eventDispatcher.addListener(BattleEvent.PAUSE_THE_GAME, this.pauseTheGame);
            this.eventDispatcher.addListener(BattleEvent.GO_ON_GAME, this.goOnTheGame);
        }

        protected override void removeListener()
        {
            this.eventDispatcher.removeListener(BattleEvent.PAUSE_THE_GAME, this.pauseTheGame);
            this.eventDispatcher.removeListener(BattleEvent.GO_ON_GAME, this.goOnTheGame);
        }

        public override void clearGameInfo()
        {
            base.clearGameInfo();
            this.removeListener();
        }

        public override void initComponent()
        {
            foreach(KeyValuePair<String, BaseBattleComponent> info in this.componentDic)
            {
                info.Value.init();
            }
        }

        public override void dispose()
        {
            base.dispose();
        }
    }
}
