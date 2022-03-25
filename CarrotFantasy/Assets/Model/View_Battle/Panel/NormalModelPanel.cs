using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace ETModel
{
    public class NormalModelPanel : BasePanel
    {
        private GameObject nodeTopPage;
        private GameObject nodeMenuPage;
        private GameObject nodeGameOverPage;
        private GameObject nodeGameWinPage;
        private GameObject nodeStartUI;
        private GameObject nodeFinalWave;

        private TopPage topPage;
        private GameWinPage gameWinPage;
        private MenuPage menuPage;
        private GameOverPage gameOverPage;

        private Button btnMenuPage;

        private int schId;

        private int schId_startGame;

        public NormalModelPanel(Dictionary<string, dynamic> param) : base(param)
        {
            this.prefabUrl = "Prefabs/Game/Panel/NormalModelPanel";
            this.isShowGray = false;
        }

        public override void init()
        {
            base.init();
            this.panelManagerUnit.registerOnAssetReady(this.OnAssetReady);
            this.panelManagerUnit.registerOnDestroy(this.OnDestroy);
        }

        protected override void OnAssetReady()
        {
            base.OnAssetReady();

            this.nodeTopPage = transform.Find("node_TopPage").gameObject;
            this.topPage = new TopPage(this.nodeTopPage.transform);

            this.nodeMenuPage = transform.Find("MenuPage").gameObject;
            this.menuPage = new MenuPage(this.nodeMenuPage.transform);

            this.nodeGameOverPage = transform.Find("GameOverPage").gameObject;
            this.gameOverPage = new GameOverPage(this.nodeGameOverPage.transform);

            this.nodeGameWinPage = transform.Find("GameWinPage").gameObject;
            this.gameWinPage = new GameWinPage(this.nodeGameWinPage.transform);

            this.nodeStartUI = transform.Find("StartUI").gameObject;

            this.btnMenuPage = this.transform.Find("node_TopPage/node_btn_container/Btn_Menu").GetComponent<Button>();

            this.topPage.init();
            this.menuPage.init();
            this.gameOverPage.init();
            this.gameWinPage.init();

            this.nodeTopPage.SetActive(true);
            this.nodeMenuPage.SetActive(false);
            this.nodeGameOverPage.SetActive(false);
            this.nodeGameWinPage.SetActive(false);

            this.addListener();

        }

        private void showMenu()
        {
            UIServer.getInstance().playButtonEffect();
            this.nodeMenuPage.SetActive(true);
            GameManager.getInstance().baseBattle.eventDispatcher.dispatchEvent(BattleEvent.PAUSE_THE_GAME);
        }

        private void addListener()
        {
            GameManager.getInstance().baseBattle.eventDispatcher.addListener(BattleEvent.START_GAME, this.showStartUI);
            this.btnMenuPage.onClick.AddListener(this.showMenu);
        }

        private void removeListener()
        {
            GameManager.getInstance().baseBattle.eventDispatcher.removeListener(BattleEvent.START_GAME, this.showStartUI);
            this.btnMenuPage.onClick.RemoveAllListeners();
        }

        private void showStartUI()
        {
            this.nodeStartUI.SetActive(true);
            BattleSchedulerComponent sche = (BattleSchedulerComponent)GameManager.getInstance().baseBattle.getComponent(BattleComponentType.SchedulerComponent);
            this.schId = sche.delayExeOnceTimes(()=> { 
                this.nodeStartUI.SetActive(false);
                UIServer.getInstance().audioManager.playEffect("AudioClips/NormalMordel/Go");
            },3.0f);
            this.schId_startGame = sche.delayExeMultipleTimes(() =>{
                UIServer.getInstance().audioManager.playEffect("AudioClips/NormalMordel/CountDown");
            },1.0f);
            sche.delayExeOnceTimes(() => {
                sche.silenceSingleSche(this.schId_startGame);
            }, 3.5f);
        }

        protected override void OnDestroy()
        {
            this.gameWinPage.dispose();
            this.gameOverPage.dispose();
            this.topPage.dispose();
            this.removeListener();
            base.OnDestroy();
        }
    }
}
