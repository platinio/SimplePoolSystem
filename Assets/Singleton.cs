using System.Collections;
using UnityEngine;

namespace Platinio
{

	/// <summary>
	/// Basic singleton class
	/// </summary>
	public class Singleton<T> : MonoBehaviour where T : Singleton<T>
	{
		private static T m_instance = null;

		public static T instance
		{
			get
			{
				if(m_instance == null)
				{
					//get all the singletones
					T[] singletons = GameObject.FindObjectsOfType( typeof(T) ) as T[];

					if(singletons != null)
					{
						if(singletons.Length == 1)
						{

							m_instance = singletons[0];
							return m_instance;
						}

						else if(singletons.Length > 1)
						{
							Debug.LogWarning("You have more thah one " + typeof( T ).Name + " In the scene, you only need one , all the instances will be destroyed for create a new one");

							m_instance = singletons[0];

							for(int n = 1 ; n < singletons.Length ; n++)
							{
								Destroy( singletons[n].gameObject );
							}

							return m_instance;
						}
					}

					GameObject go = new GameObject( typeof(T).Name );
					m_instance = go.AddComponent<T>();

				}

				return m_instance;
			}
		}



		public static bool IsAlive
		{
			get
			{
				if(m_instance == null)
					return false;
				return m_instance.m_alive;
			}
		}

		public static bool ShouldDestroyOnLoad
		{
			get
			{
				return m_instance.m_shouldDestroyOnLoad;
			}
		}


		[SerializeField] private bool 	m_shouldDestroyOnLoad	= true;
		private bool m_alive = true;

		protected virtual void Start()
		{
			T _instance = instance;

			if(!ShouldDestroyOnLoad)
			{
				DontDestroyOnLoad( m_instance );
			}
		}

		protected virtual void OnDestroy()
		{
			m_alive = false;
		}

		protected virtual void OnApplicationQuit()
		{
			m_alive = false;
		}


	}

}
