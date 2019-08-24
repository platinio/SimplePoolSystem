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

                obj = MonoBehaviour.Instantiate(prefab, parent);
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

            obj.InvokeOnSpawnCallback();


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
					activeList[n].InvokeOnUnSpawnCallback();
                    

                    activeList[n].go.transform.parent = parent;
					activeList[n].go.SetActive(false);

					PoolObject p = activeList[n];
					activeList.RemoveAt( n );

					//if the pool exced maxSize lest destroy the object we dont need it
					if(inactiveList.Count < maxSize)
						inactiveList.Add( p );

					else
					{
						//Debug.Log("destroying " + obj.name);
						MonoBehaviour.Destroy( p.go );
					}												

					return;
				}
			}

			
		}

		

	}


}
