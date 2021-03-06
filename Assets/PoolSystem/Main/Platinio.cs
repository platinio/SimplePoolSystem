﻿using System.Collections.Generic;
using UnityEngine;



namespace Platinio.PoolSystem
{


    public static class Platinio
    {

        //the links between pools
        public static Dictionary<GameObject, Pool> PoolLinks = null;

        #region SPAWN
        public static T Spawn<T>(this GameObject go) where T : Component
        {
            GameObject clone = Spawn( go, Vector3.zero, Quaternion.identity, null );

            return clone != null ? clone.GetComponent<T>() : null;
        }

        public static T Spawn<T>(this GameObject go, Vector3 position) where T : Component
        {
            GameObject clone = Spawn( go, position, Quaternion.identity, null );

            return clone != null ? clone.GetComponent<T>() : null;
        }


        public static T Spawn<T>(this GameObject go, Vector3 position, Quaternion rotation, Transform parent = null) where T : Component
        {

            GameObject clone = Spawn( go, position, rotation, parent );

            // Return the same component from the clone
            return clone != null ? clone.GetComponent<T>() : null;
        }
        #endregion

        #region UNSPAWN
        /// <summary>
        /// Unspawn object from current scene
        /// </summary>
        public static void Unspawn(this GameObject go)
        {

            CheckPoolConnections();


            Pool pool = TryGetPool( go );

            if (pool != null)
            {
                PoolLinks.Remove( go );
                RemoveFromPool( pool, go );

            }
            else
            {
                //the object does not exist in our pools let's destroy it
                MonoBehaviour.Destroy( go );
            }            

        }

        public static void CheckPoolConnections()
        {
            if (PoolLinks != null)
                return;

            PoolLinks = new Dictionary<GameObject, Pool>();
            PoolManager poolManager = PoolManager.instance;
            //set pools links
            for (int n = 0; n < poolManager.pools.Count; n++)
            {
                PoolLinks[poolManager.pools[n].prefab] = poolManager.pools[n];

                for (int j = 0; j < poolManager.pools[n].inactiveList.Count; j++)
                {
                    PoolLinks[poolManager.pools[n].inactiveList[j].go] = poolManager.pools[n];
                }
            }
        }

        private static Pool TryGetPool(GameObject go)
        {
            Pool pool = null;
            PoolLinks.TryGetValue( go, out pool );
            return pool;
        }

        private static void RemoveFromPool(Pool pool, GameObject go)
        {
            for (int n = 0; n < pool.activeList.Count; n++)
            {
                if (pool.activeList[n].go == go)
                {
                    pool.Unspawn( go );
                    return;
                }
            }

            for (int n = 0; n < pool.inactiveList.Count; n++)
            {
                if (pool.inactiveList[n].go == go)
                {
                    pool.Unspawn( go );
                    return;
                }
            }
        }


        public static void Unspawn(this GameObject go, float t)
        {
            PoolManager.instance.AddDelayUnspawn( go, t );
        }

        #endregion

        #region SPAWN
        /// <summary>
        /// Spawn object in the scene, if the pool no exist will be create
        /// </summary>
        public static GameObject Spawn(this GameObject prefab, Vector3 pos, Quaternion rot, Transform parent)
        {
            //check if we set correctly poolLinks
            CheckPoolConnections();

            //method 1 try get pool connection faster using the dictionary
            Pool pool = null;
            GameObject go = null;

            if (PoolLinks.TryGetValue( prefab, out pool ))
            {
                go = pool.Spawn( pos, rot, parent );
                PoolLinks[go] = pool;
                return go;
            }

            //create the pool and spawn the object
            go = PoolManager.instance.CreatePool( prefab ).Spawn( pos, rot, parent );
            PoolLinks[go] = pool;
            return go;
        }

        /// <summary>
        /// Spawn a object in the scene
        /// </summary>
        public static GameObject Spawn(this GameObject prefab, Vector3 pos)
        {
            return Spawn( prefab, pos, Quaternion.identity, null );
        }

        /// <summary>
        /// Spawn a object in the scene
        /// </summary>
        public static GameObject Spawn(this GameObject prefab, Vector3 pos, Quaternion rot)
        {
            return Spawn( prefab, pos, rot, null );
        }

        /// <summary>
        /// Spawn a object in the scene
        /// </summary>
        public static GameObject Spawn(this GameObject prefab, Transform parent)
        {
            return Spawn( prefab, prefab.transform.position, Quaternion.identity, null );
        }

        /// <summary>
        /// Spawn a object in the scene
        /// </summary>
        public static GameObject Spawn(this GameObject prefab, Vector3 pos, Transform parent)
        {
            return Spawn( prefab, pos, Quaternion.identity, parent );
        }

        public static GameObject Spawn(this GameObject prefab)
        {
            return Spawn( prefab, prefab.transform.position, Quaternion.identity, null );
        }
        #endregion

    }


}