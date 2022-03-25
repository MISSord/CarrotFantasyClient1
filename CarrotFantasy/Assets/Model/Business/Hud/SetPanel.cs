using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace ETModel
{
    public class SetPanel : BasePanel
    {

        private GameObject optionPageGo;
        private GameObject producerPageGo;
        private bool playBGMusic = true;
        private bool playEffectMusic = true;
        public Sprite[] btnSpritesList;//0.音效开 1.音效关 2.背景音乐开 3.背景音乐关
        private Image Img_Btn_EffectAudio;
        private Image Img_Btn_BGAudio;

        private Button btnEffectAudio;
        private Button btnBGAudio;

        private Button btnOptionPage;
        private Button btnProducePage;
        private Button btnReturn;

        private int stateId;

        private Vector3 fadePosition = new Vector3(0, 3000, 0);
        private Vector3 showPosition = Vector3.zero;

        public SetPanel(Dictionary<string, dynamic> param) : base(param)
        {
            this.prefabUrl = "Prefabs/Business/Hud/SetPanel";
            this.btnSpritesList = new Sprite[4];
        }

        public override void init()
        {
            base.init();
            this.panelManagerUnit.registerOnAssetReady(this.OnAssetReady);
            this.panelManagerUnit.registerOnDestroy(this.OnDestroy);
        }

        protected override void OnAssetReady()
        {
            base.OnAssetReady();

            this.stateId = 1;

            this.optionPageGo = transform.Find("OptionPage").gameObject;
            this.producerPageGo = transform.Find("ProducerPage").gameObject;
            this.Img_Btn_EffectAudio = optionPageGo.transform.Find("Btn_EffectAudio").GetComponent<Image>();
            this.Img_Btn_BGAudio = optionPageGo.transform.Find("Btn_BGAudio").GetComponent<Image>();

            this.btnBGAudio = this.optionPageGo.transform.Find("Btn_BGAudio").GetComponent<Button>();
            this.btnEffectAudio = this.optionPageGo.transform.Find("Btn_EffectAudio").GetComponent<Button>();

            this.btnOptionPage = this.transform.Find("node_top/Btn_Option").GetComponent<Button>();
            this.btnProducePage = this.transform.Find("node_top/Btn_Producer").GetComponent<Button>();
            this.btnReturn = this.transform.Find("node_top/Btn_Return").GetComponent<Button>();

            this.loadResource();
            this.addListener();

            this.Img_Btn_BGAudio.sprite = UIServer.getInstance().audioManager.musicEnable == true ? this.btnSpritesList[2] : this.btnSpritesList[3];
            this.Img_Btn_EffectAudio.sprite = UIServer.getInstance().audioManager.effectEnable == true ? this.btnSpritesList[0] : this.btnSpritesList[1];

            this.updatePagePosition();
        }

        private void updatePagePosition()
        {
            this.optionPageGo.transform.localPosition = this.stateId == 1 ? this.showPosition : this.fadePosition;
            this.producerPageGo.transform.localPosition = this.stateId == 2 ? this.showPosition : this.fadePosition;
        }

        private void addListener()
        {
            this.btnBGAudio.onClick.AddListener(this.updateMusicState);
            this.btnEffectAudio.onClick.AddListener(this.updateEffectState);

            this.btnOptionPage.onClick.AddListener(this.showOptionPage);
            this.btnProducePage.onClick.AddListener(this.showProducePage);

            this.btnReturn.onClick.AddListener(this.returnToLastPanel);
        }

        private void showOptionPage()
        {
            this.stateId = 1;
            this.updatePagePosition();
            UIServer.getInstance().playButtonEffect();
        }

        private void showProducePage()
        {
            this.stateId = 2;
            this.updatePagePosition();
            UIServer.getInstance().playButtonEffect();
        }

        private void returnToLastPanel()
        {
            UIServer.getInstance().playButtonEffect();
            this.finish();
        }

        private void updateMusicState()
        {
            if(UIServer.getInstance().audioManager.musicEnable == true)
            {
                this.Img_Btn_BGAudio.sprite = this.btnSpritesList[3];
                UIServer.getInstance().audioManager.setMusicEnable(false);
            }
            else
            {
                this.Img_Btn_BGAudio.sprite = this.btnSpritesList[2];
                UIServer.getInstance().audioManager.setMusicEnable(true);
            }
        }

        private void updateEffectState()
        {
            if (UIServer.getInstance().audioManager.effectEnable == true)
            {
                this.Img_Btn_EffectAudio.sprite = this.btnSpritesList[1];
                UIServer.getInstance().audioManager.setEffectEnable(false);
            }
            else
            {
                this.Img_Btn_EffectAudio.sprite = this.btnSpritesList[0];
                UIServer.getInstance().audioManager.setEffectEnable(true);
            }
        }

        private void loadResource()
        {
            this.btnSpritesList[0] = ResourceLoader.getInstance().loadRes<Sprite>("Pictures/Main/SetPanel/OptionPage/setting02-hd_15");
            this.btnSpritesList[1] = ResourceLoader.getInstance().loadRes<Sprite>("Pictures/Main/SetPanel/OptionPage/setting02-hd_21");
            this.btnSpritesList[2] = ResourceLoader.getInstance().loadRes<Sprite>("Pictures/Main/SetPanel/OptionPage/setting02-hd_6");
            this.btnSpritesList[3] = ResourceLoader.getInstance().loadRes<Sprite>("Pictures/Main/SetPanel/OptionPage/setting02-hd_11");
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}
