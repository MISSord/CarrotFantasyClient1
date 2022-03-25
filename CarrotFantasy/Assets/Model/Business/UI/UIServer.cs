using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace ETModel
{
    public class UIServer : BaseServer
    {
        private GameObject nodeObject;
        public AudioManager audioManager;
        public FpsNode fpsNode;

        public static UIServer uiServer;

        public static UIServer getInstance()
        {
            if(uiServer == null)
            {
                uiServer = new UIServer();
            }
            return uiServer;
        }

        public override void loadModule()
        {
            base.loadModule();
            this.initGlobalCanvas();
            this.initAudioManager();
            this.showFpsNode();
            this.initCustomizeShow();
            this.initResolution();
        }

        private void initGlobalCanvas()
        {
            this.nodeObject = new GameObject("global_canvas");
            this.nodeObject.layer = SceneLayerData.layerType[1];
            Canvas canvas = this.nodeObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = Server.sceneServer.getUICamera();
            canvas.overrideSorting = true;
            canvas.sortingOrder = 1000;

            CanvasScaler canvasScaler = nodeObject.AddComponent<CanvasScaler>();
            UIUtil.getInstance().initCanvasScale(canvasScaler);

            GraphicRaycaster graphic = nodeObject.AddComponent<GraphicRaycaster>();
        }

        private void addToGlobalUI(GameObject res)
        {
            res.transform.SetParent(this.nodeObject.transform, false);
        }

        private void initAudioManager()
        {
            audioManager = new AudioManager();
            audioManager.init();
            this.addToGlobalUI(audioManager.nodeObject);
        }

        private void showFpsNode()
        {
            fpsNode = new FpsNode();

        }

        private void initCustomizeShow()
        {

        }

        private void initResolution()
        {
            /*
            int height = UnityEngine.Screen.height;
            if(height >= 1440)
            {
                height = 1440;
            }
            else if(height >= 1080)
            {
                height = 1080;
            }
            else
            {
                height = 720;
            }
            //UnityEngine.Screen.SetResolution((int)(UIUtil.getInstance().SCREEN_RADIO * height), height, false);
            UIUtil.getInstance().curSetScreenSize = new Vector2((int)(GameConfig.DEVELOPMENT_SCREEN_PROP * (float)height), height);
            UnityEngine.Screen.SetResolution((int)(GameConfig.DEVELOPMENT_SCREEN_PROP * (float)height), height, false);
            */
            UIUtil.getInstance().curSetScreenSize = new Vector2(1920, 1440);
            UnityEngine.Screen.SetResolution(1920, 1440, false);
        }

        public override void dispose()
        {
            base.dispose();
            if(this.audioManager != null)
            {
                this.audioManager.dipose();
            }
            GameObject.Destroy(this.nodeObject);
        }

        public void playMainBg()
        {
            this.audioManager.playMusic("AudioClips/Main/BGMusic");
        }

        public void playButtonEffect()
        {
            this.audioManager.playEffect("AudioClips/Main/Button");
        }

        public void playPagingEffect()
        {
            this.audioManager.playEffect("AudioClips/Main/Paging");
        }
    }
}
