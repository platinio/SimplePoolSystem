using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Platinio;

[CustomEditor(typeof(PoolManager))]
public class I_PoolManagerInspector :  Editor
{
	private PoolManager _target;



	void OnEnable()
	{
		_target = (PoolManager)target;
	}


	public override void OnInspectorGUI()
	{
		
		if(_target.poolsHolder.Count > 0)
		{
			
			for(int n = 0 ; n < _target.poolsHolder.Count ; n++)
			{
				_target.poolsHolder[n].go = (GameObject)EditorGUILayout.ObjectField ("Pool Prefab" , _target.poolsHolder[n].go , typeof(GameObject) , false);

				if(GUI.Button(new Rect(GUILayoutUtility.GetLastRect().x - 15, GUILayoutUtility.GetLastRect().y, 15, 15), "-"))
				{
					_target.poolsHolder.RemoveAt (n);
				}

				_target.poolsHolder [n].poolSize = EditorGUILayout.IntField ("Pool Size: " , _target.poolsHolder[n].poolSize);

			}
		}

		if (GUILayout.Button ("Create Pool"))
		{
			_target.poolsHolder.Add (new PoolObject());
		}

		

		if (GUI.changed)
			EditorUtility.SetDirty (_target);
	}

}
