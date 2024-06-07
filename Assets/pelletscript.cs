using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pelletscript : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject points;
    void Start()
    {
        points = GameObject.Find("Points");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        points.GetComponent<pointsmanager>().incrementPoints();
        Destroy(gameObject);
    }
}
