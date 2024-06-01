using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScale : MonoBehaviour
{
    public GameObject camera;
    public int mazeWidth;
    public int mazeHeight;

    
    void Start()
    {
        camera.transform.position = new Vector3(mazeWidth/2, mazeWidth, 1);
    }

    // Update is called once per frame
    
}
