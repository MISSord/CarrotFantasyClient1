using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETModel
{
    public class SceneLayerData
    {
        public static Dictionary<PanelLayerType, bool> sceneLayerSetting = new Dictionary<PanelLayerType, bool>
        {
            { PanelLayerType.Hud, true},
            { PanelLayerType.Default, true},
            { PanelLayerType.UI, true},
            { PanelLayerType.Tip, true},
        };

        public static Dictionary<PanelLayerType, String> sceneLayerName = new Dictionary<PanelLayerType, String>
        {
            { PanelLayerType.Hud, "Hud"},
            { PanelLayerType.Default, "Default"},
            { PanelLayerType.UI, "UI"},
            { PanelLayerType.Tip, "Tip"},
        };

        public static int[] layerType = new int[] //Layer的层级
        {
            0, //Default
            5, //UI
            9, //Hidden
        };

    }

    public enum PanelLayerType 
    { 
        Default = 1,
        Hud = 2,
        UI = 3,
        Tip = 4,
    }

}
