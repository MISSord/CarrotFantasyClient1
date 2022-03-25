using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETModel
{
    public class LevelInfo
    {
        public int bigLevelID;
        public int levelID;

        public List<BattleMapGrid.GridState> gridPoints;

        public List<BattleMapGrid.GridIndex> monsterPath;

        public List<Round.RoundInfo> roundInfo;
    }
}
