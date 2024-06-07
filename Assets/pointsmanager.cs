using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class pointsmanager : MonoBehaviour
{
    // Start is called before the first frame update
    public int points = 0;
    TMP_Text pointstext;
    void Start()
    {
        pointstext = gameObject.GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void incrementPoints(){
        points++;
        pointstext.text = "Points: " + points;
    }
}
