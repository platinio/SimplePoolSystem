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
            go.BroadcastMessage( "OnSpawn", SendMessageOptions.DontRequireReceiver );
        }

        public void InvokeOnUnSpawnCallback()
        {
            go.BroadcastMessage("OnUnspawn" , SendMessageOptions.DontRequireReceiver);
        }
    }
}

