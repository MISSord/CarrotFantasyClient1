using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ETModel
{
    public class HitTestShape_ObbRect:HitTestShape_Base
    {
        public Fix64 centerX;
        public Fix64 centerY;
        public Fix64 sizeX;
        public Fix64 sizeY;

        public Fix64 rotation;
        private Fix64 halfEdgeX;
        private Fix64 halfEdgeY;
        private Fix64 radian;

        public Fix64Vector2 lt;
        public Fix64Vector2 rt;
        public Fix64Vector2 lb;
        public Fix64Vector2 rb;

        public Fix64Vector2[] axes = new Fix64Vector2[2];
        public HitTestShape_ObbRect(HitShapeType type, Fix64 centerX, Fix64 centerY, Fix64 sizeX, Fix64 sizeY, Fix64 rotation) : base(type)
        {
            this.reset(centerX, centerY, sizeX, sizeY, rotation);
            this.resetStrDesc();
        }

        private void reset(Fix64 centerX, Fix64 centerY, Fix64 sizeX, Fix64 sizeY, Fix64 rotation)
        {
            this.centerX = centerX;
            this.centerY = centerY;
            this.sizeX = sizeX;
            this.sizeY = sizeY;

            this.halfEdgeX = this.sizeX / Fix64.Two;
            this.halfEdgeY = this.sizeY / Fix64.Two;

            this.rotation = rotation;
            this.radian = Battle_func.angle2radian(this.rotation);

            this.axes[1] = Battle_func.p(Fix64.Cos(this.radian), Fix64.Sin(this.radian));
            this.axes[2] = Battle_func.p(-Fix64.One * Fix64.Sin(this.radian), Fix64.Cos(this.radian));

            this.lt = this.getGlobalPosition(-this.halfEdgeX, this.halfEdgeY);
            this.rt = this.getGlobalPosition(this.halfEdgeX, this.halfEdgeY);
            this.lb = this.getGlobalPosition(-this.halfEdgeX, -this.halfEdgeY);
            this.rb = this.getGlobalPosition(this.halfEdgeX, -this.halfEdgeY);

            Fix64 minX = Battle_func.min(lt.X, rt.X, lb.X, rb.X);
            Fix64 maxX = Battle_func.max(lt.X, rt.X, lb.X, rb.X);
            Fix64 minY = Battle_func.min(lt.Y, rt.Y, lb.Y, rb.Y);
            Fix64 maxY = Battle_func.max(lt.Y, rt.Y, lb.Y, rb.Y);

            this.boundsX = minX;
            this.boundsY = minY;
            this.boundsSizeX = maxX - minX;
            this.boundsSizeY = maxY - minY;
        }

        public Fix64Vector2 getGlobalPosition(Fix64 localX, Fix64 localY)
        {
            Fix64 X = (Fix64)(this.centerX + localX * Fix64.Cos(this.radian) + localY * Fix64.Sin(this.radian));
            Fix64 Y = (Fix64)(this.centerY - localX * Fix64.Sin(this.radian) + localY * Fix64.Cos(this.radian));
            return new Fix64Vector2(X, Y);
        }

        public Fix64 getProjectionRadius(Fix64Vector2 otherAxis)
        {
            return this.halfEdgeX * Fix64.Abs(Battle_func.pDot(otherAxis, this.axes[0])) + this.halfEdgeY * Fix64.Abs(Battle_func.pDot(otherAxis, this.axes[1]));
        }
    }
}
