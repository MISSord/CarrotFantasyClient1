using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace ETModel
{
    public class MainPanel : BasePanel
    {
        private Animator carrotAnimator;
        private Transform monsterTrans;
        private Transform cloudTrans;
        private Tween[] mainPanelTween;//0.右，1.左
        private Tween ExitTween;//离开主页运行的动画

        private Button btnNormal;
        private Button btnBoss;
        private Button btnNetwork;

        private Button btnExitGame;
        private Button btnSet;
        private Button btnHelp;

        public MainPanel(Dictionary<string, dynamic> param) : base(param)
        {
            this.isClickGrayEnable = true;
            this.prefabUrl = "Prefabs/Business/Hud/MainPanel";
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
            //获取成员变量
            this.transform.SetSiblingIndex(8);
            this.carrotAnimator = transform.Find("node_center/node_carrot").GetComponent<Animator>();
            this.carrotAnimator.Play("CarrotGrow");
            this.monsterTrans = transform.Find("node_top/spr_monster");
            this.cloudTrans = transform.Find("node_top/spr_cloud");

            mainPanelTween = new Tween[2];
            mainPanelTween[0] = transform.DOLocalMoveX(1920, 0.5f);
            mainPanelTween[0].SetAutoKill(false);
            mainPanelTween[0].Pause();
            mainPanelTween[1] = transform.DOLocalMoveX(-1920, 0.5f);
            mainPanelTween[1].SetAutoKill(false);
            mainPanelTween[1].Pause();

            this.btnNormal = this.transform.Find("node_bottom/btn_normal").GetComponent<Button>();
            this.btnBoss = this.transform.Find("node_bottom/btn_boss").GetComponent<Button>();
            this.btnNetwork = this.transform.Find("node_bottom/btn_network").GetComponent<Button>();

            this.btnExitGame = this.transform.Find("node_top/btn_exit_game").GetComponent<Button>();
            this.btnHelp = this.transform.Find("node_center/btn_help").GetComponent<Button>();
            this.btnSet = this.transform.Find("node_center/btn_set").GetComponent<Button>();

            this.addListener();

            this.PlayUITween();
            UIServer.getInstance().playMainBg();
        }

        private void addListener()
        {
            this.btnBoss.onClick.AddListener(this.toBossModel);
            this.btnNormal.onClick.AddListener(this.toNormalModel);
            this.btnNetwork.onClick.AddListener(this.startMatch);

            this.btnExitGame.onClick.AddListener(this.exitGame);
            this.btnHelp.onClick.AddListener(this.showHelpPanel);
            this.btnSet.onClick.AddListener(this.showSetPanel);
        }

        private void removeListener()
        {

        }

        //UI动画播放
        private void PlayUITween()
        {
            this.monsterTrans.DOLocalMoveY(20, 7f).SetLoops(-1, LoopType.Yoyo);
            this.cloudTrans.DOLocalMoveX(1300, 30f).SetLoops(-1, LoopType.Restart);
        }

        public void showSetPanel()
        {
            UIServer.getInstance().playButtonEffect();
            ExitTween = mainPanelTween[0];
            Server.panelServer.showPanel(new SetPanel(null));
        }

        public void showHelpPanel()
        {
            UIServer.getInstance().playButtonEffect();
            //ExitTween = mainPanelTween[1];
            Server.panelServer.showPanel(new HelpPanel(null));
        }

        //场景状态切换的方法

        public void toNormalModel()
        {
            UIServer.getInstance().playButtonEffect();
            Server.panelServer.showPanel(new MapBigLevelPanel(null));
            //this.finish();
        }

        public void toBossModel()
        {
            UIServer.getInstance().playButtonEffect();
            //mUIFacade.currentScenePanelDict[StringManager.GameLoadPanel].EnterPanel();
            //mUIFacade.ChangeSceneState(new BossGameOptionSceneState(mUIFacade));
        }

        private void startMatch()
        {
            Server.panelServer.showPanel(new RoomPanel(null));
            RoomServer.getInstance().sendStartMatch();
            UIServer.getInstance().playButtonEffect();
        }

        public void exitGame()
        {
            UIServer.getInstance().playButtonEffect();
            Business.getInstance().eventDispatcher.dispatchEvent(CommonEventType.GAME_QUIT);
        }

        protected override void OnDestroy()
        {
            this.removeListener();
            base.OnDestroy();
        }
    }
}
