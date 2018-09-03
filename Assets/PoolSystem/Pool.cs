using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.Events;
using UnityEditor.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

namespace Platinio.PoolSystem
{
	[System.Serializable]
	public class Pool 
	{
		#region PUBLIC
		public GameObject   		prefab 			= null;	
		public int		    		initialSize 	= 5;
        public int 					maxSize 		= 10;
		public List<PoolObject>		activeList		= null;
		public List<PoolObject>		inactiveList	= null;
		public Transform			parent			= null;
		public int 					id				= -1;
		#endregion



		public void Initialize(Transform t , int id)
		{
			activeList 		= new List<PoolObject>();
			inactiveList 	= new List<PoolObject>();

			//initialize
			parent 	= t;
			this.id = id;

			//create initial PoolObjects
			for(int n = 0 ; n < initialSize ; n++)
			{

                GameObject obj = null;

                #if UNITY_EDITOR

                if (Application.isPlaying)
                {
                    obj = MonoBehaviour.Instantiate(prefab, parent);
                }
                else
                {
                    obj = PrefabUtility.InstantiatePrefab( prefab ) as GameObject;
                    obj.transform.parent = parent;
                }

                #else
                obj = MonoBehaviour.Instantiate(prefab, parent);
                #endif


                obj.SetActive( false );
				inactiveList.Add( new PoolObject( obj ) );		
			}

		}


		public GameObject Spawn(Vector3 pos , Quaternion rot , Transform parent)
		{
			PoolObject obj = null;

			if( inactiveList.Count == 0 )
			{
				
				obj =  new PoolObject( (GameObject) MonoBehaviour.Instantiate( prefab, pos, rot ) );	
				obj.SetDelegates();
			}

			else
			{
				obj 						= inactiveList[0];
				obj.go.transform.parent 	= parent;
				obj.go.transform.position 	= pos;
				obj.go.transform.rotation 	= rot;
				obj.go.SetActive(true);

				inactiveList.RemoveAt(0);

			}

			activeList.Add(obj);

			if(obj.OnSpawn != null)
                obj.OnSpawn.Invoke();
            //obj.OnSpawn();
            
				

			//if we dont have more object in the pool create a new one
			if( inactiveList.Count == 0 )
			{
				PoolObject newObj =  new PoolObject( (GameObject) MonoBehaviour.Instantiate( prefab , parent ) );	
				newObj.SetDelegates();
				newObj.go.SetActive( false );
				inactiveList.Add( newObj );
			}

			Transform t = obj.go.transform;

			//try to unspawn all the childrens

			//Debug.Log("obj " + t.gameObject.name + " has " + t.childCount + " childs");

			List<Transform> childs = new List<Transform>();

			for(int j = 0 ; j < t.childCount ; j++)
			{
				childs.Add( t.GetChild(j) );
			}

			for(int n = 0 ; n < childs.Count ; n++)
			{
				childs[n].gameObject.Unspawn();
			}


			return obj.go;
				
		}

		public void Unspawn(GameObject obj)
		{
			for(int n = 0 ; n < activeList.Count ; n++)
			{
				//is the same obj?
				if( obj == activeList[n].go )
				{
					if(activeList[n].OnUnspawn != null)
                        activeList[n].OnUnspawn.Invoke();
                        //activeList[n].OnUnspawn();



                    activeList[n].go.transform.parent = parent;
					activeList[n].go.SetActive(false);

					PoolObject p = activeList[n];
					activeList.RemoveAt( n );

					//if the pool exced maxSize lest destroy the object we dont need it
					//(inactiveList.Count < maxSize)
						inactiveList.Add( p );

					//else
					//{
						//Debug.Log("destroying " + obj.name);
						//MonoBehaviour.Destroy( p.go );
					//}
												

					return;
				}
			}

			Debug.Log("obj " + obj.name + "no unspaned");

			for(int n = 0 ; n < inactiveList.Count ; n++)
			{
				//is the same obj?
				if( obj == inactiveList[n].go )
				{
					Debug.Log("callind unspaw on inactive object " + obj.name);
				}
			}


			//maybe do you want to remove me?
			//Debug.Log("destroying " + obj.name);
			//MonoBehaviour.Destroy( obj );

		}

		

	}

	[System.Serializable]
	public class PoolObject
	{
		public GameObject 		go			= null;
		//public MonoBehaviour[] 	scripts		= null;

		public delegate void OnEvent();
		//public OnEvent OnSpawn;
		//public OnEvent OnUnspawn;

        public UnityEvent OnSpawn = new UnityEvent();
        public UnityEvent OnUnspawn = new UnityEvent();

		public PoolObject(GameObject go)
		{
			this.go = go;
            SetDelegates();
		}

		//set all the callbacks
		public void SetDelegates()
		{            
            MonoBehaviour[] scripts = go.GetComponents<MonoBehaviour>();
            
            Debug.Log("setting delagates");

            for (int n = 0 ; n < scripts.Length ; n++)
			{
                
                MethodInfo method = scripts[n].GetType().GetMethod("OnSpawn", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);

                if (method != null)
                {
                    UnityAction action = (UnityAction)System.Delegate.CreateDelegate(typeof(UnityAction), scripts[n], method);
                    UnityEventTools.AddPersistentListener(OnSpawn , action);
                }

                    
                
                method = scripts[n].GetType().GetMethod("OnUnspawn", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);

                if (method != null)
                {
                    UnityAction action = (UnityAction)System.Delegate.CreateDelegate(typeof(UnityAction), scripts[n], method);
                    UnityEventTools.AddPersistentListener(OnUnspawn, action);
                }                

            }
            

        }
	}

}
