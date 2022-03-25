using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETModel
{
    /*
    public class BattleHitTestComponent : BaseBattleComponent
    {
        
        private String TRANSFORM = UnitComponentType.TRANSFORM;
        private String STATUS = UnitComponentType.STATUS;
        private String BEHIT = UnitComponentType.BEHIT;

        private BattleMapComponent battleMapComponent;
        // BattleLogComponent
        //private Dictionary<int, Dictionary<String, dynamic>> m

        private Dictionary<int, LocatedInfo> unitUid2LocatedInfo = new Dictionary<int, LocatedInfo>();
        private Dictionary<String, List<int>> unitType2UnitUidList = new Dictionary<string, List<int>>();
        private Dictionary<int, ListenerObj> uid2ListenerObj = new Dictionary<int, ListenerObj>();
        private Dictionary<String, Dictionary<int, Dictionary<int, BattleUnit>>> unitType2Grid2BeHitUnitUids = new Dictionary<string, Dictionary<int, Dictionary<int, BattleUnit>>>();

        private Dictionary<String, Dictionary<int, ListenerObj>> unitType2Grid2ListenerObjList = new Dictionary<string, Dictionary<int, ListenerObj>>();

        private int curListenerUid;
        private Dictionary<String, Dictionary<int, bool>> unitType2ChangePositionUnitUid2True = new Dictionary<string, Dictionary<int, bool>>();
        private Dictionary<String, Dictionary<int, int>> unitType2ChangePositionUnitUids = new Dictionary<string, Dictionary<int, int>>();
        private Dictionary<int, BattleUnit> uid2RegisterUnit = new Dictionary<int, BattleUnit>();
        private Dictionary<ListenerObj, String> listenerObjUnitTypes = new Dictionary<ListenerObj, string>();

        public BattleHitTestComponent(BaseBattle bBattle) : base(bBattle)
        {

        }

        public override void init()
        {
            this.battleMapComponent = (BattleMapComponent)this.baseBattle.getComponent(BattleComponentType.MapComponent);
            //this.battleLogComponent

            //this.baseBattle.eventDispatcher.addListener(BattleEvent.ITEM_BUILD,);
            //this.baseBattle.eventDispatcher.addListener(BattleEvent.ITEM_DESTROY,);
        }


        public V4 getShapeLocatedCoordRange(HitTestShape_Base shape)
        {
            BattleMapComponent battleMapComponent = this.battleMapComponent;
            V2 info = battleMapComponent.getSize();
            float minCoordX = battleMapComponent.getCoordByPosition(Math.Max(shape.boundsX, 0), Math.Max(shape.boundsY, 0)).x;
            float maxCoordX = battleMapComponent.getCoordByPosition(Math.Min(shape.boundsX + shape.boundsSizeX, info.x), Math.Max(shape.boundsY, 0)).x;
            float minCoordY = battleMapComponent.getCoordByPosition(Math.Max(shape.boundsX, 0), Math.Max(shape.boundsY, 0)).y;
            float maxCoordY = battleMapComponent.getCoordByPosition(Math.Max(shape.boundsX, 0), Math.Min(shape.boundsY + shape.boundsSizeY, info.y)).y;
            return new V4(minCoordX, maxCoordX, minCoordY, maxCoordY);
        }

        public LocatedInfo getShapeLocatedInfo(HitTestShape_Base shape)
        {
            V4 info = this.getShapeLocatedCoordRange(shape);
            List<BattleMapGrid> locatedGrids = new List<BattleMapGrid>();
            for (float i = info.x1; i <= info.x2; i++)
            {
                for (float j = info.x3; i <= info.x4; j++)
                {
                    BattleMapGrid grid = this.battleMapComponent.getGridByCoord(i, j);
                }
            }
            return new LocatedInfo(info.x1, info.x2, info.x3, info.x4, locatedGrids);
        }


        public void remoevUnitLocatedInfo(int unitUid)
        {
            BattleUnit unit = this.uid2RegisterUnit[unitUid];
            String unitType = unit.getType();
            LocatedInfo oldLocatedInfo = this.unitUid2LocatedInfo[unitUid];
            this.unitUid2LocatedInfo[unitUid] = null;
            if (oldLocatedInfo != null)
            {
                Dictionary<int, Dictionary<int, BattleUnit>> gridUid2BeHitUnitUids = this.unitType2Grid2BeHitUnitUids[unitType];
                for (int i = 0; i <= oldLocatedInfo.locatedGrids.Count - 1; i++)
                {
                    int id = oldLocatedInfo.locatedGrids[i].uid;
                    Dictionary<int, BattleUnit> unitUidList = gridUid2BeHitUnitUids[id];
                    unitUidList.Remove(unitUid);
                }
            }
        }


        public void updateUnitLocatedInfo(int unitUid)
        {
            BattleUnit unit = this.uid2RegisterUnit[unitUid];
            String unitType = unit.getType();

            LocatedInfo oldLocatedInfo = this.unitUid2LocatedInfo[unitUid];
            Dictionary<int, Dictionary<int, BattleUnit>> gridUid2BeHitUnitUids = this.unitType2Grid2BeHitUnitUids[unitType];

            if (gridUid2BeHitUnitUids != null)
            {
                gridUid2BeHitUnitUids = new Dictionary<int, Dictionary<int, BattleUnit>>();
                this.unitType2Grid2BeHitUnitUids[unitType] = gridUid2BeHitUnitUids;
            }
            UnitTransformComponent transformComponent = (UnitTransformComponent)unit.getComponent(TRANSFORM);
            HitTestShape_Base bodyHitTestShape = transformComponent.bodyHitTestShape;

            V4 info = this.getShapeLocatedCoordRange(bodyHitTestShape);
            if(oldLocatedInfo != null && info.x1 == oldLocatedInfo.minCoordX && info.x2 == oldLocatedInfo.maxCoordX 
                && info.x3 == oldLocatedInfo.minCoordY && info.x4 == oldLocatedInfo.maxCoordY)
            {
                return;
            }

            LocatedInfo curLocatedInfo =  this.getShapeLocatedInfo(bodyHitTestShape);
            Dictionary<BattleMapGrid, bool> ignoreGrid2True = new Dictionary<BattleMapGrid, bool>();
            if(oldLocatedInfo != null)
            {

            }
        }
    }*/
}
/*


public void onUnitPositionChange(event)
local unit = event.target
local unitUid = unit.uid
local unitType = unit.getType()
this.addUnitChangePositionRecord(unitUid)

}

public void onHeroBuild(event)
local params = event.params
local unit = params.hero
local unitType = unit.getType()
if( unitType ~= modelbattle.BattleUnitType.MONSTER {
    return
}
this.registerUnit(unit)
}

public void registerUnit(unit)
local unitUid = unit.uid
local unitType = unit.getType()
local unitUidList = this.unitType2UnitUidList [unitType]
if( not unitUidList {
    unitUidList = modelbattle.LinkedList.create()
    this.unitType2UnitUidList [unitType] = unitUidList
}
unitUidList.addLast(unitUid)
this.uid2RegisterUnit [unitUid] = unit
unit.eventDispatcher.addListener(modelbattle.UnitEvent.POSITION_CHANGE, this, this.onUnitPositionChange, 10000)

this.addUnitChangePositionRecord(unitUid)
if( unit.getType() == modelbattle.BattleUnitType.MONSTER {
    this.updateMonsterMidPointLocatedGrids(unit)
}
-- this.battleLogComponent.logFormatWithHero("碰撞检测", unit, "registerUnit.\n%s", debug.traceback())
}

public void addUnitChangePositionRecord(unitUid)
local unit = this.uid2RegisterUnit [unitUid]
local unitType = unit.getType()
local changePositionUnitUid2True = this.unitType2ChangePositionUnitUid2True [unitType]
if( not changePositionUnitUid2True ){
    changePositionUnitUid2True = { }
this.unitType2ChangePositionUnitUid2True [unitType] = changePositionUnitUid2True
}
if( changePositionUnitUid2True [unitUid]
{
    return
}
changePositionUnitUid2True [unitUid] = true
local changePositinUnitUids = this.unitType2ChangePositionUnitUids [unitType]
if( not changePositinUnitUids {
    changePositinUnitUids = { }
this.unitType2ChangePositionUnitUids [unitType] = changePositinUnitUids
}
table.insert(changePositinUnitUids, unit.uid)
}

public void removeUnitChangePositionRecord(unitUid)
local unit = this.uid2RegisterUnit [unitUid]
local unitType = unit.getType()
local changePositionUnitUid2True = this.unitType2ChangePositionUnitUid2True [unitType]
if( not changePositionUnitUid2True or not changePositionUnitUid2True [unitUid]
{
    return
}
changePositionUnitUid2True [unitUid] = nil
local changePositionUnitUids = this.unitType2ChangePositionUnitUids [unitType]
table.removebyvalue(changePositionUnitUids, unitUid)
}

public void onHeroDied(event)
local params = event.params
local unit = params.hero
local unitType = unit.getType()
if( unitType ~= modelbattle.BattleUnitType.MONSTER {
    return
}
this.unregisterUnit(unit)
}

public void unregisterUnit(unit)
local unitUid = unit.uid
assert(this.uid2RegisterUnit [unitUid], "invalid unit uid.", unitUid)
local unitType = unit.getType()
local unitUidList = this.unitType2UnitUidList [unitType]
unitUidList.remove(unitUid)
this.removeUnitChangePositionRecord(unitUid)
this. remoevUnitLocatedInfo(unitUid)
this.uid2RegisterUnit[unitUid] = nil
unit.eventDispatcher.removeListener(modelbattle.UnitEvent.POSITION_CHANGE, this, this.onUnitPositionChange)
assert(not this.uid2RegisterUnit[unitUid], "invalid unit uid.", unitUid)
if( unit.getType() == modelbattle.BattleUnitType.MONSTER {
    this.monsterUid2LocatedGridOfMidPoint[unit.uid] = nil
}
-- this.battleLogComponent.logFormatWithHero("碰撞检测", unit, "unregisterUnit.\n%s", debug.traceback())
}

public void  addHitTestListener(shape, unitType, preFilterFunc, postFilterFunc, callback)
local locatedInfo = this.getShapeLocatedInfo(shape)
local listenerObj = { shape = shape, unitType = unitType, callback = callback, locatedInfo = locatedInfo, preFilterFunc = preFilterFunc, postFilterFunc = postFilterFunc }
local grid2ListenerObjList = this.unitType2Grid2ListenerObjList[unitType]
if( not grid2ListenerObjList {
    grid2ListenerObjList = { }
this.unitType2Grid2ListenerObjList[unitType] = grid2ListenerObjList
}
for i, grid in ipairs(locatedInfo.locatedGrids) do
        local listenerObjList = grid2ListenerObjList[grid]
    if( not listenerObjList {
        listenerObjList = modelbattle.LinkedList.create()
        grid2ListenerObjList[grid] = listenerObjList
    }
    listenerObjList.addLast(listenerObj)
}
local uid = this.curListenerUid
this.curListenerUid = this.curListenerUid + 1
this.uid2ListenerObj[uid] = listenerObj

local listenerObjList = this.unitType2ListenerObjList[unitType]
if( not listenerObjList {
    listenerObjList = modelbattle.LinkedList.create()
    this.unitType2ListenerObjList[unitType] = listenerObjList
    table.insert(this.listenerObjUnitTypes, unitType)
}
listenerObjList.addLast(listenerObj)


return uid
}

public void  removeHitTestListener(uid)
local listenerObj = this.uid2ListenerObj[uid]
if( not listenerObj {
    return
}
local unitType = listenerObj.unitType
for i, grid in ipairs(listenerObj.locatedInfo.locatedGrids) do
        this.unitType2Grid2ListenerObjList[unitType][grid].remove(listenerObj)
}
this.uid2ListenerObj[uid] = nil
this.unitType2ListenerObjList[unitType].remove(listenerObj)
}

public void  getListenerCountByUnitType(unitType)
local listenerObjList = this.unitType2ListenerObjList[unitType]
return listenerObjList and listenerObjList.size or 0
}

local public isUnitHitTestEnabled(unit)
local statusComponent = unit.getComponent(STATUS)
local isUnselectable = false
if( statusComponent {
    isUnselectable = statusComponent.getStatus(UNSELECTABLE)
}
return not isUnselectable
}

public void listenerObjsHitTestUnits(unitType)
local battle = this.battle
local listenerObjList = this.unitType2ListenerObjList[unitType]
local gridUid2BeHitUnitUids = this.unitType2Grid2BeHitUnitUids[unitType]
if( not gridUid2BeHitUnitUids {
    return
}
local shouldCallbackListenerObjs = { }
local listenerObj2HitedUnits = { }
listenerObjList. forEach(public(listenerObj)

     local hitedUnits = { }
local unitUid2IsHandled = { }
    for i, locatedGrid in ipairs(listenerObj.locatedInfo.locatedGrids) do
        local beHitUnitUidList = gridUid2BeHitUnitUids[locatedGrid.uid]
        if( beHitUnitUidList and beHitUnitUidList.size > 0 {
            beHitUnitUidList.forEach(public(unitUid)
                local unit = this.uid2RegisterUnit[unitUid]
                if( unit and not unitUid2IsHandled[unit.uid] {
                    unitUid2IsHandled[unit.uid] = true
                    local isHitTestEnabled = isUnitHitTestEnabled(unit)
                    if( isHitTestEnabled and(not listenerObj.preFilterFunc or listenerObj.preFilterFunc(unit)) {
                       local bodyHitTestShape = unit.getComponent(TRANSFORM).bodyHitTestShape
                        local strBodyHitTestShape = bodyHitTestShape.strDesc
                        local isHited = HitTestUtil.hitTest(bodyHitTestShape, listenerObj.shape)
                        this.battleLogComponent.logFormatWithHero("碰撞检测", unit, "isHited.%s unit shape.%s listener shape.%s", isHited, strBodyHitTestShape, listenerObj.shape.strDesc)
                        if( isHited {
                            if( not listenerObj.postFilterFunc or listenerObj.postFilterFunc(unit) {
                                table.insert(hitedUnits, unit)
                            }
                        }
                    }
                }
            })
        }
    }
    if( #hitedUnits > 0 {
        table.insert(shouldCallbackListenerObjs, listenerObj)
        listenerObj2HitedUnits[listenerObj] = hitedUnits
    }
})
for i, listenerObj in ipairs(shouldCallbackListenerObjs) do
        local hitedUnits = listenerObj2HitedUnits[listenerObj]

     listenerObj.callback(hitedUnits)
}
}

public void unitsHitTestListenerObjs(unitType)
local startClock = os.clock()
local hitTestCount = 0
local battle = this.battle
local battleLogComponent = this.battleLogComponent
local unitUidList = this.unitType2UnitUidList[unitType]
local grid2ListenerObjList = this.unitType2Grid2ListenerObjList[unitType]
if( not grid2ListenerObjList {
    return
}
local shouldCallbackListenerObjs = { }
local listenerObj2HitedUnits = { }
unitUidList. forEach(public(unitUid)

     local unit = this.uid2RegisterUnit[unitUid]

     local bodyHitTestShape = unit.getComponent(TRANSFORM).bodyHitTestShape

     local isHitTestEnabled = isUnitHitTestEnabled(unit)
    if( isHitTestEnabled {
        local listenerObj2IsHandled = { }
local locatedInfo = this.unitUid2LocatedInfo[unitUid]
        if( locatedInfo {
            for i, locatedGrid in ipairs(locatedInfo.locatedGrids) do
        local listenerObjList = grid2ListenerObjList[locatedGrid]
                if( listenerObjList and listenerObjList.size > 0 {
                    listenerObjList.forEach(public(listenerObj)
                        if( not listenerObj2IsHandled[listenerObj] {
                            listenerObj2IsHandled[listenerObj] = true
                            if( not listenerObj.preFilterFunc or listenerObj.preFilterFunc(unit) {
                                local strBodyHitTestShape = bodyHitTestShape.strDesc
                                local isHited = HitTestUtil.hitTest(bodyHitTestShape, listenerObj.shape)
                                hitTestCount = hitTestCount + 1
                                battleLogComponent. logFormatWithHero("碰撞检测", unit, "isHited.%s unit shape.%s listener shape.%s", isHited, strBodyHitTestShape, listenerObj.shape.strDesc)
                                if( isHited {
                                    if( not listenerObj.postFilterFunc or listenerObj.postFilterFunc(unit) {
                                        local hitedUnits = listenerObj2HitedUnits[listenerObj]
                                        if( not hitedUnits {
                                            hitedUnits = { }
table.insert(shouldCallbackListenerObjs, listenerObj)
                                            listenerObj2HitedUnits[listenerObj] = hitedUnits
                                        }
                                        table.insert(hitedUnits, unit)
                                    }
                                }
                            }
                        }
                    })
                }
            }
        }
    }
})
--print("before callback.", os.clock() - startClock, "hitTestCount", hitTestCount)
for i, listenerObj in ipairs(shouldCallbackListenerObjs) do
        local hitedUnits = listenerObj2HitedUnits[listenerObj]

     listenerObj.callback(hitedUnits)
}
-- print("after callback.", os.clock() - startClock, "callbackCount.", callbackCount)
}

public void  handleHitTestByUnitType(unitType)
local listenerObjList = this.unitType2ListenerObjList[unitType]
if( not listenerObjList or listenerObjList.size <= 0 {
    return
}
local unitUidList = this.unitType2UnitUidList[unitType]
if( not unitUidList or unitUidList.size <= 0 {
    return
}


local startClock = os.clock()
if( unitUidList.size < listenerObjList.size {
    this.unitsHitTestListenerObjs(unitType)
else
this. listenerObjsHitTestUnits(unitType)
}
-- print("unit count.", unitUidList.size, "listener count.", listenerObjList.size, "cost.", os.clock() - startClock)
}

public void  onTick(dt)
for i, unitType in ipairs(this.listenerObjUnitTypes) do
        local listenerObjList = this.unitType2ListenerObjList[unitType]
    if( listenerObjList.size > 0 {
        this.handleHitTestByUnitType(unitType)
    }
}
}

public void  lateTick(dt)
local battle = this.battle
for i, unitType in ipairs(this.listenerObjUnitTypes) do
        local listenerObjList = this.unitType2ListenerObjList[unitType]
    if( listenerObjList.size > 0 and this.unitType2ChangePositionUnitUids[unitType] {
        local changePositionUnitUids = this.unitType2ChangePositionUnitUids[unitType]
        for _, unitUid in ipairs(changePositionUnitUids) do
    this. updateUnitLocatedInfo(unitUid)
        }
        this.unitType2ChangePositionUnitUids[unitType] = { }
this.unitType2ChangePositionUnitUid2True[unitType] = { }
}
}
}

public void  getRegisteredUnit(unitUid)
return this.uid2RegisterUnit[unitUid]
}

public void  dispose()
this.battle.eventDispatcher.removeListener(modelbattle.BattleEvent.HERO_BUILD, this, this.onHeroBuild)
this.battle.eventDispatcher.removeListener(modelbattle.BattleEvent.HERO_DIED, this, this.onHeroDied)
}

public void  getType()
return modelbattle.BattleComponentType.HIT_TEST
}
}
}

    */
