using System;
using System.Collections.Generic;

namespace ETModel
{
    public enum EventType
    {
        //状态切换
        MainMenuState,
        StartGameState,
        PlayGameState,
        EndGameState,
        ReStartGameState,

        //UI管理器
        ShowOrHideUIPanel,
        UpdateScoreText,
        AddUIPanel,
        RemoveUIPanel,
        ShowEndGamePanel,

        //对象池
        ReturnToPool,
        ReturnToDisControllerCorePool,

        //运动管理器
        RemoveOneInListMotionGameObj,
        RemoveListMotionGameObj,
        SetMotionStrategy,
        RemoveMotionStrategy,
        NewOneAndAddToGameObjectList,
        NewListAndAddToGameObjectList,
        SetGameObjectListInitPosition,
        TraverseToDo,

        //小鸟
        BirdInit,
        BirdJump,

        //操作管理器
        ChangeGameState,
        AddScore,

        ReStartInit,

    }

    public enum ReturnEventType
    {
        //对象池
        getGameObject,
        GetOneControllerCore,
    }

    
}
