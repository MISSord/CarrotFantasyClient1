using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine;

namespace ETModel
{
    public class LoginPanel : BasePanel
    {
        private Button btnLogin;
        private Button btnResgister;
        private Button btnBack;

        private bool isResigterState = false;

        private InputField inputAccount;
        private InputField inputPassword;
        private InputField inputSurePassword;

        private GameObject nodeInputSurePassword;
        private GameObject nodeBtnBack;
        private GameObject nodeBtnLogin;

        public LoginPanel(Dictionary<string, dynamic> param) : base(param)
        {
            this.isClickGrayEnable = false;
            this.prefabUrl = "Prefabs/Business/Login/LoginPanel";
        }

        public override void init()
        {
            base.init();
            this.panelManagerUnit.registerOnAssetReady(this.OnAssetReady);
            this.panelManagerUnit.registerOnDestroy(this.OnDestroy);
        }

        protected override void OnAssetReady()
        {
            this.inputAccount = this.transform.Find("node_up/input_account").GetComponent<InputField>();
            this.inputPassword = this.transform.Find("node_up/input_password").GetComponent<InputField>();
            this.nodeInputSurePassword = this.transform.Find("node_up/input_sure_password").gameObject;
            this.inputSurePassword = this.nodeInputSurePassword.transform.GetComponent<InputField>();
            this.nodeBtnBack = this.transform.Find("node_bottom/btn_back").gameObject;
            this.btnBack = this.nodeBtnBack.transform.GetComponent<Button>();

            this.nodeBtnLogin = this.transform.Find("node_bottom/btn_login").gameObject;
            this.btnLogin = this.nodeBtnLogin.transform.GetComponent<Button>();
            this.btnResgister = this.transform.Find("node_bottom/btn_register").GetComponent<Button>();

            this.btnLogin.onClick.AddListener(this.loginAccount);
            this.btnBack.onClick.AddListener(this.backLoginState);
            this.btnResgister.onClick.AddListener(this.registerEvent);
            this.backLoginState();
            this.addListener();
        }

        private void addListener()
        {
            AccountServer.getInstance().eventDispatcher.addListener(AccountServer.LOGIN_SUCCESS, this.finish);
        }

        private void removeListener()
        {
            AccountServer.getInstance().eventDispatcher.removeListener(AccountServer.LOGIN_SUCCESS, this.finish);
        }

        private void loginAccount()
        {
            String accountText = this.inputAccount.text;
            String passwordText = this.inputPassword.text;
            //或许还能做其他检验
            if (accountText == null || accountText.Equals("") || passwordText == null || passwordText.Equals(""))
            {
                //不能为空
                UIServer.getInstance().showTip("账号或密码不能为空");
                return;
            }
            AccountServer.getInstance().loginAccount(accountText, passwordText);
        }

        private void registerEvent()
        {
            if(this.isResigterState == true)
            {
                this.registerAccount();
            }
            else if(this.isResigterState == false)
            {
                this.enterRegisterAccountState();
            }
        }

        private void registerAccount()
        {
            String accountText = this.inputAccount.text;
            String passwordText = this.inputPassword.text;
            String suerpasswordText = this.inputSurePassword.text;
            if (accountText == null || accountText.Equals("") || passwordText == null || passwordText.Equals("") || suerpasswordText == null || suerpasswordText.Equals(""))
            {
                //不能为空
                UIServer.getInstance().showTip("账号或密码不能为空");
                return;
            }
            if (!passwordText.Equals(suerpasswordText))
            {
                //不能不一样
                UIServer.getInstance().showTip("两次输入的密码不一样");
                return;
            }
            AccountServer.getInstance().registerAccount(accountText, passwordText);
        }

        private void enterRegisterAccountState()
        {
            this.nodeBtnBack.SetActive(true);
            this.nodeBtnLogin.SetActive(false);
            this.nodeInputSurePassword.SetActive(true);
            this.ClearText();
            this.isResigterState = true;
        }

        private void backLoginState()
        {
            this.nodeBtnBack.SetActive(false);
            this.nodeBtnLogin.SetActive(true);
            this.nodeInputSurePassword.SetActive(false);
            this.ClearText();
            this.isResigterState = false;
        }

        private void ClearText()
        {
            this.inputAccount.text = "";
            this.inputPassword.text = "";
            this.inputSurePassword.text = "";
        }

        protected override void OnDestroy()
        {
            this.removeListener();
            base.OnDestroy();
        }
    }
}
