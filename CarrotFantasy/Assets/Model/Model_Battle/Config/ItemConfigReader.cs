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
            this.itemBirthParam.Add(100, new Dictionary<String, Fix64>() {
                { "faceDirection", Fix64.Zero},
                { "bodyRadius", new Fix64(0.5f)},
                { "scale", Fix64.One},
                { "offsetX", Fix64.Zero},
                { "offsetY", Fix64.Zero},
                { "live", new Fix64(300)},
                { "money", new Fix64(100)},
            });
            this.itemBirthParam.Add(101, new Dictionary<String, Fix64>() {
                { "faceDirection", Fix64.Zero},
                { "bodyRadius", new Fix64(0.5f)},
                { "scale", Fix64.One},
                { "offsetX", Fix64.Zero},
                { "offsetY", Fix64.Zero},
                { "live", new Fix64(300)},
                { "money", new Fix64(100)},
            });
            this.itemBirthParam.Add(102, new Dictionary<String, Fix64>() {
                { "faceDirection", Fix64.Zero},
                { "bodyRadius", new Fix64(0.6f)},
                { "scale", Fix64.One},
                { "offsetX", Fix64.Zero},
                { "offsetY", Fix64.Zero},
                { "live", new Fix64(300)},
                { "money", new Fix64(100)},
            });
            this.itemBirthParam.Add(103, new Dictionary<String, Fix64>() {
                { "faceDirection", Fix64.Zero},
                { "bodyRadius", new Fix64(0.5f)},
                { "scale", Fix64.One},
                { "offsetX", Fix64.Zero},
                { "offsetY", Fix64.Zero},
                { "live", new Fix64(100)},
                { "money", new Fix64(30)},
            });
            this.itemBirthParam.Add(104, new Dictionary<String, Fix64>() {
                { "faceDirection", Fix64.Zero},
                { "bodyRadius", new Fix64(0.5f)},
                { "scale", Fix64.One},
                { "offsetX", Fix64.Zero},
                { "offsetY", Fix64.Zero},
                { "live", new Fix64(200)},
                { "money", new Fix64(50)},
            });
            this.itemBirthParam.Add(105, new Dictionary<String, Fix64>() {
                { "faceDirection", Fix64.Zero},
                { "bodyRadius", new Fix64(0.5f)},
                { "scale", Fix64.One},
                { "offsetX", Fix64.Zero},
                { "offsetY", Fix64.Zero},
                { "live", new Fix64(300)},
                { "money", new Fix64(100)},
            });
            this.itemBirthParam.Add(106, new Dictionary<String, Fix64>() {
                { "faceDirection", Fix64.Zero},
                { "bodyRadius", new Fix64(0.3f)},
                { "scale", Fix64.One},
                { "offsetX", Fix64.Zero},
                { "offsetY", Fix64.Zero},
                { "live", new Fix64(100)},
                { "money", new Fix64(30)},
            });
            this.itemBirthParam.Add(107, new Dictionary<String, Fix64>() {
                { "faceDirection", Fix64.Zero},
                { "bodyRadius", new Fix64(0.3f)},
                { "scale", Fix64.One},
                { "offsetX", Fix64.Zero},
                { "offsetY", Fix64.Zero},
                { "live", new Fix64(100)},
                { "money", new Fix64(30)},
            });
            this.itemBirthParam.Add(108, new Dictionary<String, Fix64>() {
                { "faceDirection", Fix64.Zero},
                { "bodyRadius", new Fix64(0.5f)},
                { "scale", Fix64.One},
                { "offsetX", Fix64.Zero},
                { "offsetY", Fix64.Zero},
                { "live", new Fix64(300)},
                { "money", new Fix64(100)},
            });
            this.itemBirthParam.Add(109, new Dictionary<String, Fix64>() {
                { "faceDirection", Fix64.Zero},
                { "bodyRadius", new Fix64(0.5f)},
                { "scale", Fix64.One},
                { "offsetX", Fix64.Zero},
                { "offsetY", Fix64.Zero},
                { "live", new Fix64(150)},
                { "money", new Fix64(35)},
            });
        }

        public Dictionary<String, Fix64> getSingleItemConfig(int id)
        {
            return this.itemBirthParam[id];
        }
    }
}
