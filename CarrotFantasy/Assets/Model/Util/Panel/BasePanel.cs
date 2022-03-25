using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine;

namespace ETModel
{

    public class BasePanel
    {
        public int panelUid = 0;
        private Color grayColor = new Color(0, 0, 0, 195 / 255);
        private String grayBgPrefabPath = "Prefabs/UI/GrayBackPanel";
        protected Button grayButton;
        protected bool isClickGrayEnable; // 是否接受点击背景的按钮事件触发

        private GameObject container;

        public PanelLayerType panelLayerType = PanelLayerType.Default;
        protected Transform transform;

        public String prefabUrl = "";

        protected CanvasGroup canvasGroup;

        public PanelManagerUnit panelManagerUnit;

        public bool isPreLoadOpen = false; //用于切换场景后重新加载或延时打开
        public bool isShowByPreLoad = false; //

        protected bool isShowGray = true;

        public BasePanel(Dictionary<string, dynamic> param) { }

        public virtual void init() { }

        protected virtual void OnAssetReady() { }

        protected virtual void OnResume() { }

        protected virtual void OnPause() { }

        protected virtual void OnDestroy() { }

        public void initContainer()
        {
            this.container = new GameObject("panel_container");
            this.container.layer = SceneLayerData.layerType[1];
            Canvas canvas = container.AddComponent<Canvas>();
            
            GraphicRaycaster graphic = container.AddComponent<GraphicRaycaster>();
            this.canvasGroup =  container.AddComponent<CanvasGroup>();

            if (this.isShowGray)
            {
                //背景按钮初始化
                GameObject obj = ResourceLoader.getInstance().getGameObject(grayBgPrefabPath);
                GameObject grayGameObj = GameObject.Instantiate(obj);
                grayGameObj.transform.SetParent(container.transform, false);
                grayGameObj.layer = SceneLayerData.layerType[1];

                RectTransform rect = grayGameObj.transform.GetComponent<RectTransform>();
                rect.anchorMin = Vector2.zero;
                rect.anchorMax = Vector2.one;

                this.grayButton = grayGameObj.transform.Find("spr_bg").GetComponent<Button>();
                this.grayButton.onClick.AddListener(() => { if (this.isClickGrayEnable == true) this.finish(); });
            }
        }

        public void setLayerTran(Transform layer)
        {
            this.container.transform.SetParent(layer);
            RectTransform rectTran = container.transform.GetComponent<RectTransform>();
            rectTran.anchorMin = Vector2.zero;
            rectTran.anchorMax = Vector2.one;
            rectTran.offsetMin = Vector2.zero;
            rectTran.offsetMax = Vector2.zero;
            rectTran.localScale = Vector3.one;
        }

        public void initTran(Transform tran)
        {
            this.transform = tran;
            this.transform.SetParent(container.transform,false);
            this.panelManagerUnit = new PanelManagerUnit(this.transform.gameObject);
        }

        public void finish() //最后调用
        {
            int reason = PanelCloseReasonType.DEFAULT;
            Server.panelServer.closePanel(this.panelUid, reason);
            this.panelManagerUnit.onDestroy();
            GameObject.Destroy(this.container);
        }

        public virtual bool isCloseBySceneChange()
        {
            return true;
        }
    }
}
