using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ETModel
{
    public delegate void LiftCycleFunc();

    public class PanelManagerUnit
    {
        private PanelStateType panelStateType;
        private Dictionary<String, LiftCycleFunc> liftCycleExeMap = new Dictionary<string, LiftCycleFunc>(); //可以写成链表形式进行有序可控的执行。
        private GameObject panelGameObject;

        public PanelManagerUnit(GameObject panelObject)
        {
            this.panelGameObject = panelObject;
        }

        public void setState(PanelStateType state)
        {
            this.panelStateType = state;
        }

        public PanelStateType getState()
        {
            return this.panelStateType;
        }

        public void onAssetReady()
        {
            this.setState(PanelStateType.init_done);
            this.callLiftCycleFunc(PanelLifeCycleType.ON_ASSET_READY);
        }
        public void onResume()
        {
            this.setState(PanelStateType.active);
            this.callLiftCycleFunc(PanelLifeCycleType.ON_RESUME);
        }
        public void onPause()
        {
            this.setState(PanelStateType.pending);
            this.callLiftCycleFunc(PanelLifeCycleType.ON_PAUSE);
        }
        public void onDestroy()
        {
            this.setState(PanelStateType.disable);
            this.callLiftCycleFunc(PanelLifeCycleType.ON_DESTROY);
        }

        public void callLiftCycleFunc(String funcName)
        {
            LiftCycleFunc func = null;
            if(liftCycleExeMap.TryGetValue(funcName, out func))
            {
                func();
            }
        }
        private void registerLiftCycleEvent(String key, LiftCycleFunc func)
        {
            LiftCycleFunc fun = null;
            if (liftCycleExeMap.TryGetValue(key, out fun))
            {
                fun += func;
            }
            else
            {
                fun = func;
            }
            liftCycleExeMap[key] = fun;
        }
        public void registerOnAssetReady(LiftCycleFunc func)
        {
            this.registerLiftCycleEvent(PanelLifeCycleType.ON_ASSET_READY, func);
        }
        public void registerOnResume(LiftCycleFunc func)
        {
            this.registerLiftCycleEvent(PanelLifeCycleType.ON_RESUME, func);
        }
        public void registerOnPause(LiftCycleFunc func)
        {
            this.registerLiftCycleEvent(PanelLifeCycleType.ON_PAUSE, func);
        }
        public void registerOnDestroy(LiftCycleFunc func)
        {
            this.registerLiftCycleEvent(PanelLifeCycleType.ON_DESTROY, func);
        }


    }
}
