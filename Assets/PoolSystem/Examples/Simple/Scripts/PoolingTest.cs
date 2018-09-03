using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platinio.PoolSystem;

public class PoolingTest : MonoBehaviour
{
    public GameObject prefab = null;
    public Stack<GameObject> spawned = new Stack<GameObject>();

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            spawned.Push(prefab.Spawn(GetMousePosition()));
        }

        if (Input.GetMouseButtonDown(1) && spawned.Count > 0)
            spawned.Pop().Unspawn();            
    }

    private Vector3 GetMousePosition()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint( Input.mousePosition );
        pos.z = 0.0f;

        return pos;
    }


}
