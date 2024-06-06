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

    [SerializeField] public GameObject startToken;

    [SerializeField] public GameObject endToken;

    [SerializeField] public GameObject pellet;

    [SerializeField] public GameObject powerPellet;


    public int g = 999;
    public int f = 999;
    public int h = 0;

    public MazeCell parent;

    public bool IsVisited { get; set; }

    public bool AgentVisited { get; set; }

    public bool PathFindingVisited { get; set; }


    public void Visit(){
        IsVisited = true;
        Unvisitedblock.SetActive(false);
    }

    public void AgentVisit(){
        AgentVisited = true;
        // Unvisitedblock.SetActive(false);
    }

    public void PathFindingVisit(){
        PathFindingVisited = true;
        // Unvisitedblock.SetActive(false);
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

    public void ClearAllWalls(){
        ClearLeftWall();
        ClearRightWall();
        ClearUpWall();
        ClearDownWall();
    }
    
}
