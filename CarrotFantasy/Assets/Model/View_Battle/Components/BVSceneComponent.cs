using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace ETModel
{
    public class BVSceneComponent : BaseBattleViewComponent
    {
        private GameObject rootGameContainer;
        private Dictionary<String, GameObject> containerDic = new Dictionary<string, GameObject>();

        public BVSceneComponent(BattleView_base battleView) : base(battleView)
        {
            this.componentType = BattleViewComponentType.SCENE;
        }

        public override void init()
        {
            this.rootGameContainer = new GameObject("SceneContainer");
            this.rootGameContainer.transform.SetParent(this.battleView.rootGameObject.transform);
            this.rootGameContainer.transform.position = Vector3.zero;
            this.rootGameContainer.transform.localScale = Vector3.one;
        }

        public GameObject registerGameContainer(String name)
        {
            if (this.containerDic.ContainsKey(name)) return this.containerDic[name];
            GameObject container = new GameObject(name);
            container.transform.SetParent(this.rootGameContainer.transform);
            container.transform.position = Vector3.zero;
            container.transform.localScale = Vector3.one;

            this.containerDic.Add(name, container);
            return container;
        }

        public override void dispose()
        {
            foreach(KeyValuePair<String, GameObject> info in containerDic)
            {
                GameObject.Destroy(info.Value);
            }
            this.containerDic.Clear();
            base.dispose();
        }
    }
}
