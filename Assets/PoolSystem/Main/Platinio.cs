using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Platinio.PoolSystem
{
	

	public static class Platinio
	{

        //the links betwen pools
        public static Dictionary<GameObject , Pool> PoolLinks = null;

        #region SPAWN_COMPONENT
        public static T Spawn<T>(this GameObject go) where T : Component
        {
            
            // Clone this prefabs's GameObject            
            GameObject clone = Spawn(go, Vector3.zero, Quaternion.identity, null);

            // Return the same component from the clone
            return clone != null ? clone.GetComponent<T>() : null;
        }

        public static T Spawn<T>(this GameObject go , Vector3 position) where T : Component
        {
           
            // Clone this prefabs's GameObject            
            GameObject clone = Spawn(go, position, Quaternion.identity, null);

            // Return the same component from the clone
            return clone != null ? clone.GetComponent<T>() : null;
        }


        public static T Spawn<T>(this GameObject go, Vector3 position, Quaternion rotation, Transform parent = null) where T : Component
        {           
           
            GameObject clone = Spawn(go, position, rotation, parent);

            // Return the same component from the clone
            return clone != null ? clone.GetComponent<T>() : null;
        }
        #endregion

        #region UNSPAWN
        /// <summary>
        /// Unspawn object in from current scene , slow please dont use id instead
        /// </summary>
        public static void Unspawn( this GameObject go )
		{

            //check if we set correctly poolLinks
            SetPoolLinks();

            
            //method 1 try get pool connection faster using the dictionary
            Pool pool = null;

            if (PoolLinks.TryGetValue(go, out pool))
            {
                PoolLinks.Remove(go);

                for (int n = 0; n < pool.activeList.Count; n++)
                {
                    if (pool.activeList[n].go == go)
                    {
                        pool.Unspawn(go);
                        return;
                    }
                }

                for (int n = 0; n < pool.inactiveList.Count; n++)
                {
                    if (pool.inactiveList[n].go == go)
                    {
                        pool.Unspawn(go);
                        return;
                    }
                }
            }

            //the object does no exist in our pools lets destroy it
            MonoBehaviour.Destroy(go);
            
		}

        public static void Unspawn(this GameObject go , float t)
        {
            PoolManager.instance.AddDelayUnspawn(go , t);
        }

        #endregion

        #region SPAWN
        /// <summary>
        /// Spawn object in the scene, if the pool no exist will be create
        /// </summary>
        public static GameObject Spawn(this GameObject prefab , Vector3 pos , Quaternion rot , Transform parent)
		{
            //check if we set correctly poolLinks
            SetPoolLinks();

            //method 1 try get pool connection faster using the dictionary
            Pool pool = null;
            GameObject go = null;

            if (PoolLinks.TryGetValue(prefab, out pool))
            {
                go = pool.Spawn(pos , rot , parent);
                PoolLinks[go] = pool;
                return go;
            }
                        
			//create the pool and spawn the object
			go = PoolManager.instance.CreatePool(prefab).Spawn( pos , rot , parent );
            PoolLinks[go] = pool;
            return go;
        }

		/// <summary>
		/// Spawn a object in the scene
		/// </summary>
		public static GameObject Spawn(this GameObject prefab , Vector3 pos)
		{
			return Spawn( prefab ,  pos , Quaternion.identity , null);
		}

		/// <summary>
		/// Spawn a object in the scene
		/// </summary>
		public static GameObject Spawn(this GameObject prefab , Vector3 pos , Quaternion rot)
		{
			return Spawn( prefab ,  pos , rot , null);
		}

		/// <summary>
		/// Spawn a object in the scene
		/// </summary>
		public static GameObject Spawn(this GameObject prefab , Transform parent)
		{
			return Spawn( prefab ,  prefab.transform.position , Quaternion.identity , null);
		}

		/// <summary>
		/// Spawn a object in the scene
		/// </summary>
		public static GameObject Spawn(this GameObject prefab , Vector3 pos , Transform parent)
		{
			return Spawn( prefab , pos , Quaternion.identity , parent );
		}

		public static GameObject Spawn(this GameObject prefab)
		{
			return Spawn( prefab , prefab.transform.position , Quaternion.identity , null );
		}
        #endregion

        public static void SetPoolLinks()
        {
            if(PoolLinks != null)
                return;

            PoolLinks = new Dictionary<GameObject, Pool>();
            PoolManager poolManager = PoolManager.instance;
            //set pools links
            for (int n = 0; n < poolManager.pools.Count; n++)
            {
                PoolLinks[poolManager.pools[n].prefab] = poolManager.pools[n];

                for (int j = 0 ; j < poolManager.pools[n].inactiveList.Count ; j++)
                {
                    PoolLinks[poolManager.pools[n].inactiveList[j].go] = poolManager.pools[n];
                }
            }
        }
		

	}


}