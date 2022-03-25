namespace ETModel
{
    public static partial class ErrorCode
    {
        public const int ERR_SignError = 10000;

        //自定义错误

        //登录功能
        public const int ERR_AccountAlreadyRegisted = 200101; 
        public const int ERR_AccountOrPasswordError = 200102;
        public const int ERR_RepeatedAccountExist = 200103;

        //地图功能
        public const int ERR_GetMapInfoError = 200201;
        public const int ERR_SendMapInfoError = 200202; //是指发送的消息和服务端的不一样，主要是读配置，不一样说明有人修改数据了
        public const int ERR_SaveMapInfoError = 200203; //后端保存某个关卡的信息失败了
        public const int ERR_GameInfoIllegal = 200204; //发送过来的游戏信息不合法
        public const int ERR_GameInfoCheckFail = 200205;//发送过来的游戏信息检查失败

        public const int ERR_UserNotOnline = 300003;
        public const int ERR_CreateNewCharacter = 300004;

        public const int ERR_Disconnect = 210000;
        public const int ERR_AccountAlreadyRegister = 210001;
        public const int ERR_JoinRoomError = 210002;

        public const int ERR_LoginError = 210005;


    }
}