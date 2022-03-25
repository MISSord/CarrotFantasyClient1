using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETModel
{
    public class ConnectEventDispatcher
    {
        private EventDispatcher eventDispatcher;

        public ConnectEventDispatcher()
        {
            init();
        }

        public void init()
        {
            eventDispatcher = new EventDispatcher();
        }

        public EventDispatcher getEventDispatcher()
        {
            return eventDispatcher;
        }

        public void dispatcherConnectEvent(int eventType, Dictionary<String, dynamic> mrg)
        {
            eventDispatcher.dispatchEvent(eventType.ToString(), mrg);
        }

        public void dispose()
        {
            eventDispatcher.dispose();
        }

        public void addConnectListener(int eventType, CallBack callBack)
        {
            eventDispatcher.addListener(eventType.ToString(), callBack);
        }

        public void removeConnectListener(int eventType, CallBack callBack)
        {
            eventDispatcher.removeListener(eventType.ToString(), callBack);
        }
    }
}
