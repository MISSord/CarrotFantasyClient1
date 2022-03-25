using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ETModel
{

    public class BaseUnitViewComponent
    {
        private BattleUnitView unitView;
        private BattleUnit unit;
        private BattleView_base battleView;
        private BattleEventDispatcher unitEventDispatcher;
        public String componetType { get; protected set; }
        private float x, y, z;

        public BaseUnitViewComponent(BattleUnitView unitView)
        {
            this.rector(unitView);
        }

        public void rector(BattleUnitView unitView)
        {
            this.unitView = unitView;
            this.unit = unitView.unit;
            this.battleView = unitView.battleView;
            this.unitEventDispatcher = this.unit.eventDipatcher;
            this.x = 0;
            this.y = 0;
            this.z = 0;
        }

        public void reset() { }

        public void init() { }

        public void start() { }

        public virtual void setUnitPosition(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public virtual void setUnitScale(float scale) { }

        public virtual void setUnitFaceDirection(float faceDirection) { }

        public virtual void setUnitRotation(float rotation) { }

        public virtual void setUnitBodyRect(float bodyRect) { }

        public virtual void pause() { }

        public virtual void resume() { }

        public virtual void dispose() { }
    }
}
