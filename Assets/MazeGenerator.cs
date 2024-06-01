using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private MazeCell mazeCellPrefab;
    [SerializeField] private int mazeWidth;
    [SerializeField] private int mazeHeight;

    private Stack <MazeCell> cellStack = new Stack<MazeCell>();

    private MazeCell lastCell;

    private MazeCell[,] mazeGrid;

    public GameObject redpill;

    public GameObject startToken;

    public GameObject endToken;



    
    
    public void Start()
    {
        mazeGrid = new MazeCell[mazeWidth, mazeHeight];
        for (int i = 0; i < mazeWidth; i++)
        {
            for (int j = 0; j < mazeHeight; j++)
            {
                mazeGrid[i, j] = Instantiate(mazeCellPrefab, new Vector3(i, 0, j), Quaternion.identity);
            }
        } 

        var first = mazeGrid[0, 0];
        // Agent.transform.position = new Vector3(0, 1, 0);
        int firstx = (int)first.transform.position.x;
        int firstz = (int)first.transform.position.z;
        redpill.transform.position = new Vector3(firstx, 1, firstz);
        GenerateMaze();
        Agent agentComponent = gameObject.AddComponent<Agent>();        
        agentComponent.currentCell = mazeGrid[0, 0];
        
        agentComponent.mazeGrid = mazeGrid;
        agentComponent.redpill = redpill;
        agentComponent.mazeWidth = mazeWidth;
        agentComponent.mazeHeight = mazeHeight;
        // agentComponent.moveAgentUp(mazeGrid);
        // StartCoroutine(agentComponent.SolveDFS());
        int targetx = Random.Range(0, mazeWidth - 1);
        int targetz = Random.Range(0, mazeHeight - 1);

        Debug.Log(targetx);
        Debug.Log(targetz);
        StartCoroutine(agentComponent.agentAstart(0,0, targetx,targetz, startToken, endToken));
    }

    public void GenerateMaze(){
        //start at random cell
        //mark cell as visited
        //push cell to stack
        
        // var first = mazeGrid[Random.Range(0, mazeWidth), Random.Range(0, mazeHeight)];
        var first = mazeGrid[0, 0];
        lastCell = first;
        first.ClearLeftWall();
        first.Visit();
        cellStack.Push(first);
        while (cellStack.Count > 0) {

            var currentCell = cellStack.Pop();
            var neighbors = new List<MazeCell>();
            int x = (int)currentCell.transform.position.x;
            int z = (int)currentCell.transform.position.z;

            //check cell to the left if it is not visited and not on the left edge
            if (x > 0 && !mazeGrid[x-1, z].IsVisited){
                neighbors.Add(mazeGrid[x-1, z]);
            }

            //check cell to the right if it is not visited and not on the right edge
            if (x < mazeWidth - 1 && !mazeGrid[x+1, z].IsVisited){
                neighbors.Add(mazeGrid[x+1, z]);
            }

            if (z > 0 && !mazeGrid[x, z-1].IsVisited){
                neighbors.Add(mazeGrid[x, z-1]);
            }

            if (z < mazeHeight - 1 && !mazeGrid[x, z+1].IsVisited){
                neighbors.Add(mazeGrid[x, z+1]);
            }
            
            if (neighbors.Count > 0){
                cellStack.Push(currentCell);
                //for indexes, 0 will be left, 1 will be right, 2 will be up, 3 will be down
                int index = Random.Range(0, neighbors.Count);
                var nextCell = neighbors[index];
                ClearWalls(currentCell, nextCell);
                // 
                nextCell.Visit();
                cellStack.Push(nextCell);
                lastCell = nextCell;
            }

        }
        Debug.Log("Maze Generated");
        Debug.Log(lastCell.transform.position.x);
        if (lastCell.transform.position.x < mazeWidth - 1){
            lastCell.ClearRightWall();
        }
        if (lastCell.transform.position.z < mazeHeight - 1){
            lastCell.ClearDownWall();
        }

        if (lastCell.transform.position.x > 0){
            lastCell.ClearLeftWall();
        }
        if (lastCell.transform.position.z > 0){
            lastCell.ClearUpWall();
        }

        List<MazeCell> e = new List<MazeCell>();
        e = CheckActiveWalls(e);
        Debug.Log(e.Count);


       
    }
    

    private void ClearWalls(MazeCell previous, MazeCell current){
        //if the current cell is to the left of the previous cell
        if (current.transform.position.x < previous.transform.position.x){
            current.ClearRightWall();
            previous.ClearLeftWall();
            return;
        }
        //if the current cell is to the right of the previous cell
        if (current.transform.position.x > previous.transform.position.x){
            current.ClearLeftWall();
            previous.ClearRightWall();
            return;
        }
        //if the current cell is above the previous cell
        if (current.transform.position.z > previous.transform.position.z){
            current.ClearDownWall();
            previous.ClearUpWall();
            return; 
        }
        //if the current cell is below the previous cell
        if (current.transform.position.z < previous.transform.position.z){
            current.ClearUpWall();
            previous.ClearDownWall();
            return;
        }

    }

    public List<MazeCell> CheckActiveWalls(List<MazeCell> e){
        for (int i = 0; i < mazeWidth; i++)
        {
            for (int j = 0; j < mazeHeight; j++)
            {
                int activeWalls = 0;
                if (mazeGrid[i, j].Leftwall.activeSelf){
                    // Debug.Log("Left wall active");
                    activeWalls++;
                }
                if (mazeGrid[i, j].Rightwall.activeSelf){
                    // Debug.Log("Right wall active");
                    activeWalls++;

                }
                if (mazeGrid[i, j].Upwall.activeSelf){
                    // Debug.Log("Up wall active");
                    activeWalls++;
                }
                if (mazeGrid[i, j].Downwall.activeSelf){
                    // Debug.Log("Down wall active");
                    activeWalls++;
                }
                if (activeWalls > 2){
                    e.Add(mazeGrid[i, j]);
                }

            }

            
        }
        return e;


    }

    


    public void regenerateMaze(){
        for (int i = 0; i < mazeWidth; i++)
        {
            for (int j = 0; j < mazeHeight; j++)
            {
                Destroy(mazeGrid[i, j].gameObject);
            }
        } 
        Start();
    }

    // Update is called once per frame
   
    void Update()
    {

    }
}
