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

        }

        public void initBattle()
        {
            this.baseBattle.init();
            this.baseBattle.initComponent();
            this.baseBattleView.init();
            this.baseBattleView.initComponents();

            this.addLitener();
        }

        public void startGame()
        {
            this.baseBattle.startGame();
            this.baseBattleView.startGame();
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
        }
    }
}
