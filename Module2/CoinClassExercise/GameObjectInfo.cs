using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectInfo : MonoBehaviour
{
    // Start is called before the first frame update

    void Start()
    {
        // Print the GameObject's tag and transform to the console
        Debug.Log("Object's tag: " + gameObject.tag + " \n Object's pos:" + transform.position);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
