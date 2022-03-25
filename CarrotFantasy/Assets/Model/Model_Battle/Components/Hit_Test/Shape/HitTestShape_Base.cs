using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETModel
{
    public class HitTestShape_Base
    {
        public HitShapeType type;

        public Fix64 boundsX;
        public Fix64 boundsY;
        public Fix64 boundsSizeX;
        public Fix64 boundsSizeY;

        public string strDesc;

        public HitTestShape_Base(HitShapeType hitType)
        {
            type = hitType;
        }

        public virtual void resetStrDesc()
        {
            this.strDesc = String.Format("boundsX:%s boundsZ:%s boundsSizeX:%s boundsSizeZ:%s", this.boundsX, this.boundsY,
                this.boundsSizeX, this.boundsSizeY);
        }
    }
}
