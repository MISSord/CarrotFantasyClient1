using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ETModel
{
    public delegate void CallBackOne(Dictionary<String, dynamic> param);
    public delegate void CallBack();
    public delegate void CallBack<T>(T arg);
    public delegate void CallBack<T, X>(T arg1, X arg2);
    public delegate void CallBack<T, X, Y>(T arg1, X arg2, Y arg3);
    public delegate void CallBack<T, X, Y, Z>(T arg1, X arg2, Y arg3, Z arg4);
    public delegate void CallBack<T, X, Y, Z, W>(T arg1, X arg2, Y arg3, Z arg4, W arg5);

    public class EventDispatcher //普通事件广播
    {
        private  Dictionary<String, Delegate> eventTable = new Dictionary<String, Delegate>();

        private  void onListenerAdding(String eventType, Delegate callBack)
        {
            if (!eventTable.ContainsKey(eventType))
            {
                eventTable.Add(eventType, null);
            }
            Delegate d = eventTable[eventType];
            if (d != null && d.GetType() != callBack.GetType())
            {
                throw new Exception(string.Format("尝试为事件{0}添加不同类型的委托，当前事件所对应的委托是{1}，要添加的委托类型为{2}", eventType, d.GetType(), callBack.GetType()));
            }
        }
        private bool onListenerRemoving(String eventType, Delegate callBack)
        {
            if (eventTable.ContainsKey(eventType))
            {
                Delegate d = eventTable[eventType];
                if (d == null)
                {
                    throw new Exception(string.Format("移除监听错误：事件{0}没有对应的委托", eventType));
                }
                else if (d.GetType() != callBack.GetType())
                {
                    throw new Exception(string.Format("移除监听错误：尝试为事件{0}移除不同类型的委托，当前委托类型为{1}，要移除的委托类型为{2}", eventType, d.GetType(), callBack.GetType()));
                }
                return true;
            }
            else
            {
                return false;
                //throw new Exception(string.Format("移除监听错误：没有事件码{0}", eventType));
            }
        }
        private  void onListenerRemoved(String eventType)
        {
            if (eventTable[eventType] == null)
            {
                eventTable.Remove(eventType);
            }
        }
        //no parameters
        public  void addListener(String eventType, CallBack callBack)
        {
            this.onListenerAdding(eventType, callBack);
            eventTable[eventType] = (CallBack)eventTable[eventType] + callBack;
        }
        //Single parameters
        public  void addListener<T>(String eventType, CallBack<T> callBack)
        {
            this.onListenerAdding(eventType, callBack);
            eventTable[eventType] = (CallBack<T>)eventTable[eventType] + callBack;
        }
        //two parameters
        public  void addListener<T, X>(String eventType, CallBack<T, X> callBack)
        {
            onListenerAdding(eventType, callBack);
            eventTable[eventType] = (CallBack<T, X>)eventTable[eventType] + callBack;
        }
        //three parameters
        public  void addListener<T, X, Y>(String eventType, CallBack<T, X, Y> callBack)
        {
            onListenerAdding(eventType, callBack);
            eventTable[eventType] = (CallBack<T, X, Y>)eventTable[eventType] + callBack;
        }
        //four parameters
        public  void addListener<T, X, Y, Z>(String eventType, CallBack<T, X, Y, Z> callBack)
        {
            onListenerAdding(eventType, callBack);
            eventTable[eventType] = (CallBack<T, X, Y, Z>)eventTable[eventType] + callBack;
        }
        //five parameters
        public  void addListener<T, X, Y, Z, W>(String eventType, CallBack<T, X, Y, Z, W> callBack)
        {
            onListenerAdding(eventType, callBack);
            eventTable[eventType] = (CallBack<T, X, Y, Z, W>)eventTable[eventType] + callBack;
        }

        //no parameters
        public  void removeListener(String eventType, CallBack callBack)
        {
            if (onListenerRemoving(eventType, callBack))
            {
                eventTable[eventType] = (CallBack)eventTable[eventType] - callBack;
                onListenerRemoved(eventType);
            }
        }
        //single parameters
        public  void removeListener<T>(String eventType, CallBack<T> callBack)
        {
            if(onListenerRemoving(eventType, callBack))
            {
                eventTable[eventType] = (CallBack<T>)eventTable[eventType] - callBack;
                onListenerRemoved(eventType);
            }
        }
        //two parameters
        public  void removeListener<T, X>(String eventType, CallBack<T, X> callBack)
        {
            if (onListenerRemoving(eventType, callBack))
            {
                eventTable[eventType] = (CallBack<T, X>)eventTable[eventType] - callBack;
                onListenerRemoved(eventType);
            }
        }
        //three parameters
        public  void removeListener<T, X, Y>(String eventType, CallBack<T, X, Y> callBack)
        {
            if (onListenerRemoving(eventType, callBack))
            {
                eventTable[eventType] = (CallBack<T, X, Y>)eventTable[eventType] - callBack;
                onListenerRemoved(eventType);
            }
        }
        //four parameters
        public  void removeListener<T, X, Y, Z>(String eventType, CallBack<T, X, Y, Z> callBack)
        {
            if (onListenerRemoving(eventType, callBack))
            {
                eventTable[eventType] = (CallBack<T, X, Y, Z>)eventTable[eventType] - callBack;
                onListenerRemoved(eventType);
            }
        }
        //five parameters
        public  void removeListener<T, X, Y, Z,W>(String eventType, CallBack<T, X, Y, Z, W> callBack)
        {
            if (onListenerRemoving(eventType, callBack))
            {
                eventTable[eventType] = (CallBack<T, X, Y, Z,W>)eventTable[eventType] - callBack;
                onListenerRemoved(eventType);
            }
        }


        //no parameters
        public void dispatchEvent(String eventType)
        {
            Delegate d;
            if (eventTable.TryGetValue(eventType, out d))
            {
                CallBack callBack = d as CallBack;
                if (callBack != null)
                {
                    callBack();
                }
                else
                {
                    throw new Exception(string.Format("广播事件错误：事件{0}对应委托具有不同的类型", eventType));
                }
            }
        }
        //single parameters
        public void dispatchEvent<T>(String eventType, T arg)
        {
            Delegate d;
            if (eventTable.TryGetValue(eventType, out d))
            {
                CallBack<T> callBack = d as CallBack<T>;
                if (callBack != null)
                {
                    callBack(arg);
                }
                else
                {
                    throw new Exception(string.Format("广播事件错误：事件{0}对应委托具有不同的类型", eventType));
                }
            }
        }
        //two parameters
        public void dispatchEvent<T, X>(String eventType, T arg1, X arg2)
        {
            Delegate d;
            if (eventTable.TryGetValue(eventType, out d))
            {
                CallBack<T, X> callBack = d as CallBack<T, X>;
                if (callBack != null)
                {
                    callBack(arg1, arg2);
                }
                else
                {
                    throw new Exception(string.Format("广播事件错误：事件{0}对应委托具有不同的类型", eventType));
                }
            }
        }
        //three parameters
        public void dispatchEvent<T, X, Y>(String eventType, T arg1, X arg2, Y arg3)
        {
            Delegate d;
            if (eventTable.TryGetValue(eventType, out d))
            {
                CallBack<T, X, Y> callBack = d as CallBack<T, X, Y>;
                if (callBack != null)
                {
                    callBack(arg1, arg2, arg3);
                }
                else
                {
                    throw new Exception(string.Format("广播事件错误：事件{0}对应委托具有不同的类型", eventType));
                }
            }
        }
        //four parameters
        public void dispatchEvent<T, X, Y, Z>(String eventType, T arg1, X arg2, Y arg3, Z arg4)
        {
            Delegate d;
            if (eventTable.TryGetValue(eventType, out d))
            {
                CallBack<T, X, Y, Z> callBack = d as CallBack<T, X, Y, Z>;
                if (callBack != null)
                {
                    callBack(arg1, arg2, arg3, arg4);
                }
                else
                {
                    throw new Exception(string.Format("广播事件错误：事件{0}对应委托具有不同的类型", eventType));
                }
            }
        }
        //five parameters
        public void dispatchEvent<T, X, Y, Z, W>(String eventType, T arg1, X arg2, Y arg3, Z arg4, W arg5)
        {
            Delegate d;
            if (eventTable.TryGetValue(eventType, out d))
            {
                CallBack<T, X, Y, Z, W> callBack = d as CallBack<T, X, Y, Z, W>;
                if (callBack != null)
                {
                    callBack(arg1, arg2, arg3, arg4, arg5);
                }
                else
                {
                    throw new Exception(string.Format("广播事件错误：事件{0}对应委托具有不同的类型", eventType));
                }
            }
        }


        public void dispose()
        {
            this.eventTable.Clear();
        }

    }
}




