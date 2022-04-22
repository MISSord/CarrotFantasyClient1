using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETModel
{
    public abstract class BaseBattleViewComponent
    {
        public BattleView_base battleView;
        public BaseBattle battle;
        public String componentType { get; protected set; }
        public EventDispatcher eventDispatcher;

        public BaseBattleViewComponent(BattleView_base battleView)
        {
            this.battleView = battleView;
            this.battle = battleView.battle;
            this.eventDispatcher = this.battle.eventDispatcher;
        }

        public abstract void init();
        public virtual void start() { } //开始游戏调用
        public virtual void onTick(float time) { }
        public virtual void clearGameInfo() { } //重新开始游戏前调用
        public virtual void dispose() { }
    }
}
