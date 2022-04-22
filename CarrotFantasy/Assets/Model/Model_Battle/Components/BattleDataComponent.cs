using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ETModel 
{
    public class BattleDataComponent : BaseBattleComponent
    {
        public int CoinCount { get; private set; } //金币数量
        public int curWaves { get; private set; }  //当前怪物波次
        public bool isDouleSpeed { get; private set; } //是否是双倍速
        public int bigLevel { get; private set; }
        public int level { get; private set; }
        public int totalWaves { get; private set; }
        public int carrotLive { get; private set; }
        public int[] curTowerIDList { get; private set; }
        public int towerIDListLength { get; private set; }
        public int yRow { get; private set; }
        public int xColumn { get; private set; }

        private bool isFinishGame = false;

        public BattleDataComponent(BaseBattle bBattle) : base(bBattle)
        {
            this.componentType = BattleComponentType.DataComponent;
        }

        public override void init()
        {
            if(BattleParamServer.getInstance().isPVE == true)
            {
                this.bigLevel = BattleParamServer.getInstance().curBigLevel;
                this.level = BattleParamServer.getInstance().curLevel;

                this.totalWaves = BattleParamServer.getInstance().curStage.mTotalRound;
                this.curTowerIDList = BattleParamServer.getInstance().curStage.mTowerIDList;
                this.towerIDListLength = this.curTowerIDList.Length;
            }
            else
            {

            }
            this.CoinCount = 800;
            this.curWaves = 0;
            this.isDouleSpeed = false;

            this.xColumn = 12;
            this.yRow = 8;

            this.carrotLive = 10;
            this.addListener();
        }

        private void addListener()
        {
            this.eventDispatcher.addListener<int>(BattleEvent.COIN_CHANGE, this.CoinCountNumberChange);
            this.eventDispatcher.addListener(BattleEvent.CARROT_LIVE_REDUCE, this.CarrotLiveReduce);
        }

        private void removeListener()
        {
            this.eventDispatcher.removeListener<int>(BattleEvent.COIN_CHANGE, this.CoinCountNumberChange);
            this.eventDispatcher.removeListener(BattleEvent.CARROT_LIVE_REDUCE, this.CarrotLiveReduce);
        }

        private void CoinCountNumberChange(int change)
        {
            if(this.CoinCount + change >= 0)
            {
                this.CoinCount += change;
            }
            else
            {
                Debug.LogError(String.Format("数量扣除不合法，原{0},改变{1}",this.CoinCount, change));
            }
        }

        public void WavesNumberChange()
        {
            this.curWaves += 1;
            this.eventDispatcher.dispatchEvent<int>(BattleEvent.WAVES_NUMBER_ADD, this.curWaves); //主要是面板
        }

        private void CarrotLiveReduce()
        {
            this.carrotLive -= 1;
        }

        public bool CarrotIsDead()
        {
            if(this.carrotLive <= 0)
            {
                return true;
            }
            return false;
        }

        public void gameOverByCarrotDead()
        {
            this.isFinishGame = false;
            this.baseBattle.eventDispatcher.dispatchEvent(BattleEvent.PAUSE_THE_GAME);
            this.eventDispatcher.dispatchEvent(BattleEvent.SHOW_GAME_OVER_PAGE);
        }

        public void gameOverByMonsterDead()
        {
            this.isFinishGame = true;
            SingleMapInfo unSaveMapInfo = new SingleMapInfo();
            unSaveMapInfo.bigLevelId = this.bigLevel;
            unSaveMapInfo.levelId = this.level;

            BattleItemComponent itemComponent = (BattleItemComponent)this.baseBattle.getComponent(BattleComponentType.ItemComponent);
            if(itemComponent.battleItemList.Count == 0)
            {
                unSaveMapInfo.isAllClear = MapInfoType.ALL_CLEAR;
            }
            else
            {
                unSaveMapInfo.isAllClear = MapInfoType.NOT_ALL_CLEAR;
            }
            unSaveMapInfo.carrotState = this.carrotTropyLevel();
            unSaveMapInfo.unLocked = MapInfoType.UNLOCK_LEVEL;

            MapServer.getInstance().sendSetSingleMapInfo(unSaveMapInfo);

            this.baseBattle.eventDispatcher.dispatchEvent(BattleEvent.PAUSE_THE_GAME);
            this.eventDispatcher.dispatchEvent(BattleEvent.SHOW_GAME_FINISH_PAGE);
        }

        public int carrotTropyLevel()
        {
            if (this.carrotLive >= 7)
            {
                return 3;
            }
            else if (this.carrotLive >= 3)
            {
                return 2;
            }
            else
            {
                return 1;
            }
        }

        public override void clearInfo()
        {
            this.removeListener();
        }

        public override void dispose()
        {
            this.clearInfo();
            base.dispose();
        }
    }
}
