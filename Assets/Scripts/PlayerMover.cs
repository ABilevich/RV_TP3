using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    public float speed = 10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.anyKey){
            speed = 20;
        }
        else{
            speed = 10;
        }

        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }
}
