using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    public float speed = 10;

    void Start()
    {
        //transform.position = GameObject.Find("Player").transform.position + new Vector3(0,-1.2f,0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            speed = 20;
        }
        else
        {
            speed = 10;
        }

        transform.position = transform.position + new Vector3(0, 0, Time.deltaTime * speed);
    }
}
