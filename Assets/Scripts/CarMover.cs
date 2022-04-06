using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMover : MonoBehaviour
{
    public float speed = 10;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = player.transform.position + new Vector3(0,-1.2f,0);
        //transform.rotation = player.transform.rotation;
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

        transform.Translate(-Vector3.forward * Time.deltaTime * speed);
    }
}
