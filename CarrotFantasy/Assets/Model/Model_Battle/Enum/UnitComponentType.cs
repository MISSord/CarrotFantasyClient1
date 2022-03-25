using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETModel
{
    public class UnitComponentType
    {
        public const String TRANSFORM = "Transform"; //坐标
        public const String MOVE = "Move"; //移动
        public const String ACTION = "Action";
        public const String BEHIT = "Behit"; //碰撞

        public const String SKILL = "Skill";
        public const String TRAIN = "Train"; //传输

        public const String STATUS = "STATUS";//状态

        public const String MOVE_MONSTER = "Move_Monster";
        public const String MOVE_BULLET = "Move_Bullet";
    }
}
