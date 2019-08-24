using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace Platinio.PoolSystem
{
    [CustomEditor( typeof( PoolManager ) )]
    public class I_PoolManager : Editor
    {
        private PoolManager poolManager;

        void OnEnable()
        {
            poolManager = (PoolManager) target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Space();

            GUIContent cont;

            //if we dont have any pool
            if (poolManager.pools == null)
            {
                poolManager.pools = new List<Pool>();
                poolManager.pools.Add( new Pool() );
            }

            for (int n = 0; n < poolManager.pools.Count; n++)
            {
                EditorGUILayout.LabelField( "Pool " + ( n + 1 ), EditorStyles.boldLabel );

                cont = new GUIContent( "Prefab", "The pool object" );
                poolManager.pools[n].prefab = (GameObject) EditorGUILayout.ObjectField( cont, poolManager.pools[n].prefab, typeof( GameObject ), false );

                cont = new GUIContent( "Initial Size", "The initial number of objects create in the pool" );
                poolManager.pools[n].initialSize = EditorGUILayout.IntField( cont, poolManager.pools[n].initialSize );

                cont = new GUIContent( "Max Size", "Max number of objects what can exist in the pool, A lower number can cause instantiated on running time, A big number can cause waste memory space" );
                poolManager.pools[n].maxSize = EditorGUILayout.IntField( cont, poolManager.pools[n].maxSize );

                if (GUILayout.Button( "-", GUILayout.Width( 20 ) ))
                    poolManager.pools.RemoveAt( n );
            }

            if (GUILayout.Button( "+", GUILayout.Width( 20 ) ))
                poolManager.pools.Add( new Pool() );


        }
    }

}
