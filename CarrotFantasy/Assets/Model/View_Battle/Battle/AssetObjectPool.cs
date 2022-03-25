using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ETModel
{
    public class AssetObjectPool
    {
        private static AssetObjectPool assetObjectPool;
        private Dictionary<String, GameObject> path2Asset = new Dictionary<string, GameObject>();
        private Dictionary<String, UnityEngine.Object> path2Asset2 = new Dictionary<string, UnityEngine.Object>();

        public static AssetObjectPool getInstance()
        {
            if(assetObjectPool == null)
            {
                assetObjectPool = new AssetObjectPool();
            }
            return assetObjectPool;
        }

        public GameObject getAsset(String path)
        {
            if(this.path2Asset[path] == null)
            {
                this.path2Asset[path] = ResourceLoader.getInstance().getGameObject(path);
            }
            return this.path2Asset[path];
        }

        public T getAsset<T>(String path) where T: UnityEngine.Object
        {
            if (this.path2Asset2[path] == null)
            {
                this.path2Asset2[path] = ResourceLoader.getInstance().loadRes<T> (path);
            }
            return (T)this.path2Asset2[path];
        }

        public void dispose()
        {
            this.path2Asset.Clear();
            this.path2Asset2.Clear();
        }
    }
}
