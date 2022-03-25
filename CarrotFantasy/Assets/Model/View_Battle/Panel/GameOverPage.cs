using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ETModel
{
    /// <summary>
    /// 游戏失败结束页面
    /// </summary>
    public class GameOverPage
    {
        private BattleDataComponent dataComponent;

        private Transform transform;

        private Text txtResultShow;
        private Text txtLevelShow;

        private Button btnReplay;
        private Button btnChooseLevel;

        public GameOverPage(Transform node)
        {
            this.transform = node;
            this.dataComponent = (BattleDataComponent)GameManager.getInstance().baseBattle.getComponent(BattleComponentType.DataComponent);
        }

        public void init()
        {
            this.txtResultShow = this.transform.Find("txt_result_show").GetComponent<Text>();
            this.txtLevelShow = this.transform.Find("txt_level_show").GetComponent<Text>();

            this.btnReplay = this.transform.Find("btn_replay").GetComponent<Button>();
            this.btnChooseLevel = this.transform.Find("btn_choose_level").GetComponent<Button>();

            this.addListener();
        }

        private void addListener()
        {
            this.btnReplay.onClick.AddListener(this.btnEvenReplay);
            this.btnChooseLevel.onClick.AddListener(this.btnEvenChooseOtherLevel);

            this.dataComponent.eventDispatcher.addListener(BattleEvent.SHOW_GAME_OVER_PAGE, this.showGameOverPage);
        }

        private void removeListener()
        {
            this.btnReplay.onClick.RemoveAllListeners();
            this.btnChooseLevel.onClick.RemoveAllListeners();
            this.dataComponent.eventDispatcher.removeListener(BattleEvent.SHOW_GAME_OVER_PAGE, this.showGameOverPage);
        }

        public void showGameOverPage()
        {
            this.transform.gameObject.SetActive(true);
            UIServer.getInstance().audioManager.playEffect("AudioClips/NormalMordel/Lose");
            int waves = dataComponent.curWaves;
            this.txtResultShow.text = LanguageUtil.getInstance().getFormatString(1002, (waves / 10).ToString(), (waves % 10).ToString(), dataComponent.totalWaves.ToString());
            this.txtLevelShow.text = LanguageUtil.getInstance().getFormatString(1003, dataComponent.bigLevel.ToString(), dataComponent.level.ToString());
        }

        public void btnEvenReplay()
        {
            GameManager.getInstance().baseBattle.eventDispatcher.dispatchEvent(BattleEvent.REPLAY_THE_GAME);
        }

        public void btnEvenChooseOtherLevel()
        {
            Business.getInstance().eventDispatcher.dispatchEvent(CommonEventType.RETURN_TO_MAIN_SCENE);
        }

        public void dispose()
        {
            this.removeListener();
        }
    }
}

