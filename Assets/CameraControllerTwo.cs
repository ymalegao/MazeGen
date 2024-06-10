using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerTwo : MonoBehaviour
{
    // Start is called before the first frame update

    public int mazeWidth;
    public int mazeHeight;
    void Start()
    {
        //give birds eye view of maze
        transform.position = new Vector3(mazeWidth/2, mazeHeight, mazeWidth/2);
        transform.rotation = Quaternion.Euler(90, 0, 0);

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
