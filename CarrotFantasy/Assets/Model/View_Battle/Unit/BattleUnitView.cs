using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace ETModel
{
    public class BattleUnitView
    {
        private Dictionary<String, BaseUnitViewComponent> componentDic = new Dictionary<string, BaseUnitViewComponent>();
        private List<BaseUnitViewComponent> componentList = new List<BaseUnitViewComponent>();

        public BattleView_base battleView;
        public BattleUnit unit;
        public BattleEventDispatcher unitEventDispatcher;

        private Vector3 curViewPosition;
        private float curViewFaceDirection;

        private Vector3 hidePositon = new Vector3(5000, 5000, 0);
        private UnitTransformComponent transformComponent;

        private bool isPaused = false;
        public bool isVisible; //是否可见

        private Vector3 lastPosition;

        public Transform transform { get; set; }

        public virtual void initTransform(Transform node)
        {
            this.transform = node;
        }

        public void loadInfo(BattleView_base battleView, BattleUnit battleUnit)
        {
            this.battleView = battleView;
            this.unit = battleUnit;

            this.unitEventDispatcher = battleUnit.eventDipatcher;
            this.isPaused = false;
            this.isVisible = true;
            this.transformComponent = (UnitTransformComponent)this.unit.getComponent(UnitComponentType.TRANSFORM);
        }

        public virtual void reloadInfo()
        {
            this.initViewPosition();
            this.initViewFaceDirection();
            this.updateUnitScale();
            this.updateBodyRect();
        }

        public virtual void init()
        {
            this.initListener();
            this.initViewPosition();
            this.initViewFaceDirection();
            this.updateUnitScale();
            this.updateBodyRect();
        }

        public virtual void onTick(float deltaTime)
        {
            this.updatePosition(deltaTime);
        }

        public virtual void initComponents() //子类调用
        {
            for(int i = 0; i<= this.componentList.Count - 1; i++)
            {
                this.componentList[i].init();
            }
        }

        public void addComponent(BaseUnitViewComponent component)
        {
            if (component == null) return;
            this.componentDic.Add(component.componetType, component);
            this.componentList.Add(component);
        }

        public void removeComponent(BaseUnitViewComponent component)
        {
            if (component == null) return;
            this.componentDic.Remove(component.componetType);
            this.componentList.Remove(component);
        }

        public BaseUnitViewComponent getComponent(String type)
        {
            return this.componentDic[type];
        }

        public virtual void initListener()
        {
            this.unitEventDispatcher.addListener(UnitEvent.FACE_DIRECTION_CHANGE, this.updateFaceDirection);
            this.unitEventDispatcher.addListener(UnitEvent.STATUS_CHANGE, this.updateUnitScale);
            this.unitEventDispatcher.addListener(UnitEvent.ROTATION_CHANGE, this.updateRotation);
            this.unitEventDispatcher.addListener(UnitEvent.BODY_RECT_CHANE, this.updateBodyRect);
            this.unitEventDispatcher.addListener(UnitEvent.POSITION_CHANGE, this.getLastPosition);
        }

        public virtual void removeListener()
        {
            if (this.unitEventDispatcher == null) return;
            this.unitEventDispatcher.removeListener(UnitEvent.FACE_DIRECTION_CHANGE, this.updateFaceDirection);
            this.unitEventDispatcher.removeListener(UnitEvent.STATUS_CHANGE, this.updateUnitScale);
            this.unitEventDispatcher.removeListener(UnitEvent.ROTATION_CHANGE, this.updateRotation);
            this.unitEventDispatcher.removeListener(UnitEvent.BODY_RECT_CHANE, this.updateBodyRect);
            this.unitEventDispatcher.removeListener(UnitEvent.POSITION_CHANGE, this.getLastPosition);

        }

        private void updateUnitScale()
        {
            //for (int i = 0; i <= this.componentList.Count - 1; i++)
            //{
                //this.componentList[i].setUnitScale(this.transformComponent.scale * this.unit.define.scale);
            //}
            //this.updateUnityScale();
        }

        private void updateRotation()
        {
            for (int i = 0; i <= this.componentList.Count - 1; i++)
            {
                this.componentList[i].setUnitRotation((float)this.transformComponent.rotation);
            }
        }

        private void updateBodyRect()
        {

        }

        private void updatePosition(float time)
        {
            //Vector3 newViewPosition = Vector3.Lerp(transform.position, this.lastPosition, 1 / Vector3.Distance(transform.position, this.lastPosition) * time);
            //this.curViewPosition = newViewPosition;
            this.curViewPosition = this.lastPosition;
            this.refreshUnityTransform();
            for (int i = 0; i <= this.componentList.Count - 1; i++)
            {
                this.componentList[i].setUnitPosition(this.curViewPosition.x, this.curViewPosition.y, this.curViewPosition.z);
            }

        }

        private void initViewPosition()
        {
            this.getLastPosition();
            this.curViewPosition = this.lastPosition;
            this.refreshUnityTransform();
            for(int i = 0; i <= this.componentList.Count - 1; i++)
            {
                this.componentList[i].setUnitPosition(this.curViewPosition.x, this.curViewPosition.y, this.curViewPosition.z);
            }
        }

        private void getLastPosition()
        {
            this.transformComponent.getPosition(out this.lastPosition.x, out this.lastPosition.y, out this.lastPosition.z);
        }

        private void refreshUnityTransform()
        {
            if(this.isVisible == true)
            {
                this.updateUnityPosition();
            }
            else
            {
                if (this.transform == null) return;
                this.transform.localPosition = hidePositon;
            }
        }

        private void updateUnityPosition()
        {
            this.transform.localPosition = this.curViewPosition;
        }

        private void initViewFaceDirection()
        {
            this.curViewFaceDirection = (float)this.transformComponent.faceDirection;
            this.transform.localRotation = Quaternion.Euler(0, this.curViewFaceDirection, 0);
            for (int i = 0; i <= this.componentList.Count - 1; i++)
            {
                this.componentList[i].setUnitFaceDirection(this.curViewFaceDirection);
            }
        }

        protected virtual void updateFaceDirection() //根据移动的方向进行调整
        {
            float faceDirection = (float)this.transformComponent.faceDirection;
            this.transform.localRotation = Quaternion.Euler(0, this.curViewFaceDirection, 0);
            this.curViewFaceDirection = faceDirection;
        }

        public virtual void clearUnitInfo()
        {
            this.removeListener();
            this.unitEventDispatcher = null;
            this.unit = null;
            this.transformComponent = null;
            this.isVisible = false;
            for (int i = this.componentList.Count - 1; i >= 0; i--)
            {
                this.componentList[i].dispose();
                GameViewObjectPool.getInstance().pushViewObjectToPool(this.componentList[i].componetType, this.componentList[i]); ;
            }
            this.componentList.Clear();
            this.componentDic.Clear();
            this.transform = null;
        }

        public virtual void dispose()
        {
            this.clearUnitInfo();
            this.battleView = null;
        }
    }
}
