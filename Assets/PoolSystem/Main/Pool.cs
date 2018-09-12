using System.Collections.Generic;
using System.Reflection;
using UnityEngine.Events;


#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Events;
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
		#endregion



		public void Initialize(Transform t)
		{
			activeList 		= new List<PoolObject>();
			inactiveList 	= new List<PoolObject>();

			//initialize
			parent 	= t;
			

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


            if (obj.OnSpawn != null)
                obj.OnSpawn.Invoke();
            //obj.OnSpawn();


            //if we dont have more object in the pool create a new one
            if ( inactiveList.Count == 0)
			{
                
				PoolObject newObj =  new PoolObject( (GameObject) MonoBehaviour.Instantiate( prefab , this.parent ) );	
				newObj.go.SetActive( false );
				inactiveList.Add( newObj );
			}

			
			return obj.go;
				
		}

		public void Unspawn(GameObject obj , float t = 0.0f)
		{
            if (t > 0.0f)
            {
                PoolManager.instance.AddDelayUnspawn( obj , t );
                return;
            }

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
        public delegate void OnAction();

        public GameObject   go			= null;
		public OnAction     OnSpawn     = null;
        public OnAction     OnUnspawn   = null;

		public PoolObject(GameObject go)
		{
			this.go = go;
            SetDelegates();
		}

		//set all the callbacks
		private void SetDelegates()
		{      
           

            MonoBehaviour[] scripts = go.GetComponents<MonoBehaviour>();           
            

            for (int n = 0 ; n < scripts.Length ; n++)
			{
                
                MethodInfo method = scripts[n].GetType().GetMethod("OnSpawn", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);

                if (method != null)
                {
                    OnSpawn += (OnAction)System.Delegate.CreateDelegate(typeof(OnAction), scripts[n], method);                   

                }

                method = scripts[n].GetType().GetMethod("OnUnspawn", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);

                if (method != null)
                {
                    OnUnspawn = (OnAction)System.Delegate.CreateDelegate(typeof(OnAction), scripts[n], method);
                }

            }
            

        }
	}

}
