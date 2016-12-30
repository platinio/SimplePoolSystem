using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Platinio;

public class Remove : MonoBehaviour
{
	public float timeToRemove;

	private float timer = 0f;


	void OnDisable()
	{
		timer = 0f;
	}

	void Update()
	{

		timer += Time.deltaTime;

		if(timer > timeToRemove)
		{
			PoolManager.instance.Remove (gameObject);
		}
			
	}
}
