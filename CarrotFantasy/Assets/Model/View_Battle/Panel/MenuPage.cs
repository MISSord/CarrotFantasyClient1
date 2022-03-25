using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace ETModel
{
    /// <summary>
    /// 菜单页面
    /// </summary>
    public class MenuPage
    {
        private Transform transform;

        private Button btnGoOn;
        private Button btnReplay;
        private Button btnChooseLevel;

        public MenuPage(Transform node)
        {
            this.transform = node;
        }

        public void init()
        {
            this.btnGoOn = this.transform.Find("btn_go_on").GetComponent<Button>();
            this.btnReplay = this.transform.Find("btn_replay").GetComponent<Button>();
            this.btnChooseLevel = this.transform.Find("btn_choose_level").GetComponent<Button>();

            this.btnGoOn.onClick.AddListener(this.btnEvenGoOn);
            this.btnReplay.onClick.AddListener(this.btnEvenReplay);
            this.btnChooseLevel.onClick.AddListener(this.btnEvenChooseOtherLevel);
        }

        public void btnEvenGoOn()
        {
            UIServer.getInstance().playButtonEffect();
            this.transform.gameObject.SetActive(false);
            GameManager.getInstance().baseBattle.eventDispatcher.dispatchEvent(BattleEvent.PAUSE_THE_GAME);
        }

        public void btnEvenReplay()
        {
            UIServer.getInstance().playButtonEffect();
            this.transform.gameObject.SetActive(false);
            GameManager.getInstance().baseBattle.eventDispatcher.dispatchEvent(BattleEvent.REPLAY_THE_GAME);
        }

        public void btnEvenChooseOtherLevel()
        {
            UIServer.getInstance().playButtonEffect();
            this.transform.gameObject.SetActive(false);
            Business.getInstance().eventDispatcher.dispatchEvent(CommonEventType.RETURN_TO_MAIN_SCENE);
        }
    }
}

