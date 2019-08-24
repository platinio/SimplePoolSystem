using UnityEngine;

namespace Platinio
{
    [System.Serializable]
    public class PoolObject
    {
        public GameObject go = null;

        public PoolObject(GameObject go)
        {
            this.go = go;
        }

        public void InvokeOnSpawnCallback()
        {
            go.SendMessage("OnSpawn");
        }

        public void InvokeOnUnSpawnCallback()
        {
            go.SendMessage("OnUnSpawn");
        }
    }
}

