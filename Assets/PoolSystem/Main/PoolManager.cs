using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;



using UnityEngine;

namespace Platinio.PoolSystem
{
	/// <summary>
    /// Singleton for handling the pool system
    /// </summary>
	public class PoolManager : Singleton<PoolManager>
	{
		#region PUBLIC
		[HideInInspector] public List<Pool> 	pools					= new List<Pool>();
		[HideInInspector] public int 			defaultInitialPoolSize 	= 10;
		[HideInInspector] public int 			defaultMaxPoolSize		= 20;		
		#endregion

        //a list to keep track of delay Unspawn
        private List<DelayUnspawn> m_delayUnspawnRequest = new List<DelayUnspawn>();
        #region UNITY_EVENTS
        private void Awake()
		{            
            Platinio.SetPoolLinks();

            //initialize pool
            PreSpawnObjects();
		}

        private void LateUpdate()
        {
            //update delay unspawn request
            for (int n = 0 ; n < m_delayUnspawnRequest.Count ; n++)
            {
                m_delayUnspawnRequest[n].time -= Time.deltaTime;

                if (m_delayUnspawnRequest[n].time <= 0.0f)
                {
                    m_delayUnspawnRequest[n].go.Unspawn();
                    m_delayUnspawnRequest.RemoveAt(n);
                }
            }
        }
        #endregion

        #region PUBLIC_METHODS

        public void AddDelayUnspawn(GameObject go , float t)
        {
            m_delayUnspawnRequest.Add( new DelayUnspawn() { go = go, time = t } );
        }

		/// <summary>
        /// Called in awake to create the pool initial objects
        /// </summary>
		private void PreSpawnObjects ()
		{
			           
			//create the pools
			for(int n = 0 ; n < pools.Count ; n++)
			{
                if (pools[n].prefab != null)
                {
					GameObject go = new GameObject( pools[n].prefab.name );
                    go.transform.parent = transform;

					pools[n].Initialize(go.transform);
                }
            }

		}

		/// <summary>
		/// Creates a new pool in running time, no recomended
		/// </summary>
		/// <param name="prefab">Prefab.</param>
		/// <param name="maxSize">Max size.</param>
		/// <param name="initialSize">Initial size.</param>
		public Pool CreatePool(GameObject prefab , int maxSize = -1 , int initialSize = -1)
		{
			//create object and initialize it
			Pool p = new Pool();

			p.prefab 		= prefab;
			p.maxSize 		= maxSize < 1 ? defaultMaxPoolSize : maxSize;
			p.initialSize 	= initialSize < 1 ? defaultInitialPoolSize : initialSize;
            p.parent        = transform;

			GameObject go 		= new GameObject( p.prefab.name );
			go.transform.parent = transform;
			p.Initialize( go.transform);

			//add to the pool
			pools.Add( p );

            //set the link connection
            Platinio.PoolLinks[prefab] = p;

			return p;
		}
		#endregion


	}

    public class DelayUnspawn
    {
        public GameObject go;
        public float time;
    }
}

