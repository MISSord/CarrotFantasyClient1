using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETModel
{
    public class UnitEvent
    {
        public const String POSITION_CHANGE = "position_change";
        public const String ROTATION_CHANGE = "rotation_change";
        public const String STATUS_CHANGE = "status_change";
        public const String FACE_DIRECTION_CHANGE = "face_direction_change"; // 怪物脸的方向
        public const String TOWER_DIRECTION_CHANGE = "tower_direction_change"; //炮台的方向
        public const String BODY_RECT_CHANE = "body_rect_change";

        public const String TOWER_UPDATE = "tower_update";

        public const String DAMAGE_CALCULATE_COMPLETE = "damage_calculate_complete";

        public const String KILL_UNIT = "kill_unit";

        public const String HP_CHANGE = "hp_change";
    }
}
