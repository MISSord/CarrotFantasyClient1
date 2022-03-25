namespace ETModel
{
    public class zh_cn : zh_language
    {
        public override void init()
        {
            //room 
            this.zhLangage.Add(100, "未准备好");
            this.zhLangage.Add(101, "准备好了");
            this.zhLangage.Add(102, "");
            this.zhLangage.Add(103, "正在匹配...");
            this.zhLangage.Add(104, "(匹配时间:{0})正在匹配");

            //Battle
            this.zhLangage.Add(1001, " {0} {1}  / {2} 波怪物");
            this.zhLangage.Add(1002, "你共击退了   {0}  {1}    / {2} 波怪物");
            this.zhLangage.Add(1003, "关卡:{0}-{1} 当前处于冒险模式");
            this.zhLangage.Add(1004, "金币数量不足");
        }
    }
}
