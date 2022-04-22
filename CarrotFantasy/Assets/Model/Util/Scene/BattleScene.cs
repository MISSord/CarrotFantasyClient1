using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETModel
{
    public class BattleScene : BaseScene
    {
        public BattleScene(BaseSceneType type, string name, Dictionary<string, dynamic> param) : base(type, name, param)
        {
            this.prefabUrl = null;
        }

        public override void init()
        {
            base.init();
            this.addListener();

            GameManager manager = this.gameObj.AddComponent<GameManager>();
            manager.init();
            manager.initBattle();

            Sche.delayExeOnceTimes(manager.startGame, 2.0f);
        }

        private void addListener()
        {
            Business.getInstance().eventDispatcher.addListener(CommonEventType.RETURN_TO_MAIN_SCENE, this.returnToMainScene);
        }

        private void removeListener()
        {
            Business.getInstance().eventDispatcher.removeListener(CommonEventType.RETURN_TO_MAIN_SCENE, this.returnToMainScene);
        }

        private void returnToMainScene()
        {
            Server.sceneServer.loadScene(BaseSceneType.MainScene, null);
        }

        public override void dispose()
        {
            GameManager.getInstance().dispose();
            this.removeListener();
            base.dispose();
        }
    }
}
