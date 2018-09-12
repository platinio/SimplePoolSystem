using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace Platinio.PoolSystem
{
    [CustomEditor(typeof( PoolManager ))]
    public class I_PoolManager : Editor
    {
        private PoolManager m_target;

        void OnEnable()
        {
            m_target = (PoolManager) target;
        }

		public override void OnInspectorGUI()
        {
			base.OnInspectorGUI();
            EditorGUILayout.Space();

            GUIContent cont;

            //if we dont have any pool
            if (m_target.pools == null )
            {
                m_target.pools = new List<Pool>();
                m_target.pools.Add( new Pool() );
            }

            for (int n = 0; n < m_target.pools.Count; n++ )
            {
                EditorGUILayout.LabelField("Pool " + ( n + 1) , EditorStyles.boldLabel );

                cont = new GUIContent("Prefab" , "The pool object");
                m_target.pools[n].prefab = (GameObject) EditorGUILayout.ObjectField(cont, m_target.pools[n].prefab, typeof(GameObject), false);
                				
                cont = new GUIContent("Initial Size" , "The initial number of objects create in the pool");
                m_target.pools[n].initialSize = EditorGUILayout.IntField( cont , m_target.pools[n].initialSize );

                cont = new GUIContent("Max Size" , "Max number of objects what can exist in the pool, A lower number can cause instantiates on running time A big number can cause waste memory space be wise!");
                m_target.pools[n].maxSize = EditorGUILayout.IntField(cont, m_target.pools[n].maxSize);
                
                if (GUILayout.Button("-", GUILayout.Width(20)))
                    m_target.pools.RemoveAt(n);                              
            }

            if (GUILayout.Button("+" , GUILayout.Width(20) ))
                m_target.pools.Add( new Pool() );
            		
			           
        }
    }

}
