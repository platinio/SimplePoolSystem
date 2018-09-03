using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;



using UnityEngine;

namespace Platinio.PoolSystem
{
	
	public class PoolManager : Singleton<PoolManager>
	{
		#region PUBLIC
		 public List<Pool> 	pools					= new List<Pool>();
		[HideInInspector] public int 			defaultInitialPoolSize 	= 10;
		[HideInInspector] public int 			defaultMaxPoolSize		= 20;
		[HideInInspector] public int			idCounter				= 0;
		#endregion


		void Awake()
		{
			//set all the delegates
            /*
			for(int n = 0 ; n < pools.Count ; n++)
			{
				for(int  j = 0 ; j < pools[n].inactiveList.Count ; j++)
				{
					pools[n].inactiveList[j].SetDelegates();
				}
			}
            */
		}

		#region PUBLIC_METHODS
		/// <summary>
		/// Called by the inspector to create the pool before running the game this make loading faster
		/// </summary>
		public void PreSpawnObjects ()
		{
			idCounter = 0;

			//this piece of code looks strange but for some strange reason is neede
			int childCount = transform.childCount;

			for(int n = 0 ; n < childCount ; n++ )
			{
				for(int j = 0 ; j < transform.childCount ; j++)
					DestroyImmediate( transform.GetChild( j ).gameObject );
			}

           
			//create the pools
			for(int n = 0 ; n < pools.Count ; n++)
			{
                if (pools[n].prefab != null)
                {
					GameObject go = new GameObject( pools[n].prefab.name );
                    go.transform.parent = transform;

					pools[n].Initialize(go.transform , idCounter++);
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

			GameObject go 		= new GameObject( p.prefab.name );
			go.transform.parent = transform;
			p.Initialize( go.transform , idCounter++);

			//add to the pool
			pools.Add( p );

			return p;
		}
		#endregion


	}
}

