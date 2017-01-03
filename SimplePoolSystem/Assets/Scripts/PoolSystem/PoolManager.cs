using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platinio
{

	public class PoolManager : MonoBehaviour 
	{
		//singletone
		private static PoolManager poolMananager;
		public static PoolManager instance
		{
			get
			{ 
				if (!poolMananager)
				{
					poolMananager = FindObjectOfType (typeof(PoolManager)) as PoolManager;


					if (!poolMananager)
						Debug.LogError ("You need to have at least a PoolManager active in the scene");
				}

				return poolMananager;
			}

		}

		public List<PoolObject> poolsHolder;


		void Awake()
		{
			InitializePool ();

		}

		/// <summary>
		/// Initializes the pool.
		/// </summary>
		public void InitializePool ()
		{
			for(int n = 0 ; n < poolsHolder.Count ; n++)
			{
				for(int i = 0 ; i < poolsHolder[n].poolSize ; i++)
				{
					CreateNewPoolPrefab (n);
				}

			}

		}

		/// <summary>
		/// Add a new object in the pool
		/// </summary>
		/// <returns>The gameobject</returns>
		/// <param name="poolIndex">The index of the pool</param>
		public GameObject CreateNewPoolPrefab(int poolIndex)
		{
			poolsHolder [poolIndex].unactive.Add (Instantiate (poolsHolder [poolIndex].go, Vector3.zero, Quaternion.identity) as GameObject);

			int index = poolsHolder [poolIndex].unactive.Count - 1;
			poolsHolder [poolIndex].unactive[index].SetActive (false);
			poolsHolder [poolIndex].unactive[index].transform.parent = transform;

			return poolsHolder [poolIndex].unactive[index];
		}

		/// <summary>
		/// Create a Gameobject using position and rotation.
		/// </summary>
		/// <returns>The gameobject</returns>
		/// <param name="obj">The pool Prefab</param>
		/// <param name="pos">Position of the object</param>
		/// <param name="rot">Rotation of the object</param>
		public GameObject Create(GameObject obj , Vector3 pos , Quaternion rot)
		{
			GameObject go;

			//start finding a unactived object
			for(int n = 0 ; n < poolsHolder.Count ; n++)
			{
				//if we are in the correct pool
				if(poolsHolder[n].go == obj)
				{
					//find a unactived object
					Debug.Log(poolsHolder[n].unactive.Count);
					if(poolsHolder[n].unactive.Count > 0)
					{
						//get object
						int lastIndex = poolsHolder [n].unactive.Count - 1;
						go = poolsHolder [n].unactive [lastIndex];

						//intialize object
						go.SetActive (true);
						go.transform.position = pos;
						go.transform.rotation = rot;

						//move object
						poolsHolder [n].active.Add (go);
						poolsHolder [n].unactive.Remove (go);

						return go; //return gameObject
					}



					//if we dont have any unactived object in the pool
					//initializa object
					go = CreateNewPoolPrefab (n);
					go.SetActive (true);
					go.transform.position = pos;
					go.transform.rotation = rot;

					//move object
					poolsHolder [n].active.Add (go);
					poolsHolder [n].unactive.Remove (go);

					return go; //return gameObject

				}
			}


			Debug.LogError ("Pool object can no be found");
			return null;
		}

		/// <summary>
		/// Create a Gameobject using position.
		/// </summary>
		/// <param name="obj">The pool Prefab</param>
		/// <param name="pos">Position of the object</param>
		public GameObject Create(GameObject obj , Vector3 pos)
		{
			return Create (obj , pos , Quaternion.identity);
		}

		/// <summary>
		/// Create a object in position (0 , 0 , 0)
		/// </summary>
		/// <param name="obj">The pool Prefab</param>
		public GameObject Create(GameObject obj)
		{
			return Create (obj , Vector3.zero , Quaternion.identity);
		}

		/// <summary>
		/// Return the object to the pool
		/// </summary>
		/// <param name="go">The GameObjec to remove</param>
		public void Remove(GameObject go)
		{

			//reset object
			go.SetActive (false);
			go.transform.position = Vector3.zero;

			if (go.transform.parent != transform)
				go.transform.parent = transform;

			//move object
			for(int n = 0 ; n < poolsHolder.Count ; n++)
			{

				if(poolsHolder[n].active.Contains(go))
				{
					poolsHolder [n].active.Remove (go);
					poolsHolder [n].unactive.Add (go);

					return;
				}
			}

		}
	
	}

	[System.Serializable]
	public class PoolObject
	{
		public GameObject go; //the original game object
		public List<GameObject> active; //the pool of objects
		public List<GameObject> unactive;
		public int poolSize; //the pool size

	}


}