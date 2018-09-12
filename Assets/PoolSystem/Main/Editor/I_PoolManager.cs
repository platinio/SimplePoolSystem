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
        private PoolManager _target;

        void OnEnable()
        {
			_target = (PoolManager) target;
        }

		public override void OnInspectorGUI()
        {
			base.OnInspectorGUI();
            EditorGUILayout.Space();

            GUIContent cont;

            //if we dont have any pool
            if ( _target.pools == null )
            {
                _target.pools = new List<Pool>();
                _target.pools.Add( new Pool() );
            }

            for (int n = 0; n < _target.pools.Count; n++ )
            {
                EditorGUILayout.LabelField("Pool " + ( n + 1) , EditorStyles.boldLabel );

                cont = new GUIContent("Prefab" , "The pool object");
                _target.pools[n].prefab = (GameObject) EditorGUILayout.ObjectField(cont, _target.pools[n].prefab, typeof(GameObject), false);

				//cont = new GUIContent("Pool Name" , "Use to get pools objects faster");
               // _target.pools[n].poolName = EditorGUILayout.TextField( cont , _target.pools[n].poolName );

                cont = new GUIContent("Initial Size" , "The initial number of objects create in the pool");
                _target.pools[n].initialSize = EditorGUILayout.IntField( cont , _target.pools[n].initialSize );

                cont = new GUIContent("Max Size" , "Max number of objects what can exist in the pool, \nA lower number can cause instantiates on running time \nA big number can cause waste memory space \n be wise!");
                _target.pools[n].maxSize = EditorGUILayout.IntField(cont, _target.pools[n].maxSize);
                
                if (GUILayout.Button("-", GUILayout.Width(20)))
					_target.pools.RemoveAt(n);   
				
                                
            }

            if (GUILayout.Button("+" , GUILayout.Width(20) ))
                _target.pools.Add( new Pool() );
            		
			           
        }
    }

}
