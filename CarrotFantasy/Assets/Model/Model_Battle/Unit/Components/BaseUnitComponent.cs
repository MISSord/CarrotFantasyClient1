using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETModel
{
    public abstract class BaseUnitComponent
    {
        public BattleUnit unit { get; private set; }
        public String unitComponentType { get; protected set; } //在子类里赋值

        public void loadUnit(BattleUnit unit)
        {
            this.unit = unit;
        }

        public virtual void init(){}

        public virtual void start(){}

        public abstract void onTick(Fix64 deltaTime);

        public virtual void lateTick(Fix64 deltaTime) { }

        public virtual void dispose()
        {
            this.unit = null;
        }
    }
}
