using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectInfo2 : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject[] coin; 

    public float speed = 2f;

    void Start()
    {
        // Print the GameObject's tag and transform to the console
        Debug.Log("Object's tag: " + gameObject.tag + " \n Object's pos:" + transform.position);

        coin = GameObject.FindGameObjectsWithTag("Coin");

    }

    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(transform.position, coin[0].transform.position);
        Debug.Log("Dist:  " + dist);


        transform.position += Vector3.left * speed * Time.deltaTime;

        transform.localScale += new Vector3(0.5f, 0.5f, 0.5f) * Time.deltaTime;
    }
}
