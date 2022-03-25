using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETModel
{
    public class BulletConfigReader
    {
        public Dictionary<int, Dictionary<String, Fix64>> bulletBirthParam = new Dictionary<int, Dictionary<string, Fix64>>();

        public void init()
        {
            this.bulletBirthParam.Add(101, new Dictionary<String, Fix64>() {
                { "faceDirection", Fix64.Zero},
                { "bodyRadius", Fix64.One},
                { "scale", Fix64.One},
                { "speed", Fix64.One},
                { "damage", new Fix64(10)},
                { "moveSpeed", new Fix64(4)},
                { "isRemove", Fix64.Zero},
            });

            this.bulletBirthParam.Add(102, new Dictionary<String, Fix64>() {
                { "faceDirection", Fix64.Zero},
                { "bodyRadius", Fix64.One},
                { "scale", Fix64.One},
                { "speed", Fix64.One},
                { "damage", new Fix64(15)},
                { "moveSpeed", new Fix64(4)},
                { "isRemove", Fix64.Zero},
            });

            this.bulletBirthParam.Add(103, new Dictionary<String, Fix64>() {
                { "faceDirection", Fix64.Zero},
                { "bodyRadius", Fix64.One},
                { "scale", Fix64.One},
                { "speed", Fix64.One},
                { "damage", new Fix64(20)},
                { "moveSpeed", new Fix64(4)},
                { "isRemove", Fix64.Zero},
            });

            this.bulletBirthParam.Add(201, new Dictionary<String, Fix64>() {
                { "faceDirection", Fix64.Zero},
                { "bodyRadius", Fix64.One},
                { "scale", Fix64.One},
                { "speed", Fix64.One},
                { "damage", new Fix64(10)},
                { "moveSpeed", new Fix64(4)},
                { "isRemove", Fix64.Zero},
            });

            this.bulletBirthParam.Add(202, new Dictionary<String, Fix64>() {
                { "faceDirection", Fix64.Zero},
                { "bodyRadius", Fix64.One},
                { "scale", Fix64.One},
                { "speed", Fix64.One},
                { "damage", new Fix64(15)},
                { "moveSpeed", new Fix64(4)},
                { "isRemove", Fix64.Zero},
            });
            this.bulletBirthParam.Add(203, new Dictionary<String, Fix64>() {
                { "faceDirection", Fix64.Zero},
                { "bodyRadius", Fix64.One},
                { "scale", Fix64.One},
                { "speed", Fix64.One},
                { "damage", new Fix64(20)},
                { "moveSpeed", new Fix64(4)},
                { "isRemove", Fix64.Zero},
            });

            this.bulletBirthParam.Add(301, new Dictionary<String, Fix64>() {
                { "faceDirection", Fix64.Zero},
                { "bodyRadius", Fix64.One},
                { "scale", Fix64.One},
                { "speed", Fix64.Two},
                { "damage", new Fix64(10)},
                { "moveSpeed", new Fix64(4)},
                { "isRemove", Fix64.Zero},
            });

            this.bulletBirthParam.Add(302, new Dictionary<String, Fix64>() {
                { "faceDirection", Fix64.Zero},
                { "bodyRadius", Fix64.One},
                { "scale", Fix64.One},
                { "speed", Fix64.Two},
                { "damage", new Fix64(15)},
                { "moveSpeed", new Fix64(4)},
                { "isRemove", Fix64.Zero},
            });

            this.bulletBirthParam.Add(303, new Dictionary<String, Fix64>() {
                { "faceDirection", Fix64.Zero},
                { "bodyRadius", Fix64.One},
                { "scale", Fix64.One},
                { "speed", Fix64.Two},
                { "damage", new Fix64(20)},
                { "moveSpeed", new Fix64(4)},
                { "isRemove", Fix64.Zero},
            });

            this.bulletBirthParam.Add(401, new Dictionary<String, Fix64>() {
                { "faceDirection", Fix64.Zero},
                { "bodyRadius", Fix64.One},
                { "scale", Fix64.One},
                { "speed", Fix64.Two},
                { "damage", new Fix64(10)},
                { "moveSpeed", new Fix64(4)},
                { "isRemove", Fix64.One},
            });

            this.bulletBirthParam.Add(402, new Dictionary<String, Fix64>() {
                { "faceDirection", Fix64.Zero},
                { "bodyRadius", Fix64.One},
                { "scale", Fix64.One},
                { "speed", Fix64.Two},
                { "damage", new Fix64(15)},
                { "moveSpeed", new Fix64(4)},
                { "isRemove", Fix64.One},
            });

            this.bulletBirthParam.Add(403, new Dictionary<String, Fix64>() {
                { "faceDirection", Fix64.Zero},
                { "bodyRadius", Fix64.One},
                { "scale", Fix64.One},
                { "speed", Fix64.Two},
                { "damage", new Fix64(20)},
                { "moveSpeed", new Fix64(4)},
                { "isRemove", Fix64.One},
            });

            this.bulletBirthParam.Add(501, new Dictionary<String, Fix64>() {
                { "faceDirection", Fix64.Zero},
                { "bodyRadius", Fix64.One},
                { "scale", Fix64.One},
                { "speed", Fix64.Two},
                { "damage", new Fix64(10)},
                { "moveSpeed", new Fix64(4)},
                { "isRemove", Fix64.Zero},
            });

            this.bulletBirthParam.Add(502, new Dictionary<String, Fix64>() {
                { "faceDirection", Fix64.Zero},
                { "bodyRadius", Fix64.One},
                { "scale", Fix64.One},
                { "speed", Fix64.Two},
                { "damage", new Fix64(15)},
                { "moveSpeed", new Fix64(4)},
                { "isRemove", Fix64.Zero},
            });

            this.bulletBirthParam.Add(503, new Dictionary<String, Fix64>() {
                { "faceDirection", Fix64.Zero},
                { "bodyRadius", Fix64.One},
                { "scale", Fix64.One},
                { "speed", Fix64.Two},
                { "damage", new Fix64(20)},
                { "moveSpeed", new Fix64(4)},
                { "isRemove", Fix64.Zero},
            });

        }

        public Dictionary<String, Fix64> getSingleBulletConfig(int id)
        {
            return this.bulletBirthParam[id];
        }
    }
}
