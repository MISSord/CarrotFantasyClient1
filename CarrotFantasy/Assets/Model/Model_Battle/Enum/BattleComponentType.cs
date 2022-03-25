using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETModel
{
    public class BattleComponentType
    {
        public const String MapComponent = "MapComponent"; //地图
        public const String DataComponent = "DataComponent"; //信息
        public const String LogComponent = "LogComponent";//
        public const String InputComponent = "InputComponent"; //输入管理 
        public const String RandomComponent = "RandomComponent";//随机数
        //public const String RecordComponent = "RecordComponent"; //成绩

        public const String SchedulerComponent = "SchedulerComponent"; //延迟调用
        public const String HitTestComponent = "HitTestComponent"; //碰撞检测
        public const String JudgeComponent = "JudgeComponent"; //判断游戏过程

        public const String TowerComponent = "TowerComponent";
        public const String MonsterComponent = "MonsterComponent";
        public const String BulletComponent = "BulletComponent";
    }
}
