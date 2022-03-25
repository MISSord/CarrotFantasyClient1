using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETModel
{
    public class Gamer
    {
        public long UserID { get; private set; }
        public bool isReady { get; set; }
        public Gamer(long userId)
        {
            this.UserID = userId;
            this.isReady = false;
        }
    }
}
