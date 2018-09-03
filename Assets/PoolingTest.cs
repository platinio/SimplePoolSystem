using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platinio.PoolSystem;

public class PoolingTest : MonoBehaviour
{
    public GameObject prefab = null;


    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            prefab.Spawn();

        if(Input.GetKey(KeyCode.A))
            prefab.Unspawn();
            
    }
}
