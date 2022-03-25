using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ETModel
{
    public class HitTestHandler
    {
        //基
        public static bool hitTest(HitTestShape_Base shape1, HitTestShape_Base shape2)
        {
            return false;
        }

        //主要用这个
        //圆形碰撞圆形
        public static bool hitTest(HitTestShape_Circle shape1, HitTestShape_Circle shape2)
        {
            Fix64 distanceSQ = Battle_func.pGetDistanceSQ(shape1.centerX, shape1.centerY, shape2.centerX, shape2.centerY);
            return distanceSQ <= ((shape1.radius + shape2.radius) * (shape1.radius + shape2.radius));
        }

        //圆形碰矩形
        public static bool hitTest(HitTestShape_Circle shapeCircle, HitTestShape_Rect shapeRect)
        {
            Fix64 circleCenterX = shapeCircle.centerX;
            Fix64 circleCenterY = shapeCircle.centerY;
            Fix64 circleRadius = shapeCircle.radius;

            Fix64 rectX = shapeRect.x;
            Fix64 rectY = shapeRect.y;
            Fix64 rectSizeX = shapeRect.sizeX;
            Fix64 rectSizeY = shapeRect.sizeY;

            if (Battle_func.pGetDistanceSQ(circleCenterX, circleCenterY, rectX, rectY) < circleRadius)
            {
                return true;
            }
            else if (Battle_func.pGetDistanceSQ(circleCenterX, circleCenterY, rectX + rectSizeX, rectY) < circleRadius)
            {
                return true;
            }
            else if (Battle_func.pGetDistanceSQ(circleCenterX, circleCenterY, rectX + rectSizeX, rectY + rectSizeY) < circleRadius)
            {
                return true;
            }
            else if (Battle_func.pGetDistanceSQ(circleCenterX, circleCenterY, rectX, rectY + rectSizeY) < circleRadius)
            {
                return true;
            }
            return Battle_func.rectContainsPoint(rectX, rectY, rectSizeX, rectSizeY, circleCenterX, circleCenterY);
        }

        //圆形碰Obb形
        public static bool hitTest(HitTestShape_Circle circle, HitTestShape_ObbRect obbRect)
        {
            return hitTest(obbRect, circle);
        }

        //OBB形碰圆形
        private static Fix64 getCross(Fix64Vector2 p1, Fix64Vector2 p2, Fix64Vector2 p)
        {
            return (p2.X - p1.X) * (p.Y - p1.Y) - (p2.Y - p1.Y) * (p.X - p1.X);
        }

        private static bool isRectContainsPoint(Fix64Vector2 p1, Fix64Vector2 p2, Fix64Vector2 p3, Fix64Vector2 p4, Fix64Vector2 p)
        {
            bool isPointOut = getCross(p1, p2, p) * getCross(p3, p4, p) >= Fix64.Zero || getCross(p1, p3, p) * getCross(p2, p4, p) >= Fix64.Zero;
            return !isPointOut;
        }

        public static bool hitTest(HitTestShape_ObbRect shapeObbRect, HitTestShape_Circle shapeCircle)
        {
            Fix64 circleCenterX = shapeCircle.centerX;
            Fix64 circleCenterY = shapeCircle.centerY;
            Fix64 circleRadius = shapeCircle.radius;

            Fix64Vector2 circleCenter = Battle_func.p(circleCenterX, circleCenterY);
            if (Battle_func.pGetDistanceOfPoint2Line(circleCenter, shapeObbRect.lb, shapeObbRect.rb) < circleRadius)
            {
                return true;
            }
            else if (Battle_func.pGetDistanceOfPoint2Line(circleCenter, shapeObbRect.lb, shapeObbRect.rb) < circleRadius)
            {
                return true;
            }
            else if (Battle_func.pGetDistanceOfPoint2Line(circleCenter, shapeObbRect.lb, shapeObbRect.lt) < circleRadius)
            {
                return true;
            }
            else if (Battle_func.pGetDistanceOfPoint2Line(circleCenter, shapeObbRect.lt, shapeObbRect.rt) < circleRadius)
            {
                return true;
            }
            else if (Battle_func.pGetDistanceOfPoint2Line(circleCenter, shapeObbRect.rb, shapeObbRect.rt) < circleRadius)
            {
                return true;
            }
            else if (isRectContainsPoint(shapeObbRect.lb, shapeObbRect.rb, shapeObbRect.lt, shapeObbRect.rt, circleCenter))
            {
                return true;
            }
            return false;
        }

        //OBB形碰OBB形
        public static bool hitTest(HitTestShape_ObbRect shapeOddRect1, HitTestShape_ObbRect shapeOddRect2)
        {
            Fix64Vector2 nv = Battle_func.p(shapeOddRect1.centerX - shapeOddRect2.centerX, shapeOddRect1.centerY - shapeOddRect2.centerY);
            Fix64Vector2 axisA1 = shapeOddRect1.axes[0];
            if (shapeOddRect1.getProjectionRadius(axisA1) + shapeOddRect2.getProjectionRadius(axisA1) <= Fix64.Abs(Battle_func.pDot(nv, axisA1)))
            {
                return false;
            }
            Fix64Vector2 axisA2 = shapeOddRect1.axes[1];
            if (shapeOddRect1.getProjectionRadius(axisA2) + shapeOddRect2.getProjectionRadius(axisA2) <= Fix64.Abs(Battle_func.pDot(nv, axisA2)))
            {
                return false;
            }
            Fix64Vector2 axisB1 = shapeOddRect2.axes[0];
            if (shapeOddRect1.getProjectionRadius(axisB1) + shapeOddRect2.getProjectionRadius(axisB1) <= Fix64.Abs(Battle_func.pDot(nv, axisB1)))
            {
                return false;
            }
            Fix64Vector2 axisB2 = shapeOddRect2.axes[1];
            if (shapeOddRect1.getProjectionRadius(axisB2) + shapeOddRect2.getProjectionRadius(axisB2) <= Fix64.Abs(Battle_func.pDot(nv, axisB2)))
            {
                return false;
            }
            return true;
        }

        //OBB形碰AABB矩形
        public static bool hitTest(HitTestShape_ObbRect shapeObbRect, HitTestShape_Rect shapeRect)
        {
            HitTestShape_ObbRect shapeObbRect2 = new HitTestShape_ObbRect(HitShapeType.OBB_RECT
                , shapeRect.x + shapeRect.sizeX / Fix64.Two, shapeRect.y + shapeRect.sizeY / Fix64.Two, shapeRect.sizeX, shapeRect.sizeX, Fix64.Zero);
            return hitTest(shapeObbRect, shapeObbRect2);
        }

        //矩形碰圆形
        public static bool hitTest(HitTestShape_Rect shapeRect, HitTestShape_Circle shapeCircle)
        {
            return hitTest(shapeCircle, shapeRect);
        }

        //矩形碰OBB形
        public static bool hitTest(HitTestShape_Rect shapeRect, HitTestShape_ObbRect shapeObbRect)
        {
            return hitTest(shapeObbRect, shapeRect);
        }

        //矩形碰撞矩形
        public static bool hitTest(HitTestShape_Rect shapeRect1, HitTestShape_Rect shapeRect2)
        {
            return Battle_func.rectIntersectsRect(shapeRect1.x, shapeRect1.y, shapeRect1.sizeX,
                shapeRect1.sizeY, shapeRect2.x, shapeRect2.y, shapeRect2.sizeX, shapeRect2.sizeY);
        }

    }
}
