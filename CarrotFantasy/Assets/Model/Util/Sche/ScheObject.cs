using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETModel
{
    

    public class ScheObject
    {
        public int uid;
        public callBack ucallBack;
        public Fix64 lastStartTime;
        public bool isOnce = false; 
        public bool isUnscheduled = false;
        public Fix64 intervalTime;
        public ScheObject(int id, callBack call, float interval, Fix64 curScheTime)
        {
            uid = id;
            ucallBack = call;
            lastStartTime = curScheTime;
            intervalTime = new Fix64(interval);
        }
    }
}
