using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace ETModel
{
	public class GameMain : MonoBehaviour  //游戏开始脚本(作为业务层，游戏层可能有所调整)
	{
		private void Awake()
        {
			//开始游戏前的工作
			Screen.sleepTimeout = SleepTimeout.NeverSleep;

			//加载底层模块和逻辑
			this.StartAsync().Coroutine();
			UIUtil.getInstance().init();
			Server.getInstance().init();

			//加载业务
			Business.getInstance().init();
			Business.getInstance().loadBusiness();

			//加载登录场景
			Server.sceneServer.loadScene(BaseSceneType.MainScene, null);


			//发送各业务的信息请求
			//获取后加载各业务
			//主场景

			//战斗场景
			
		}

		private void Update()
		{
			OneThreadSynchronizationContext.Instance.Update();
			Game.EventSystem.Update();
			Sche.tick(new Fix64(Time.deltaTime));
		}

		private void LateUpdate()
		{
			Game.EventSystem.LateUpdate();
		}

		private void OnApplicationQuit()
		{
			Game.Close();
		}

		private async ETVoid StartAsync()
		{
			try
			{
				SynchronizationContext.SetSynchronizationContext(OneThreadSynchronizationContext.Instance);

				DontDestroyOnLoad(gameObject);
				ClientConfigHelper.SetConfigHelper();
				Game.EventSystem.Add(DLLType.Core, typeof(Core).Assembly);
				Game.EventSystem.Add(DLLType.Model, typeof(GameMain).Assembly);

				Game.Scene.AddComponent<GlobalConfigComponent>(); //web资源服务器设置组件
				Game.Scene.AddComponent<ResourcesComponent>(); //资源加载组件
				Game.Scene.AddComponent<OpcodeTypeComponent>();
				Game.Scene.AddComponent<NetOuterComponent>();
				

				//测试输出正确加载了Config所带的信息
				//ETModel.Game.Scene.GetComponent<ResourcesComponent>().LoadBundle("config.unity3d");
				//Game.Scene.AddComponent<ConfigComponent>();
				//ETModel.Game.Scene.GetComponent<ResourcesComponent>().UnloadBundle("config.unity3d");
				//UnitConfig unitConfig = (UnitConfig)Game.Scene.GetComponent<ConfigComponent>().Get(typeof(UnitConfig), 1001);
				//Log.Debug($"config {JsonHelper.ToJson(unitConfig)}");

			}
			catch (Exception e)
			{
				Log.Error(e);
			}
		}
	}
}