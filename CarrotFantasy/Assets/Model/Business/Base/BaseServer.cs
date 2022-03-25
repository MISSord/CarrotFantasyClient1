using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETModel
{
    public abstract class BaseServer
    {
        public bool isFirstLoad = false;

        public BaseServer()
        {
            
        }

        public virtual void init() //初始化
        {
            
        }
        public virtual void loadModule() 
        {
            isFirstLoad = true;
        }
        public virtual void reloadModule() 
        {
            isFirstLoad = false;
        }
        public virtual void dispose() { }

        public virtual void addSocketListener() { }

        public virtual void removeSocketListener() { }
    }
}
