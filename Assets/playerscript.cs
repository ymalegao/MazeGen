using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerscript : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject start;

    void Start()
    {   
        //transform.position = new Vector3(-10, 10, 0);
        start = GameObject.Find("first");
        gameObject.transform.position = new Vector3(start.transform.position.x, start.transform.position.y + 10, start.transform.position.z);
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey("a")){
            transform.Translate(-0.01f, 0, 0);
        }
        if(Input.GetKey("d")){
            transform.Translate(0.01f, 0, 0);
        }
        if(Input.GetKey("w")){
            transform.Translate(0, 0, 0.01f);
        }
        if(Input.GetKey("s")){
            transform.Translate(0, 0, -0.01f);
        }
    }
}
