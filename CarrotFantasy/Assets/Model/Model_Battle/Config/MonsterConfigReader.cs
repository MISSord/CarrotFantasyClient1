using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETModel
{
    public class MonsterConfigReader
    {
        public Dictionary<int, Dictionary<String, Fix64>> monsterBirthParam = new Dictionary<int, Dictionary<string, Fix64>>();

        public void init()
        {
            this.monsterBirthParam.Add(101, new Dictionary<String, Fix64>() { 
                { "faceDirection", Fix64.Zero},
                { "bodyRadius", Fix64.One},
                { "scale", Fix64.One},
                { "offsetX", Fix64.Zero},
                { "offsetY", Fix64.Zero},
                { "speed", Fix64.One},
                { "live", new Fix64(100)},
            });
            this.monsterBirthParam.Add(102, new Dictionary<String, Fix64>() {
                { "faceDirection", Fix64.Zero},
                { "bodyRadius", Fix64.One},
                { "scale", Fix64.One},
                { "offsetX", Fix64.Zero},
                { "offsetY", Fix64.Zero},
                { "speed", Fix64.One},
                { "live", new Fix64(100)},
            });
            this.monsterBirthParam.Add(103, new Dictionary<String, Fix64>() {
                { "faceDirection", Fix64.Zero},
                { "bodyRadius", Fix64.One},
                { "scale", Fix64.One},
                { "offsetX", Fix64.Zero},
                { "offsetY", Fix64.Zero},
                { "speed", Fix64.One},
                { "live", new Fix64(100)},
            });
            this.monsterBirthParam.Add(104, new Dictionary<String, Fix64>() {
                { "faceDirection", Fix64.Zero},
                { "bodyRadius", Fix64.One},
                { "scale", Fix64.One},
                { "offsetX", Fix64.Zero},
                { "offsetY", Fix64.Zero},
                { "speed", Fix64.One},
                { "live", new Fix64(100)},
            });
            this.monsterBirthParam.Add(105, new Dictionary<String, Fix64>() {
                { "faceDirection", Fix64.Zero},
                { "bodyRadius", Fix64.One},
                { "scale", Fix64.One},
                { "offsetX", Fix64.Zero},
                { "offsetY", Fix64.Zero},
                { "speed", Fix64.One},
                { "live", new Fix64(100)},
            });
            this.monsterBirthParam.Add(106, new Dictionary<String, Fix64>() {
                { "faceDirection", Fix64.Zero},
                { "bodyRadius", Fix64.One},
                { "scale", Fix64.One},
                { "offsetX", Fix64.Zero},
                { "offsetY", Fix64.Zero},
                { "speed", Fix64.One},
                { "live", new Fix64(100)},
            });
            this.monsterBirthParam.Add(107, new Dictionary<String, Fix64>() {
                { "faceDirection", Fix64.Zero},
                { "bodyRadius", Fix64.One},
                { "scale", Fix64.One},
                { "offsetX", Fix64.Zero},
                { "offsetY", Fix64.Zero},
                { "speed", Fix64.One},
                { "live", new Fix64(100)},
            });
            this.monsterBirthParam.Add(108, new Dictionary<String, Fix64>() {
                { "faceDirection", Fix64.Zero},
                { "bodyRadius", Fix64.One},
                { "scale", Fix64.One},
                { "offsetX", Fix64.Zero},
                { "offsetY", Fix64.Zero},
                { "speed", Fix64.One},
                { "live", new Fix64(100)},
            });
            this.monsterBirthParam.Add(109, new Dictionary<String, Fix64>() {
                { "faceDirection", Fix64.Zero},
                { "bodyRadius", Fix64.One},
                { "scale", Fix64.One},
                { "offsetX", Fix64.Zero},
                { "offsetY", Fix64.Zero},
                { "speed", Fix64.One},
                { "live", new Fix64(100)},
            });
            this.monsterBirthParam.Add(110, new Dictionary<String, Fix64>() {
                { "faceDirection", Fix64.Zero},
                { "bodyRadius", Fix64.One},
                { "scale", Fix64.One},
                { "offsetX", Fix64.Zero},
                { "offsetY", Fix64.Zero},
                { "speed", Fix64.One},
                { "live", new Fix64(100)},
            });
            this.monsterBirthParam.Add(111, new Dictionary<String, Fix64>() {
                { "faceDirection", Fix64.Zero},
                { "bodyRadius", Fix64.One},
                { "scale", Fix64.One},
                { "offsetX", Fix64.Zero},
                { "offsetY", Fix64.Zero},
                { "speed", Fix64.One},
                { "live", new Fix64(100)},
            });
            this.monsterBirthParam.Add(112, new Dictionary<String, Fix64>() {
                { "faceDirection", Fix64.Zero},
                { "bodyRadius", Fix64.One},
                { "scale", Fix64.One},
                { "offsetX", Fix64.Zero},
                { "offsetY", Fix64.Zero},
                { "speed", Fix64.One},
                { "live", new Fix64(100)},
            });
        }

        public Dictionary<String, Fix64> getSingleMonsterConfig(int id)
        {
            return this.monsterBirthParam[id];
        }
    }
}
