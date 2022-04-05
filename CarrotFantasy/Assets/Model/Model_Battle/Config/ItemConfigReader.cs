using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETModel
{
    public class ItemConfigReader
    {
        public Dictionary<int, Dictionary<String, Fix64>> itemBirthParam = new Dictionary<int, Dictionary<string, Fix64>>();

        public void init()
        {
            this.itemBirthParam.Add(101, new Dictionary<String, Fix64>() {
                { "faceDirection", Fix64.Zero},
                { "bodyRadius", Fix64.One},
                { "scale", Fix64.One},
                { "offsetX", Fix64.Zero},
                { "offsetY", Fix64.Zero},
                { "speed", Fix64.One},
                { "live", new Fix64(100)},
            });
            this.itemBirthParam.Add(102, new Dictionary<String, Fix64>() {
                { "faceDirection", Fix64.Zero},
                { "bodyRadius", Fix64.One},
                { "scale", Fix64.One},
                { "offsetX", Fix64.Zero},
                { "offsetY", Fix64.Zero},
                { "speed", Fix64.One},
                { "live", new Fix64(100)},
            });
            this.itemBirthParam.Add(103, new Dictionary<String, Fix64>() {
                { "faceDirection", Fix64.Zero},
                { "bodyRadius", Fix64.One},
                { "scale", Fix64.One},
                { "offsetX", Fix64.Zero},
                { "offsetY", Fix64.Zero},
                { "speed", Fix64.One},
                { "live", new Fix64(100)},
            });
            this.itemBirthParam.Add(104, new Dictionary<String, Fix64>() {
                { "faceDirection", Fix64.Zero},
                { "bodyRadius", Fix64.One},
                { "scale", Fix64.One},
                { "offsetX", Fix64.Zero},
                { "offsetY", Fix64.Zero},
                { "speed", Fix64.One},
                { "live", new Fix64(100)},
            });
            this.itemBirthParam.Add(105, new Dictionary<String, Fix64>() {
                { "faceDirection", Fix64.Zero},
                { "bodyRadius", Fix64.One},
                { "scale", Fix64.One},
                { "offsetX", Fix64.Zero},
                { "offsetY", Fix64.Zero},
                { "speed", Fix64.One},
                { "live", new Fix64(100)},
            });
            this.itemBirthParam.Add(105, new Dictionary<String, Fix64>() {
                { "faceDirection", Fix64.Zero},
                { "bodyRadius", Fix64.One},
                { "scale", Fix64.One},
                { "offsetX", Fix64.Zero},
                { "offsetY", Fix64.Zero},
                { "speed", Fix64.One},
                { "live", new Fix64(100)},
            });
        }

        public Dictionary<String, Fix64> getSingleItemConfig(int id)
        {
            return this.itemBirthParam[id];
        }
    }
}
