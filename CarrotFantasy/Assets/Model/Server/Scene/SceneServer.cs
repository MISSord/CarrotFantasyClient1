using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ETModel
{
    public class SceneServer 
    {
        private EventDispatcher eventDispatcher;
        private Camera uiCamera; //固定的
        private Camera mainCamera; // 主摄像机（主要是拍3D物体）
        public BaseScene currentScene;

        public void init()
        {
            this.currentScene = null;
            this.eventDispatcher = new EventDispatcher();
            GameObject uiCamera = GameObject.Find("uiCamera");
            this.uiCamera = uiCamera.GetComponent<Camera>();
        }

        public Camera getUICamera()
        {
            return this.uiCamera;
        }

        public EventDispatcher getEventDispatcher()
        {
            return this.eventDispatcher;
        }

        public BaseScene getCurScene()
        {
            return currentScene;
        }

        public Dictionary<PanelLayerType, GameObject> getPanelLayerInfo()
        {
            return this.currentScene.getLayerDic();
        }

        private void removeScene()
        {
            Server.panelServer.closeAllPanel(PanelCloseReasonType.SCENE_CHANGE, this.currentScene.sceneType);
            Server.panelServer.setShowPanelActive(false);
            this.currentScene.dispose(); //卸载旧场景
        }

        public bool loadScene(BaseSceneType sceneType, Dictionary<String, dynamic> param)
        {
            bool isLoad = false;
            if(this.currentScene != null)
            {
                if (this.currentScene.sceneType == sceneType) return isLoad;
                this.removeScene();
            }
            //ResourceLoader.getInstance().setSceneType(sceneType); 切换场景，卸载旧场景资源（）
            isLoad = this.loadSceneProgress(sceneType, param);
            return isLoad;
        }

        private bool loadSceneProgress(BaseSceneType sceneType, Dictionary<String, dynamic> param)
        {
            BaseScene targetScene = null;
            switch (sceneType) {
                case BaseSceneType.MainScene:
                    targetScene = new MainScene(sceneType, "MainScene", param);
                    break;
                case BaseSceneType.BattleScene:
                    targetScene = new BattleScene(sceneType, "BattleScene", param);
                    break;
                default:
                    Debug.Log("场景加载失败");
                    break;
            }
            if(targetScene == null)
            {
                return false;
            }
            this.currentScene = targetScene;
            this.mainCamera = currentScene.getMainCamera();
            if (this.mainCamera != null)
            {
                this.uiCamera.clearFlags = CameraClearFlags.Depth;
            }
            else
            {
                this.uiCamera.clearFlags = CameraClearFlags.Color;
            }
            this.currentScene.initSceneObject();

            Server.panelServer.setShowPanelActive(true); //其实不一定需要这句
            this.eventDispatcher.dispatchEvent(SceneEventType.LOAD_SCENE_FINISH);

            this.currentScene.init();
            return true;
        }

    }
}
