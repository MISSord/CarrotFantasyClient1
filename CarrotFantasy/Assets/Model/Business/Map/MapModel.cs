using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ETModel
{
    public class MapModel
    {
        public EventDispatcher eventDispatcher;
        private int levelCount;
        private SingleMapInfo[] allMapInfo;
        private Dictionary<int, BigLevelInfo> bigLevelInfo;

        public MapModel(EventDispatcher eventDis)
        {
            this.eventDispatcher = eventDis;
            this.bigLevelInfo = new Dictionary<int, BigLevelInfo>();
            MapConfigReader.initConfig();
        }

        public void parseMapInfo(String mapInfo)
        {
            String[] map = mapInfo.Split('#');
            this.levelCount = map.Length;
            this.allMapInfo = new SingleMapInfo[this.levelCount - 1];
            this.bigLevelInfo.Clear();
            for(int i = 1; i < map.Length; i++)
            {
                this.parseSingleMapInfo(map[i]);
            }
            this.initBigLevelMapInfo();
            this.eventDispatcher.dispatchEvent(MapEventType.MAP_INFO_CHANGE);
        }

        private void parseSingleMapInfo(String singleMapInfo)
        {
            String[] mapInfo = singleMapInfo.Split(',');
            SingleMapInfo info = new SingleMapInfo();
            info.bigLevelId = int.Parse(mapInfo[0]);
            info.levelId = int.Parse(mapInfo[1]);
            info.carrotState = int.Parse(mapInfo[2]);
            info.isAllClear = int.Parse(mapInfo[3]);
            info.unLocked = int.Parse(mapInfo[4]);

            this.allMapInfo[this.getMapNumber(info)] = info;
        }

        private void initBigLevelMapInfo()
        {
            BigLevelInfo info;
            for (int i = 0; i < this.allMapInfo.Length; i++)
            {
                if (!this.bigLevelInfo.TryGetValue(this.allMapInfo[i].bigLevelId, out info))
                {
                    info = new BigLevelInfo();
                    info.bigLevel = this.allMapInfo[i].bigLevelId;
                    info.isLock = true;
                    bigLevelInfo.Add(this.allMapInfo[i].bigLevelId, info);
                }
                if (this.allMapInfo[i].unLocked == MapInfoType.UNLOCK_LEVEL)
                {
                    info.unlockCount += 1;
                    info.isLock = false;
                }
                info.count += 1;
            }
        }

        private void refreshBigLevelMapInfo(int bigLevel)
        {
            BigLevelInfo levelInfo = this.bigLevelInfo[bigLevel];
            levelInfo.unlockCount = 0;
            levelInfo.count = 0;
            levelInfo.isLock = true;
            for (int i = 0; i < allMapInfo.Length; i++)
            {
                if (allMapInfo[i].bigLevelId == bigLevel)
                {
                    levelInfo.count += 1;
                    if (allMapInfo[i].unLocked == MapInfoType.UNLOCK_LEVEL)
                    {
                        levelInfo.isLock = false;
                        levelInfo.unlockCount += 1;
                    }
                }
            }
        }

        private int getMapNumber(SingleMapInfo info)
        {
            return this.getMapNumber(info.bigLevelId, info.levelId);
        }

        private int getMapNumber(int bigLevel, int level)
        {
            return (bigLevel - 1) * 5 + (level - 1);
        }

        public void updateSingleMapInfo(SingleMapInfo unSave)
        {
            if(unSave != null)
            {
                SingleMapInfo info = this.allMapInfo[this.getMapNumber(unSave)];
                info.carrotState = info.carrotState >= unSave.carrotState ? info.carrotState : unSave.carrotState;
                info.isAllClear = info.isAllClear <= unSave.isAllClear ? info.isAllClear : unSave.isAllClear;
                this.allMapInfo[this.getMapNumber(unSave)] = info;
                this.refreshBigLevelMapInfo(unSave.bigLevelId);
            }
            else
            {
                Debug.Log("更新单个地图信息有错误");
            }
        }

        public void updateSingleMapInfoUnLockState(int bigLevel, int level, int unLock)
        {
            if (bigLevel == 0 || level == 0)
            {
                Debug.Log("开完全部地图了");
                return;
            }
            this.allMapInfo[this.getMapNumber(bigLevel, level)].unLocked = unLock;
            this.refreshBigLevelMapInfo(bigLevel);
            this.eventDispatcher.dispatchEvent(MapEventType.MAP_INFO_CHANGE);
        }

        public SingleMapInfo[] getOnceBigLevelMapInfo(int level)
        {
            SingleMapInfo[] mapInfoList = new SingleMapInfo[5];
            for(int i = 0; i < allMapInfo.Length; i++)
            {
                if(allMapInfo[i].bigLevelId == level)
                {
                    mapInfoList[allMapInfo[i].levelId - 1] = allMapInfo[i];
                }
            }
            return mapInfoList;
        }

        public BigLevelInfo getBigLevelInfo(int bigLevel)
        {
            return this.bigLevelInfo[bigLevel];
        }

        public int getBigLevelCount()
        {
            return this.bigLevelInfo.Count;
        }

        public Stage getStage(int bigLevel, int level)
        {
            return MapConfigReader.getSingleStage(bigLevel, level);
        }

        public SingleMapInfo getSingleMapInfo(int bigLevel, int level)
        {
            return this.allMapInfo[this.getMapNumber(bigLevel, level)];
        }

        public void dispose()
        {
            this.allMapInfo = null;
            this.bigLevelInfo.Clear();
            this.eventDispatcher.dispose();
        }
    }
}
