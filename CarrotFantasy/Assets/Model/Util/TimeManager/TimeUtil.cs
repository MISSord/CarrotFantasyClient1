using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ETModel
{
    public class TimeUtil
    {
        public static long getCurTime() //当前时间
        {
            return TimeHelper.ClientNow();
        }

    }
}
