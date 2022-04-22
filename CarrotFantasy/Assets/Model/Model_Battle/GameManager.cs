using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ETModel
{
    public class GameManager : MonoBehaviour //驱动
    {
        private static GameManager instance;

        private NormalModelPanel panel;

        public static GameManager getInstance()
        {
            return instance;
        }

        public BaseBattle baseBattle { get; private set; }
        public BattleView_base baseBattleView { get; private set; }

        private void Awake()
        {
            instance = this;
        }

        public void init()
        {
            UIServer.getInstance().audioManager.playMusic("AudioClips/NormalMordel/BGMusic");
            if (BattleParamServer.getInstance().isPVE == true)
            {
                this.baseBattle = new PveBattle();
                this.baseBattleView = new PveBattleView(this.baseBattle);
            }
            else
            {
                //this.baseBattle = new PvpBattle();
                //this.baseBattleView = new PveBattleView(this.baseBattle);
            }
            this.baseBattleView.rootGameObject = this.transform.gameObject;
            this.addLitener();
        }

        private void addLitener()
        {
            this.baseBattle.eventDispatcher.addListener(BattleEvent.REPLAY_THE_GAME, this.restartGame);
        }

        private void removeListener()
        {
            this.baseBattle.eventDispatcher.removeListener(BattleEvent.REPLAY_THE_GAME, this.restartGame);
        }

        private void restartGame()
        {
            UIServer.getInstance().showLoadingPanel();
            this.baseBattleView.clearGameInfo();
            this.baseBattle.clearGameInfo();


            if (this.panel != null)
            {
                this.panel.finish();
                this.panel = null;
            }

            this.initBattle();
            Sche.delayExeOnceTimes(this.startGame, 2.0f);
        }

        public void initBattle()
        {
            this.baseBattle.init();
            this.baseBattle.initComponent();
            this.baseBattleView.init();
            this.baseBattleView.initComponents();

            this.panel = new NormalModelPanel(null);
            Server.panelServer.showPanel(this.panel);
        }

        public void startGame()
        {
            this.baseBattle.startGame();
            this.baseBattleView.startGame();
            UIServer.getInstance().fadeLoadingPanel();
        }

        public void Update()
        {
            this.baseBattle.tick(new Fix64(Time.deltaTime));
            this.baseBattleView.onTick(Time.deltaTime);
        }

        public void dispose()
        {
            this.removeListener();
            this.baseBattleView.dispose();
            this.baseBattle.dispose();

            this.baseBattleView = null;
            this.baseBattle = null;

            UIServer.getInstance().showLoadingPanel();
        }
    }
}
