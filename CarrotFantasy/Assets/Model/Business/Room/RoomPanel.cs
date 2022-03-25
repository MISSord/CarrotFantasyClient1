using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine;

namespace ETModel
{
    public class RoomPanel : BasePanel
    {

        private Button btn_fight;
        private Button btn_canel;

        private Text txt_tips;
        private Text txt_userName;
        private Text txt_userState;
        private Text txt_myState;

        private int matchTime = 0;
        private int scheId;


        public RoomPanel(Dictionary<string, dynamic> param) : base(param)
        {
            this.isClickGrayEnable = false;
            this.prefabUrl = "Prefabs/Business/Room/RoomPanel";
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
            this.btn_fight = this.transform.Find("node_bottom/btn_ready").GetComponent<Button>();
            this.btn_canel = this.transform.Find("node_bottom/btn_back").GetComponent<Button>();

            this.txt_tips = this.transform.Find("node_up/txt_tips").GetComponent<Text>();
            this.txt_tips.text = LanguageUtil.getInstance().getString(103);
            
            this.initUserInfoUI();
            this.addListener();
            this.scheId = Sche.delayExeMultipleTimes(this.updateTipTxt, 1.0f);
        }

        private void initUserInfoUI()
        {
            this.txt_userName = this.transform.Find("node_center/node_info/node_info1/txt_user_name").GetComponent<Text>();
            this.txt_userState = this.transform.Find("node_center/node_info/node_info2/txt_user_name").GetComponent<Text>();
            this.txt_myState = this.transform.Find("node_center/node_info/node_info3/txt_user_name").GetComponent<Text>();

            this.txt_userName.text = LanguageUtil.getInstance().getString(102);
            this.txt_userState.text = LanguageUtil.getInstance().getString(100);
            this.txt_myState.text = LanguageUtil.getInstance().getString(100);
        }

        private void addListener()
        {
            this.btn_canel.onClick.AddListener(this.canelFight);
            this.btn_fight.onClick.AddListener(this.stateToFight);

            RoomServer.getInstance().eventDispatcher.addListener(RoomEventType.USER_INFO_CHANGE, this.changeUserInfo);
        }

        private void removeListener()
        {
            RoomServer.getInstance().eventDispatcher.removeListener(RoomEventType.USER_INFO_CHANGE, this.changeUserInfo);
        }

        private void updateTipTxt()
        {
            this.matchTime += 1;
            this.txt_tips.text = LanguageUtil.getInstance().getFormatString(104, this.matchTime.ToString());
            if(this.matchTime >= 31)
            {
                this.canelFight();
            }
        }

        private void changeUserInfo()
        {
            if(RoomServer.getInstance().partner != null)
            {
                this.txt_userName.text = RoomServer.getInstance().partner.UserID.ToString();
                if(RoomServer.getInstance().partner.isReady == true)
                {
                    this.txt_userState.text = LanguageUtil.getInstance().getString(101);
                }
                else
                {
                    this.txt_userState.text = LanguageUtil.getInstance().getString(100);
                }
            }
            else
            {
                this.txt_userName.text = LanguageUtil.getInstance().getString(102);
                this.txt_userState.text = LanguageUtil.getInstance().getString(100);
            }
            this.txt_myState.text = LanguageUtil.getInstance().getString(RoomServer.getInstance().myself.isReady ? 101:100);
        }

        private void stateToFight()
        {
            //发送准备消息
            UIServer.getInstance().playButtonEffect();
            RoomServer.getInstance().sendReadyFight();
            //
        }

        private void canelFight()
        {
            //发送取消消息
            UIServer.getInstance().playButtonEffect();
            RoomServer.getInstance().canelMatch();
            this.finish();
        }

        protected override void OnDestroy()
        {
            this.removeListener();
            Sche.silenceSingleSche(this.scheId);
            base.OnDestroy();
        }
    }
}
