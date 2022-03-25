using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace ETModel
{
    public class PveBattleView : BattleView_base
    {

        public PveBattleView(BaseBattle battle) : base(battle)
        {
            this.rootGameObject = Server.sceneServer.currentScene.gameObj;
        }

        public override void init()
        {
            this.addComponent(new BVSceneComponent(this));
            this.addComponent(new BVMapComponent(this));
            this.addComponent(new BVMonsterComponent(this));
            this.addComponent(new BVTowerComponent(this));
            this.addComponent(new BVBulletComponent(this));
            this.addComponent(new BVUIComponent(this));
        }



    }
}
