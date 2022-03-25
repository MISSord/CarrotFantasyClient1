using System;
using System.Collections.Generic;
using ETModel;
using System.Collections;

namespace ETModel
{
    public class EventObject
    {
        int priority = 0;
        String eventName;
        CallBackOne delega;
        public EventObject(String eventName, int priority, CallBackOne d)
        {
            this.priority = priority;
            this.eventName = eventName;
            this.delega = d;
        }

        public int getPriority()
        {
            return this.priority;
        } 

        public String getEventName()
        {
            return this.eventName;
        }

        public CallBackOne getCallBack()
        {
            return this.delega;
        }

        public void dispatcher(Dictionary<String, dynamic> param)
        {
            if (this.delega != null)
            {
                CallBackOne callBack = this.delega;
                if (callBack != null)
                {
                    callBack(param);
                }
                else
                {
                    throw new Exception(string.Format("广播事件错误：事件{0}对应委托为空", this.eventName));
                }
            }
        }
    }
}
