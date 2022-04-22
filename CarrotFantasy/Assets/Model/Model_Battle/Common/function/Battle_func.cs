using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ETModel
{
    public class Battle_func
    {
        public static Fix64Vector2 p(Fix64 x, Fix64 y)
        {
            return new Fix64Vector2(x, y);
        }

        public static Fix64Vector2 pAdd(Fix64Vector2 one,Fix64Vector2 two)
        {
            return one + two;
        }

        public static Fix64Vector2 pSub(Fix64Vector2 one, Fix64Vector2 two)
        {
            return one - two;
        }

        public static Fix64Vector2 pMul(Fix64Vector2 one, Fix64Vector2 two)
        {
            return new Fix64Vector2(one.X * two.X, one.Y * two.Y);
        }

        public static Fix64 pGetLength(Fix64Vector2 pt)
        {
            return Fix64.Sqrt(pt.X * pt.X + pt.Y * pt.Y); //保留修改意见
        }

        public static Fix64 pDistanceSqure(Fix64Vector2 pt1, Fix64Vector2 pt2)
        {
            return (pt1.X - pt2.X) * (pt1.X - pt2.X) + (pt1.Y - pt2.Y) * (pt1.Y - pt2.Y);
        }

        public static Fix64 pi = Fix64.Pi;
        public static Fix64 pi_div_180 = Fix64.DegreeToRad;

        public static Fix64 angle2radian(Fix64 angle)
        {
            return angle * pi_div_180; //未来定点数
        }

        public static Fix64 p180_div_pi = new Fix64(180) / Fix64.Pi;
        public static Fix64 radian2angle(Fix64 radian)
        {
            return radian * p180_div_pi;//未来定点数
        }

        public static Fix64 min(Fix64 min1, Fix64 min2, Fix64 min3, Fix64 min4)
        {
            Fix64 min = min1;
            if(min > min2)
            {
                min = min2;
            }
            if(min > min3)
            {
                min = min3;
            }
            if(min > min4)
            {
                min = min4;
            }
            return min;
        }

        public static Fix64 max(Fix64 min1, Fix64 min2, Fix64 min3, Fix64 min4)
        {
            Fix64 min = min1;
            if (min < min2)
            {
                min = min2;
            }
            if (min < min3)
            {
                min = min3;
            }
            if (min < min4)
            {
                min = min4;
            }
            return min;
        }

        public static Fix64 pDot(Fix64Vector2 one, Fix64Vector2 two)
        {
            return one.X * two.X + one.Y * two.Y;
        }

        public static Fix64 pLengthSQ(Fix64Vector2 pt)
        {
            return pDot(pt, pt);
        }

        public static Fix64 pGetDistanceSQ(Fix64 pointX1, Fix64 pointY1, Fix64 pointX2, Fix64 pointY2)
        {
            Fix64 offsetX = pointX1 - pointX2;
            Fix64 offsetY = pointY1 - pointY2;
            Fix64 distanceSQ = offsetX * offsetX + offsetY * offsetY;

            return distanceSQ; //未来定点数
        }

        public static Fix64 pGetDistanceOfPoint2Line(Fix64Vector2 p, Fix64Vector2 plineA, Fix64Vector2 plineB)
        {
            Fix64Vector2 u = pSub(plineA, plineB);
            Fix64 lengthSQ = pLengthSQ(u);
            lengthSQ = lengthSQ != Fix64.Zero ? lengthSQ : Fix64.One;
            Fix64 t = pDot(pSub(p, plineA), u) / lengthSQ;
            t = Fix64.Max(Fix64.Min(t, Fix64.One), Fix64.Zero);

            Fix64 d = pGetLength(pSub(p, pAdd(plineA, new Fix64Vector2(t * u.X, t * u.Y))));
            return d;
        }

        public static bool rectContainsPoint(Fix64 rectX, Fix64 rectY, Fix64 rectWidth, Fix64 rectHeight, Fix64 pointX, Fix64 pointY)
        {
            bool ret = false;
            if((pointX >= rectX) && (pointX <= rectX + rectWidth) 
                && (pointY >= rectY) && (pointY <= rectY + rectHeight))
            {
                ret = true;
            }
            return ret;
        }

        public static bool rectIntersectsRect(Fix64 x1, Fix64 y1, Fix64 width1, Fix64 height1, 
            Fix64 x2, Fix64 y2, Fix64 width2, Fix64 height2)
        {
            //未来定点数
            bool intersect = !((x1 > x2 + width2) || (x1 + width1 < x2) || (y1 > y2 + height2)
                || (y1 + height1 < y2));
            return intersect;
        }
    }
}
