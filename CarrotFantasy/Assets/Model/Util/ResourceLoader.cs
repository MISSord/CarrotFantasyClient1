using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ETModel
{
    public class ResourceLoader
    {
        private static ResourceLoader _resourceLoader;
        private static ResourcesComponent resourcesComponent;
        public static ResourceLoader getInstance()
        {
            if(_resourceLoader == null)
            {
                _resourceLoader = new ResourceLoader();
                resourcesComponent = Game.Scene.GetComponent<ResourcesComponent>();
            }
            return _resourceLoader;
        }
        /*
        /// <summary>
        /// 获取未实例化的游戏物体
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public GameObject getGameObject(string path)
        {
            try
            {
                GameObject bundleGameObject = this.loadRes<GameObject>(path);
                return bundleGameObject;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }

        public void UnloadBundle(string path)
        {
            if(resourcesComponent != null)
            {
                resourcesComponent.UnloadBundle($"{path}.unity3d");
            }
            else
            {
                //组件丢失
            }
        }

        /// <summary>
        /// 获取非游戏物体的资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public T loadRes<T>(String path) where T:UnityEngine.Object
        {
            try
            {
                //获取资源
                T bundle = (T)resourcesComponent.GetAsset($"{path}.unity3d", $"{path}");
                if (bundle == null)
                {
                    //加载AB包
                    resourcesComponent.LoadBundle($"{path}.unity3d");
                    //再次获取资源
                    bundle = (T)resourcesComponent.GetAsset($"{path}.unity3d", $"{path}");
                }
                return bundle;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }*/

        public GameObject getGameObject(String path)
        {
            return Resources.Load<GameObject>(path);
        }

        public T loadRes<T>(String path) where T : UnityEngine.Object
        {
            return Resources.Load<T>(path);
        }

    }
}
