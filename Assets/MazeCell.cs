using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCell : MonoBehaviour
{
    
    [SerializeField] public GameObject Leftwall;
    [SerializeField] public GameObject Rightwall;
    [SerializeField] public GameObject Upwall;
    [SerializeField] public GameObject Downwall;
        
    [SerializeField] public GameObject Unvisitedblock;

    public bool IsVisited { get; set; }

    public void Visit(){
        IsVisited = true;
        Unvisitedblock.SetActive(false);
    }

    public void ClearLeftWall(){
        Leftwall.SetActive(false);
    }

    public void ClearRightWall(){
        Rightwall.SetActive(false);
    }

    public void ClearUpWall(){
        Upwall.SetActive(false);
    }

    public void ClearDownWall(){
        Downwall.SetActive(false);
    }





    
}
