using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ETModel
{
    public class PanelServer
    {
        private int uuid; //记录面板的id
        private Dictionary<int, BasePanel> panelDic = new Dictionary<int, BasePanel>(); //方便获取
        private List<BasePanel> panelList = new List<BasePanel>(); //方便增删
        public EventDispatcher eventDispatcher;

        private bool isCanShowPanel = true;
        private TipUI tipPanel;
        private String tipPanelUrl = "Prefabs/Util/TipPanel";

        private Dictionary<PanelLayerType, GameObject> curPanlInfo = new Dictionary<PanelLayerType, GameObject>();

        private List<BasePanel> preLoadPanelList = new List<BasePanel>();

        public void init()
        {
            this.uuid = 0;
            this.eventDispatcher = new EventDispatcher();
            Server.sceneServer.getEventDispatcher().addListener(SceneEventType.LOAD_SCENE_FINISH, this.sceneLoadFinishCallBack);
        }

        private int getPanelUid()
        {
            this.uuid += 1;
            return uuid;
        }

        private void sceneLoadFinishCallBack()
        {
            this.reloadPanelLayerInfo();
            this.tryShowPreLoadPanel();
            this.addTipPanel();
        }

        private void addTipPanel()
        {
            GameObject panel = ResourceLoader.getInstance().getGameObject(tipPanelUrl);
            GameObject pan = GameObject.Instantiate(panel);
            pan.transform.SetParent(this.curPanlInfo[PanelLayerType.Tip].transform, false);
            this.tipPanel = new TipUI(pan);
        }

        public void showTip(String tip)
        {
            this.tipPanel.refreshTip(tip);
        }

        public void showTipLong(String tip)
        {
            this.tipPanel.showTip(tip);
        }

        public void fadeTipLong()
        {
            this.tipPanel.fadeTip();
        }

        private void tryShowPreLoadPanel()
        {
            for (int i = 0; i <= preLoadPanelList.Count - 1; i++)
            {
                BasePanel panel = preLoadPanelList[i];
                panel.isShowByPreLoad = true;
                this.showPanel(panel);
            }
            preLoadPanelList.Clear();
        }

        private void reloadPanelLayerInfo()
        {
            this.curPanlInfo = Server.sceneServer.getPanelLayerInfo();
        }

        public void showPanel(BasePanel targetPanel)
        {
            if(targetPanel == null)
            {
                Debug.Log("面板类不可用");
                return;
            }
            String curPath = targetPanel.prefabUrl;
            if (isCanShowPanel == false)
            {
                preLoadPanelList.Add(targetPanel);
                Debug.Log(String.Format("当前无法打开面板——{0}", curPath));
                return;
            }
            Dictionary<String, System.Object> msg = new Dictionary<string, System.Object>() {
                {"panelName",curPath },{ "enableShow", true},{"reason","" } };
            this.eventDispatcher.dispatchEvent(PanelEventType.OPEN_PANEL_PREPARE, msg);
            if ((bool)msg["enableShow"] == false)
            {
                Debug.Log(String.Format("{0}面板打开被打断,原因{1}", curPath,msg["reason"]));
                return;
            }
            if (Server.sceneServer.getCurScene() == null)
            {
                Debug.Log("当前场景不可用");
                return;
            }
            foreach (BasePanel panel in panelList)
            {
                if(String.Equals(curPath, panel.prefabUrl))
                {
                    Debug.Log(String.Format("已存在当前相同面板——{0}", curPath));
                    return;
                }
            }
            GameObject item = ResourceLoader.getInstance().getGameObject(curPath);
            if(item != null)
            {
                GameObject tranPanel = GameObject.Instantiate(item);
                if(tranPanel == null)
                {
                    Debug.Log(String.Format("打开面板失败,prefab加载失败：{0}", curPath));
                    return;
                }
                tranPanel.layer = SceneLayerData.layerType[1]; //UI层
                targetPanel.initContainer();
                if(curPanlInfo[targetPanel.panelLayerType] == null)
                {
                    Debug.Log(String.Format("当前UI层级不可用——{0}{1}", curPath,targetPanel.panelLayerType));
                    return;
                }
                targetPanel.setLayerTran(curPanlInfo[targetPanel.panelLayerType].transform);
                targetPanel.initTran(tranPanel.transform);
                targetPanel.init(); //子类复写

                targetPanel.panelManagerUnit.onAssetReady();

                int uid = this.getPanelUid();
                targetPanel.panelUid = uid;
                panelDic.Add(uid, targetPanel);
                panelList.Add(targetPanel);

                targetPanel.panelManagerUnit.onResume();
            }
            else
            {
                Debug.Log(String.Format("打开面板失败,prefab加载失败：{0}", curPath));
            }
        }

        public void closePanel(int uid, int closeReason)
        {
            BasePanel targetPanel;
            if (panelDic.TryGetValue(uid, out targetPanel))
            {
                if(targetPanel.isPreLoadOpen == true)
                {
                    this.addToPreLoadPanelList(targetPanel);
                }
                if(closeReason != PanelCloseReasonType.SCENE_CHANGE)
                {
                    panelList.Remove(targetPanel);
                }
                panelDic.Remove(uid);
                targetPanel.panelManagerUnit.onDestroy();
                GameObject.Destroy(targetPanel.container);
            }
            else
            {
                Debug.Log(String.Format("关闭面板失败,面板Id：{0}", uid));
            }
        }

        private void addToPreLoadPanelList(BasePanel panel)
        {
            preLoadPanelList.Add(panel);
        }

        public void setShowPanelActive(bool isCanShow)
        {
            this.isCanShowPanel = isCanShow;
        }

        public void closeAllPanel(int closeReason, BaseSceneType nextSceneType)
        {
            for(int i = panelList.Count - 1; i >= 0; i--)
            {
                if (closeReason == PanelCloseReasonType.SCENE_CHANGE && panelList[i].isCloseBySceneChange())
                {
                    this.panelList[i].finish();
                }
            }
        }

    }
}
