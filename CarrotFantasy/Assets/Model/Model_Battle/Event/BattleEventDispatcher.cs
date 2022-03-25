using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETModel
{
    public class BattleEventDispatcher //战斗模块的事件广播
    {
        private Dictionary<String, Delegate> event2ListenerList = new Dictionary<string, Delegate>();
        private Dictionary<String, Delegate> event2ViewListenerList = new Dictionary<string, Delegate>();

        private BaseBattle battle;
        private BattleUnit unit;

        public BattleEventDispatcher(BattleUnit unit, BaseBattle battle)
        {
            this.unit = unit;
            this.battle = battle;
        }

        private  void OnListeneradding(String eventType, Delegate callBack, Dictionary<String, Delegate> eventList)
        {
            if (!eventList.ContainsKey(eventType))
            {
                eventList.Add(eventType, null);
            }
            Delegate d = eventList[eventType];
            if (d != null && d.GetType() != callBack.GetType())
            {
                throw new Exception(string.Format("尝试为事件{0}添加不同类型的委托，当前事件所对应的委托是{1}，要添加的委托类型为{2}", eventType, d.GetType(), callBack.GetType()));
            }
        }
        private  void OnListenerRemoving(String eventType, Delegate callBack, Dictionary<String, Delegate> eventList)
        {
            if (eventList.ContainsKey(eventType))
            {
                Delegate d = eventList[eventType];
                if (d == null)
                {
                    throw new Exception(string.Format("移除监听错误：事件{0}没有对应的委托", eventType));
                }
                else if (d.GetType() != callBack.GetType())
                {
                    throw new Exception(string.Format("移除监听错误：尝试为事件{0}移除不同类型的委托，当前委托类型为{1}，要移除的委托类型为{2}", eventType, d.GetType(), callBack.GetType()));
                }
            }
            else
            {
                throw new Exception(string.Format("移除监听错误：没有事件码{0}", eventType));
            }
        }
        private  void OnListenerremoved(String eventType, Dictionary<String, Delegate> eventList)
        {
            if (eventList[eventType] == null)
            {
                eventList.Remove(eventType);
            }
        }
        //no parameters
        public  void addListener(String eventType, CallBack callBack)
        {
            OnListeneradding(eventType, callBack,this.event2ListenerList);
            this.event2ListenerList[eventType] = (CallBack)this.event2ListenerList[eventType] + callBack;
        }
        //Single parameters
        public  void addListener<T>(String eventType, CallBack<T> callBack)
        {
            OnListeneradding(eventType, callBack, this.event2ListenerList);
            this.event2ListenerList[eventType] = (CallBack<T>)this.event2ListenerList[eventType] + callBack;
        }
        //two parameters
        public  void addListener<T, X>(String eventType, CallBack<T, X> callBack)
        {
            OnListeneradding(eventType, callBack, this.event2ListenerList);
            this.event2ListenerList[eventType] = (CallBack<T, X>)this.event2ListenerList[eventType] + callBack;
        }
        //three parameters
        public  void addListener<T, X, Y>(String eventType, CallBack<T, X, Y> callBack)
        {
            OnListeneradding(eventType, callBack, this.event2ListenerList);
            this.event2ListenerList[eventType] = (CallBack<T, X, Y>)this.event2ListenerList[eventType] + callBack;
        }
        //four parameters
        public  void addListener<T, X, Y, Z>(String eventType, CallBack<T, X, Y, Z> callBack)
        {
            OnListeneradding(eventType, callBack, this.event2ListenerList);
            this.event2ListenerList[eventType] = (CallBack<T, X, Y, Z>)this.event2ListenerList[eventType] + callBack;
        }
        //five parameters
        public  void addListener<T, X, Y, Z, W>(String eventType, CallBack<T, X, Y, Z, W> callBack)
        {
            OnListeneradding(eventType, callBack, this.event2ListenerList);
            this.event2ListenerList[eventType] = (CallBack<T, X, Y, Z, W>)this.event2ListenerList[eventType] + callBack;
        }

        public void addListenerForView(String eventType, CallBack callBack)
        {
            OnListeneradding(eventType, callBack, this.event2ViewListenerList);
            this.event2ViewListenerList[eventType] = (CallBack)this.event2ViewListenerList[eventType] + callBack;
        }
        //Single parameters
        public void addListenerForView<T>(String eventType, CallBack<T> callBack)
        {
            OnListeneradding(eventType, callBack, this.event2ViewListenerList);
            this.event2ViewListenerList[eventType] = (CallBack<T>)this.event2ViewListenerList[eventType] + callBack;
        }
        //two parameters
        public void addListenerForView<T, X>(String eventType, CallBack<T, X> callBack)
        {
            OnListeneradding(eventType, callBack, this.event2ViewListenerList);
            this.event2ViewListenerList[eventType] = (CallBack<T, X>)this.event2ViewListenerList[eventType] + callBack;
        }
        //three parameters
        public void addListenerForView<T, X, Y>(String eventType, CallBack<T, X, Y> callBack)
        {
            OnListeneradding(eventType, callBack, this.event2ViewListenerList);
            this.event2ViewListenerList[eventType] = (CallBack<T, X, Y>)this.event2ViewListenerList[eventType] + callBack;
        }
        //four parameters
        public void addListenerForView<T, X, Y, Z>(String eventType, CallBack<T, X, Y, Z> callBack)
        {
            OnListeneradding(eventType, callBack, this.event2ViewListenerList);
            this.event2ViewListenerList[eventType] = (CallBack<T, X, Y, Z>)this.event2ViewListenerList[eventType] + callBack;
        }
        //five parameters
        public void addListenerForView<T, X, Y, Z, W>(String eventType, CallBack<T, X, Y, Z, W> callBack)
        {
            OnListeneradding(eventType, callBack, this.event2ViewListenerList);
            this.event2ViewListenerList[eventType] = (CallBack<T, X, Y, Z, W>)this.event2ViewListenerList[eventType] + callBack;
        }


        //no parameters
        public  void removeListener(String eventType, CallBack callBack)
        {
            OnListenerRemoving(eventType, callBack,this.event2ListenerList);
            this.event2ListenerList[eventType] = (CallBack)this.event2ListenerList[eventType] - callBack;
            OnListenerremoved(eventType, this.event2ListenerList);
        }
        //single parameters
        public  void removeListener<T>(String eventType, CallBack<T> callBack)
        {
            OnListenerRemoving(eventType, callBack,this.event2ListenerList);
            this.event2ListenerList[eventType] = (CallBack<T>)this.event2ListenerList[eventType] - callBack;
            OnListenerremoved(eventType, this.event2ListenerList);
        }
        //two parameters
        public  void removeListener<T, X>(String eventType, CallBack<T, X> callBack)
        {
            OnListenerRemoving(eventType, callBack,this.event2ListenerList);
            this.event2ListenerList[eventType] = (CallBack<T, X>)this.event2ListenerList[eventType] - callBack;
            OnListenerremoved(eventType, this.event2ListenerList);
        }
        //three parameters
        public  void removeListener<T, X, Y>(String eventType, CallBack<T, X, Y> callBack)
        {
            OnListenerRemoving(eventType, callBack,this.event2ListenerList);
            this.event2ListenerList[eventType] = (CallBack<T, X, Y>)this.event2ListenerList[eventType] - callBack;
            OnListenerremoved(eventType, this.event2ListenerList);
        }
        //four parameters
        public  void removeListener<T, X, Y, Z>(String eventType, CallBack<T, X, Y, Z> callBack)
        {
            OnListenerRemoving(eventType, callBack,this.event2ListenerList);
            this.event2ListenerList[eventType] = (CallBack<T, X, Y, Z>)this.event2ListenerList[eventType] - callBack;
            OnListenerremoved(eventType, this.event2ListenerList);
        }
        //five parameters
        public  void removeListener<T, X, Y, Z, W>(String eventType, CallBack<T, X, Y, Z, W> callBack)
        {
            OnListenerRemoving(eventType, callBack,this.event2ListenerList);
            this.event2ListenerList[eventType] = (CallBack<T, X, Y, Z, W>)this.event2ListenerList[eventType] - callBack;
            OnListenerremoved(eventType, this.event2ListenerList);
        }

        public void removeListenerForView(String eventType, CallBack callBack)
        {
            OnListenerRemoving(eventType, callBack, this.event2ViewListenerList);
            this.event2ViewListenerList[eventType] = (CallBack)this.event2ViewListenerList[eventType] - callBack;
            OnListenerremoved(eventType, this.event2ListenerList);
        }
        //single parameters
        public void removeListenerForView<T>(String eventType, CallBack<T> callBack)
        {
            OnListenerRemoving(eventType, callBack, this.event2ViewListenerList);
            this.event2ViewListenerList[eventType] = (CallBack<T>)this.event2ViewListenerList[eventType] - callBack;
            OnListenerremoved(eventType, this.event2ViewListenerList);
        }
        //two parameters
        public void removeListenerForView<T, X>(String eventType, CallBack<T, X> callBack)
        {
            OnListenerRemoving(eventType, callBack, this.event2ViewListenerList);
            this.event2ViewListenerList[eventType] = (CallBack<T, X>)this.event2ViewListenerList[eventType] - callBack;
            OnListenerremoved(eventType, this.event2ViewListenerList);
        }
        //three parameters
        public void removeListenerForView<T, X, Y>(String eventType, CallBack<T, X, Y> callBack)
        {
            OnListenerRemoving(eventType, callBack, this.event2ViewListenerList);
            this.event2ViewListenerList[eventType] = (CallBack<T, X, Y>)this.event2ViewListenerList[eventType] - callBack;
            OnListenerremoved(eventType, this.event2ViewListenerList);
        }
        //four parameters
        public void removeListenerForView<T, X, Y, Z>(String eventType, CallBack<T, X, Y, Z> callBack)
        {
            OnListenerRemoving(eventType, callBack, this.event2ViewListenerList);
            this.event2ViewListenerList[eventType] = (CallBack<T, X, Y, Z>)this.event2ViewListenerList[eventType] - callBack;
            OnListenerremoved(eventType, this.event2ViewListenerList);
        }
        //five parameters
        public void removeListenerForView<T, X, Y, Z, W>(String eventType, CallBack<T, X, Y, Z, W> callBack)
        {
            OnListenerRemoving(eventType, callBack, this.event2ViewListenerList);
            this.event2ViewListenerList[eventType] = (CallBack<T, X, Y, Z, W>)this.event2ViewListenerList[eventType] - callBack;
            OnListenerremoved(eventType, this.event2ViewListenerList);
        }


        //no parameters
        public  void dispatchEvent(String eventType)
        {
            Delegate d;
            if (this.event2ListenerList.TryGetValue(eventType, out d))
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
            if(this.battle.isIgnoreViewListener == false)
            {
                if (this.event2ViewListenerList.TryGetValue(eventType, out d))
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
        }
        //single parameters
        public  void dispatchEvent<T>(String eventType, T arg)
        {
            Delegate d;
            if (this.event2ListenerList.TryGetValue(eventType, out d))
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
            if (this.battle.isIgnoreViewListener == false)
            {
                if (this.event2ViewListenerList.TryGetValue(eventType, out d))
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
        }
        //two parameters
        public  void dispatchEvent<T, X>(String eventType, T arg1, X arg2)
        {
            Delegate d;
            if (this.event2ListenerList.TryGetValue(eventType, out d))
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
            if (this.battle.isIgnoreViewListener == false)
            {
                if (this.event2ViewListenerList.TryGetValue(eventType, out d))
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
        }
        //three parameters
        public  void dispatchEvent<T, X, Y>(String eventType, T arg1, X arg2, Y arg3)
        {
            Delegate d;
            if (this.event2ListenerList.TryGetValue(eventType, out d))
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
            if (this.battle.isIgnoreViewListener == false)
            {
                if (this.event2ViewListenerList.TryGetValue(eventType, out d))
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
        }
        //four parameters
        public  void dispatchEvent<T, X, Y, Z>(String eventType, T arg1, X arg2, Y arg3, Z arg4)
        {
            Delegate d;
            if (this.event2ListenerList.TryGetValue(eventType, out d))
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
            if (this.battle.isIgnoreViewListener == false)
            {
                if (this.event2ViewListenerList.TryGetValue(eventType, out d))
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
        }
        //five parameters
        public  void dispatchEvent<T, X, Y, Z, W>(String eventType, T arg1, X arg2, Y arg3, Z arg4, W arg5)
        {
            Delegate d;
            if (this.event2ListenerList.TryGetValue(eventType, out d))
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
            if (this.battle.isIgnoreViewListener == false)
            {
                if (this.event2ViewListenerList.TryGetValue(eventType, out d))
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
        }

        public void dispatchEventForView(String eventType)
        {
            Delegate d;
            if (this.battle.isIgnoreViewListener == false)
            {
                if (this.event2ViewListenerList.TryGetValue(eventType, out d))
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
        }
        //single parameters
        public void dispatchEventForView<T>(String eventType, T arg)
        {
            Delegate d;
            if (this.battle.isIgnoreViewListener == false)
            {
                if (this.event2ViewListenerList.TryGetValue(eventType, out d))
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
        }
        //two parameters
        public void dispatchEventForView<T, X>(String eventType, T arg1, X arg2)
        {
            Delegate d;
            if (this.battle.isIgnoreViewListener == false)
            {
                if (this.event2ViewListenerList.TryGetValue(eventType, out d))
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
        }
        //three parameters
        public void dispatchEventForView<T, X, Y>(String eventType, T arg1, X arg2, Y arg3)
        {
            Delegate d;
            if (this.battle.isIgnoreViewListener == false)
            {
                if (this.event2ViewListenerList.TryGetValue(eventType, out d))
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
        }
        //four parameters
        public void dispatchEventForView<T, X, Y, Z>(String eventType, T arg1, X arg2, Y arg3, Z arg4)
        {
            Delegate d;
            if (this.battle.isIgnoreViewListener == false)
            {
                if (this.event2ViewListenerList.TryGetValue(eventType, out d))
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
        }
        //five parameters
        public void dispatchEventForView<T, X, Y, Z, W>(String eventType, T arg1, X arg2, Y arg3, Z arg4, W arg5)
        {
            Delegate d;
            if (this.battle.isIgnoreViewListener == false)
            {
                if (this.event2ViewListenerList.TryGetValue(eventType, out d))
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
        }

        public void dispose()
        {
            this.event2ListenerList.Clear();
            this.event2ViewListenerList.Clear();
        }
    }
}
