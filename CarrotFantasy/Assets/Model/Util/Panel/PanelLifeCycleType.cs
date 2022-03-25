using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETModel
{
    public class PanelLifeCycleType
    {
        public const String ON_RESUME = "on_resume";
        public const String ON_DESTROY = "on_destroy";
        public const String ON_ASSET_READY = "on_asset_ready";
        public const String ON_PAUSE = "on_pause";
    }

    public enum PanelStateType
    {
        creating,
        init_done, //初始化完成
        active,
        pending, //挂起
        disable,
    }
}
