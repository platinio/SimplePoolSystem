using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTest : MonoBehaviour
{
    private void OnUnspawn()
    {
        Debug.Log("Calling UnSpawn on " + gameObject.name);
    }

    private void OnSpawn()
    {
        Debug.Log("Calling Spawn on " + gameObject.name);
    }
}
