using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETModel
{
    public class StartLoadPanel : BasePanel
    {
        public StartLoadPanel(Dictionary<string, dynamic> param) : base(param)
        {
            this.isClickGrayEnable = false;
            this.prefabUrl = "Prefabs/Business/Login/StartLoadPanel";
        }

        public override void init()
        {
            base.init();
        }

        public void autoClose()
        {
            this.finish();
        }
    }
}
