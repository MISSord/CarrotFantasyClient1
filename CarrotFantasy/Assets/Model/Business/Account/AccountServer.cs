using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETModel
{
    public class AccountServer : BaseServer
    {
        private static AccountServer accountServer;
        private String account;
        private long gateLoginKey;
        public long userId { get; private set; }
        private bool isInit = false;

        public static String LOGIN_SUCCESS = "Login_success";
        public EventDispatcher eventDispatcher;

        public static AccountServer getInstance()
        {
            if (accountServer == null)
            {
                accountServer = new AccountServer();
                accountServer.eventDispatcher = new EventDispatcher();
            }
            return accountServer;
        }

        public override void loadModule()
        {
            base.loadModule();
            this.addListener();
            this.userId = 0;
        }

        private void addListener()
        {
            
        }

        public override void addSocketListener()
        {
            Server.connectionServer.addListener(HotfixOpcode.A0003_LoginGate_G2C, this.notifyLoginGate);
            Server.connectionServer.addListener(HotfixOpcode.A0002_Login_R2C, this.notifyLogin);
            Server.connectionServer.addListener(HotfixOpcode.A0001_Register_R2C, this.notifyRegister);
        }

        public override void removeSocketListener()
        {
            Server.connectionServer.removeListener(HotfixOpcode.A0003_LoginGate_G2C, this.notifyLoginGate);
            Server.connectionServer.removeListener(HotfixOpcode.A0002_Login_R2C, this.notifyLogin);
            Server.connectionServer.removeListener(HotfixOpcode.A0001_Register_R2C, this.notifyRegister);
        }

        public void setAccountId(String id)
        {
            if (isInit == false)
            {
                account = id;
            }
            isInit = true;
        }

        public string getAccountId()
        {
            return this.account;
        }

        public override void dispose()
        {

        }

        public void loginAccount(String accout, String password)
        {
            Server.connectionServer.Send(new A0002_Login_C2R() { Account = accout, Password = password });
            this.account = accout;
        }

        public void loginGateAccount()
        {
            Server.connectionServer.Send(new A0003_LoginGate_C2G() { GateLoginKey = this.gateLoginKey });
        }

        public void registerAccount(String accout, String password)
        {
            Server.connectionServer.Send(new A0001_Register_C2R { Account = accout, Password = password });
        }

        private void notifyLogin(IMessage message)
        {
            A0002_Login_R2C messageRealm = (A0002_Login_R2C)message;
            //判断Realm服务器返回结果
            if (messageRealm.Error == ErrorCode.ERR_AccountOrPasswordError)
            {
                UIServer.getInstance().showTip("登录失败,账号或密码错误");
                return;
            }
            this.setAccountId(this.account);
            this.gateLoginKey = messageRealm.GateLoginKey;
            this.loginGateAccount();
        }

        private void notifyRegister(IMessage message)
        {
            A0001_Register_R2C messageRealm = (A0001_Register_R2C)message;
            if (messageRealm.Error == ErrorCode.ERR_AccountAlreadyRegisted)
            {
                UIServer.getInstance().showTip("注册失败，账号已被注册");
                return;
            }

            if (messageRealm.Error == ErrorCode.ERR_RepeatedAccountExist)
            {
                UIServer.getInstance().showTip("注册失败，出现重复账号");
                return;
            }

            //显示登录成功的提示
            UIServer.getInstance().showTip("注册成功");
        }

        private void notifyLoginGate(IMessage message)
        {
            A0003_LoginGate_G2C msg = (A0003_LoginGate_G2C)message;
            this.userId = msg.UserID;
            this.eventDispatcher.dispatchEvent(LOGIN_SUCCESS);
            UIServer.getInstance().showTip("登录成功,祝你游玩愉快");
        }
    }
}
