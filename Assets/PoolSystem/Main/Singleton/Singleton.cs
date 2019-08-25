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
                if (m_instance == null)
                {
                    m_instance = FindSingletonInstance();
                }

                return m_instance;
            }
        }



        public static bool ApplicationIsQuitting = false;


        public static bool DestroyOnLoad
        {
            get
            {
                return m_instance.m_destroyOnLoad;
            }
        }

        private static bool CanCreateMoreInstances
        {
            get
            {
                return !ApplicationIsQuitting;
            }
        }


        [SerializeField] private bool m_destroyOnLoad = true;

        protected virtual void Awake()
        {            
            //destroy repeat instance
            if (instance != this)
            {
                Destroy(gameObject);
                return;
            }

            if (!DestroyOnLoad)
            {
                DontDestroyOnLoad( instance );
            }
        }

        protected virtual void OnApplicationQuit()
        {
            ApplicationIsQuitting = true;

        }

        private static T FindSingletonInstance()
        {

            T[] singletons = GetAllSingletonObjects();

            T instance = DestroyAllRepeatInstances(singletons);

            if (instance != null)
                return instance;

            //if the application is quitting dont create more singletons
            if (!CanCreateMoreInstances)
                return null;

                        
            return CreateSingleton();
        }

        private static T[] GetAllSingletonObjects()
        {
            return GameObject.FindObjectsOfType( typeof( T ) ) as T[];
        }

        private static T DestroyAllRepeatInstances(T[] singletons)
        {
            if (singletons == null || singletons.Length == 0)
                return null;

            if (singletons.Length > 1)
            {
                for (int n = 1; n < singletons.Length; n++)
                {
                    Destroy( singletons[n].gameObject );
                }
            }

            return singletons[0];
        }

        private static T CreateSingleton()
        {
            GameObject go = new GameObject( typeof( T ).Name );
            return go.AddComponent<T>();
        }

    }    

}
