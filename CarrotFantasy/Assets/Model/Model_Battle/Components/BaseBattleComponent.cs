using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETModel
{
    public abstract class BaseBattleComponent
    {
        public BaseBattle baseBattle { get; private set; }
        public String componentType { get; protected set; } //子类赋值

        public EventDispatcher eventDispatcher { get; protected set; }

        public BaseBattleComponent(BaseBattle bBattle)
        {
            this.baseBattle = bBattle;
            this.eventDispatcher = bBattle.eventDispatcher;
        }

        public abstract void init();

        public virtual void start() { } //用于开始游戏(即使是重新开始)

        public virtual void onTick(Fix64 time) { }

        public virtual void lateTick(Fix64 time) { }

        public virtual void clearInfo() { } //用于重新开始游戏

        public virtual void dispose() { }
    }

}
