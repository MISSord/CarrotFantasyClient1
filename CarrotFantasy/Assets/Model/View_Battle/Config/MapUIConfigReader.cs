using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETModel
{
    public class MapUIConfigReader
    {
        public Dictionary<int, Dictionary<String, int>> mapUIParam = new Dictionary<int, Dictionary<string, int>>();

        public void init()
        {
            this.mapUIParam.Add(101, new Dictionary<String, int>() {
                { "mapBg", 0},
                { "mapRoad", 1},
            });
            this.mapUIParam.Add(102, new Dictionary<String, int>() {
                { "mapBg", 0},
                { "mapRoad", 2},
            });
            this.mapUIParam.Add(103, new Dictionary<String, int>() {
                { "mapBg", 1},
                { "mapRoad", 3},
            });
            this.mapUIParam.Add(104, new Dictionary<String, int>() {
                { "mapBg", 1},
                { "mapRoad", 4},
            });
            this.mapUIParam.Add(105, new Dictionary<String, int>() {
                { "mapBg", 0},
                { "mapRoad", 5},
            });
        }

        public Dictionary<String, int> getMapUIConfig(int bigLevel, int level)
        {
            return this.mapUIParam[bigLevel * 100 + level];
        }
    }
}
