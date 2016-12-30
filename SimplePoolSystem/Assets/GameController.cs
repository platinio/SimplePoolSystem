using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Platinio;

public class GameController : MonoBehaviour 
{

	public GameObject boxPrefab;

	void Update()
	{
		if (Input.GetMouseButtonDown (0))
		{
			Vector3 pos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			PoolManager.instance.Create (boxPrefab , new Vector3(pos.x , pos.y , 0));
		}
	}

}
