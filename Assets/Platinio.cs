using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Platinio.PoolSystem
{
	

	public static class Platinio
	{

        //the links betwen pools
        public static Dictionary<GameObject , Pool> links = new Dictionary<GameObject, Pool>();

        #region SPAWN_COMPONENT

        public static T Spawn<T>(T prefab) where T : Component
        {
            if (prefab == null)
            {
                Debug.LogError("can no spanw null");
                return null;
            }

            // Clone this prefabs's GameObject
            GameObject gameObject = prefab.gameObject;
            GameObject clone = Spawn(gameObject, Vector3.zero, Quaternion.identity, null);

            // Return the same component from the clone
            return clone != null ? clone.GetComponent<T>() : null;
        }

        public static T Spawn<T>(T prefab, Vector3 position) where T : Component
        {
            if (prefab == null)
            {
                Debug.LogError("can no spanw null");
                return null;
            }

            // Clone this prefabs's GameObject
            GameObject gameObject = prefab.gameObject;
            GameObject clone = Spawn(gameObject, position, Quaternion.identity, null);

            // Return the same component from the clone
            return clone != null ? clone.GetComponent<T>() : null;
        }


        public static T Spawn<T>(T prefab, Vector3 position, Quaternion rotation, Transform parent = null) where T : Component
        {
            if (prefab == null)
            {
                Debug.LogError("can no spanw null");
                return null;
            }

            // Clone this prefabs's GameObject
            GameObject gameObject = prefab.gameObject;
            GameObject clone = Spawn(gameObject, position, rotation, parent);

            // Return the same component from the clone
            return clone != null ? clone.GetComponent<T>() : null;
        }
        #endregion

        /// <summary>
        /// Unspawn object from current scene , use id to identify pool faster
        /// </summary>
        public static void Unspawn( this GameObject go , int id )
		{
			for(int n = 0 ; n < PoolManager.instance.pools.Count ; n++)
			{
				if( id == PoolManager.instance.pools[n].id )
				{
					PoolManager.instance.pools[n].Unspawn( go );
					return;
				}
			}

			Debug.LogError("No pool hash found");
		}

		/// <summary>
		/// Unspawn object in from current scene , slow please dont use id instead
		/// </summary>
		public static void Unspawn( this GameObject go )
		{
			//look firts on active list
			for(int j = 0 ; j < PoolManager.instance.pools.Count ; j++)
			{
				for(int k = 0 ; k < PoolManager.instance.pools[j].activeList.Count ; k++)
				{
					if(PoolManager.instance.pools[j].activeList[k].go == go)
					{
						PoolManager.instance.pools[j].Unspawn( go );
						return;
					}
				}				

			}

			//look in incavtive list
			for(int j = 0 ; j < PoolManager.instance.pools.Count ; j++)
			{
				for(int k = 0 ; k < PoolManager.instance.pools[j].inactiveList.Count ; k++)
				{
					if(PoolManager.instance.pools[j].inactiveList[k].go == go)
					{
						PoolManager.instance.pools[j].Unspawn( go );
						return;
					}
				}				

			}


		}

		/// <summary>
		/// Spawn object in the scene, if the pool no exist will be create
		/// </summary>
		public static GameObject Spawn(this GameObject prefab , Vector3 pos , Quaternion rot , Transform parent)
		{

			for(int n = 0 ; n < PoolManager.instance.pools.Count ; n++)
			{
				if( PoolManager.instance.pools[n].prefab == prefab )
					return PoolManager.instance.pools[n].Spawn( pos , rot , parent );

			}

			//create the pool and spawn the object
			return PoolManager.instance.CreatePool(prefab).Spawn( pos , rot , parent );
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

		//call in initialization to cache hash
		public static int GetPoolID( this GameObject go )
		{
			for(int j = 0 ; j < PoolManager.instance.pools.Count ; j++)
			{
				if(PoolManager.instance.pools[j].prefab == go)
					return PoolManager.instance.pools[j].id;

				for(int k = 0 ; k < PoolManager.instance.pools[j].activeList.Count ; k++)
				{
					if(PoolManager.instance.pools[j].activeList[k].go == go)
						return PoolManager.instance.pools[j].id;
				}	

				for(int k = 0 ; k < PoolManager.instance.pools[j].inactiveList.Count ; k++)
				{
					if(PoolManager.instance.pools[j].inactiveList[k].go == go)
						return PoolManager.instance.pools[j].id;
				}	



			}

			Debug.LogError("No pool id found, the object does no exist as prefab from pools or in the inactiveList, please be sure to cache PoolHas in Awake, before any spawning");

			return -1;
		}

	}


}