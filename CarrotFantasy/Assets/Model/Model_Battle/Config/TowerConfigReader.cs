using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETModel
{
    public class TowerConfigReader
    {
        public Dictionary<int, Dictionary<String, Fix64>> towerBirthParam = new Dictionary<int, Dictionary<string, Fix64>>();

        public void init()
        {
            this.towerBirthParam.Add(1, new Dictionary<String, Fix64>() {
                { "towerID", Fix64.One},
                { "price0", new Fix64(100)},
                { "price1", new Fix64(120)},
                { "price2", new Fix64(140)},
                { "attackCD", Fix64.One},
                { "bodyRadius0", Fix64.Two},
                { "bodyRadius1", new Fix64(2.5f)},
                { "bodyRadius2", new Fix64(3)},
                { "scale", Fix64.One},
                { "faceDirection", Fix64.Zero},
            });

            this.towerBirthParam.Add(2, new Dictionary<String, Fix64>() {
                { "towerID", Fix64.Two},
                { "price0", new Fix64(120)},
                { "price1", new Fix64(140)},
                { "price2", new Fix64(160)},
                { "attackCD", Fix64.One},
                { "bodyRadius0", Fix64.Two},
                { "bodyRadius1", new Fix64(2.5f)},
                { "bodyRadius2", new Fix64(3)},
                { "scale", Fix64.One},
                { "buildTowerPirce", new Fix64(120)},
                { "faceDirection", Fix64.Zero},
            });

            this.towerBirthParam.Add(3, new Dictionary<String, Fix64>() {
                { "towerID", new Fix64(3)},
                { "price0", new Fix64(160)},
                { "price1", new Fix64(180)},
                { "price2", new Fix64(200)},
                { "attackCD", Fix64.One},
                { "bodyRadius0", Fix64.Two},
                { "bodyRadius1", new Fix64(2.5f)},
                { "bodyRadius2", new Fix64(3)},
                { "scale", Fix64.One},
                { "faceDirection", Fix64.Zero},
            });

            this.towerBirthParam.Add(4, new Dictionary<String, Fix64>() {
                { "towerID", new Fix64(4)},
                { "price0", new Fix64(160)},
                { "price1", new Fix64(180)},
                { "price2", new Fix64(200)},
                { "attackCD", Fix64.One},
                { "bodyRadius0", Fix64.Two},
                { "bodyRadius1", new Fix64(2.5f)},
                { "bodyRadius2", new Fix64(3)},
                { "scale", Fix64.One},
                { "faceDirection", Fix64.Zero},
            });

            this.towerBirthParam.Add(5, new Dictionary<String, Fix64>() {
                { "towerID", new Fix64(5)},
                { "price0", new Fix64(160)},
                { "price1", new Fix64(180)},
                { "price2", new Fix64(200)},
                { "attackCD", Fix64.One},
                { "bodyRadius0", Fix64.Two},
                { "bodyRadius1", new Fix64(2.5f)},
                { "bodyRadius2", new Fix64(3)},
                { "scale", Fix64.One},
                { "faceDirection", Fix64.Zero},
            });

        }

        public Dictionary<String, Fix64> getSingleTowerConfig(int id)
        {
            return this.towerBirthParam[id];
        }
    }
}
