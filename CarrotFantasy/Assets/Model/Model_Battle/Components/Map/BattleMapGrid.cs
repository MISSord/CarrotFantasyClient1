using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ETModel
{
    public class BattleMapGrid
    {
        public int uid;

        public int x { get; private set; }
        public int y { get; private set; }

        public Fix64 realX { get; private set; }
        public Fix64 realY { get; private set; }

        public struct GridState
        {
            public bool canBuild;
            public bool isMonsterPoint;
            public bool hasItem;
            public int itemID;
        }

        public GridState state { get; private set; }

        public struct GridIndex //地图中的二维坐标（非实际坐标）
        {
            public int xIndex;
            public int yIndex;
        }

        public bool hasTower { get; private set; }

        public BattleMapGrid(int id, int x, int y)
        {
            this.uid = id;
            this.x = x;
            this.y = y;

            this.realX = new Fix64(this.x * BattleConfig.MAP_RATIO);
            this.realY = new Fix64(this.y * BattleConfig.MAP_RATIO);
        }

        public void loadGridInfo(GridState state)
        {
            this.state = state;
        }

        public void changeTowerState(bool isHave)
        {
            this.hasTower = isHave;
        }


    }
}
