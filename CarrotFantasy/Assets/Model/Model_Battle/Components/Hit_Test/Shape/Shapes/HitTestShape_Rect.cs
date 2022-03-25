using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETModel
{
    public class HitTestShape_Rect : HitTestShape_Base
    {
        public Fix64 x;
        public Fix64 y;
        public Fix64 sizeX;
        public Fix64 sizeY;

        public HitTestShape_Rect(HitShapeType hitType, Fix64 x, Fix64 y, Fix64 sizeX, Fix64 sizeY) : base(hitType)
        {
            this.reset(x, y, sizeX, sizeY);
            this.resetStrDesc();
        }

        private void reset(Fix64 x, Fix64 y, Fix64 sizeX, Fix64 sizeY)
        {
            this.x = x;
            this.y = y;
            this.sizeX = sizeX;
            this.sizeY = sizeY;

            this.boundsX = x;
            this.boundsY = y;
            this.boundsSizeX = sizeX;
            this.boundsSizeY = sizeY;
        }
    }
}
