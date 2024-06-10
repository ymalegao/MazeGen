using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScalethree : MonoBehaviour
{
    // Start is called before the first frame update
    public int mazeWidth;
    public int mazeHeight;
    void Start()

    {
        //idk what view this is actually

        transform.position = new Vector3(mazeWidth/2, mazeHeight + 10, mazeWidth/2);
        transform.rotation = Quaternion.Euler(100, 0, 0);        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
