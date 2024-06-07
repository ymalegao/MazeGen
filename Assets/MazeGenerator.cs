using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MazeGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private MazeCell mazeCellPrefab;
    [SerializeField] private int mazeWidth;
    [SerializeField] private int mazeHeight;
    public GameObject myPlayer;
    private Stack <MazeCell> cellStack = new Stack<MazeCell>();

    private MazeCell lastCell;

    public TextMeshProUGUI text;

    private MazeCell[,] mazeGrid;

    public GameObject redpill;

    public GameObject otherPill;

    public GameObject bluepill;

    public GameObject startToken;

    public GameObject endToken;
    public Agent agentComponent;  

    public Agent agenttwo;

    Coroutine followPathCoroutine;

    private List <MazeCell> WallCells = new List<MazeCell>();

    int centralBoxWidth = 3;
    int centralBoxHeight = 3;

    public Material startMaterial;
    
    public void Start()
    {
        mazeGrid = new MazeCell[mazeWidth, mazeHeight];
        for (int i = 0; i < mazeWidth; i++)
        {
            for (int j = 0; j < mazeHeight; j++)
            {
                mazeGrid[i, j] = Instantiate(mazeCellPrefab, new Vector3(i, 0, j), Quaternion.identity);
                if((i == 0) && (j == 0)){
                    mazeGrid[i, j].name = "first";
                }
            }
        }
        

        float r = Random.Range(0f, 1f);
        float g = Random.Range(0f, 1f);
        float b = Random.Range(0f, 1f);

        startMaterial.color = new Color(r, g, b); 
        
        CreateCentralBox();
         
        
        var first = mazeGrid[0, 0];
        var second = mazeGrid[mazeWidth - 1, mazeHeight - 1];
        first.Leftwall.SetActive(false);
        // Agent.transform.position = new Vector3(0, 1, 0);
        int firstx = (int)first.transform.position.x;
        int firstz = (int)first.transform.position.z;
        int secondx = (int)second.transform.position.x;
        int secondz = (int)second.transform.position.z;
        redpill.transform.position = new Vector3(firstx, 1, firstz);

        GenerateMaze();
        
        agentComponent = gameObject.AddComponent<Agent>();        
        agentComponent.currentCell = mazeGrid[0, 0];
        
        agentComponent.mazeGrid = mazeGrid;
        agentComponent.redpill = redpill;
        agentComponent.mazeWidth = mazeWidth;
        agentComponent.mazeHeight = mazeHeight;
        agentComponent.StartToken = startToken;
        agentComponent.EndToken = endToken;
        agentComponent.PlayerPill = bluepill;
        agentComponent.livesText = text;

        // agenttwo = gameObject.AddComponent<Agent>();        
        // agenttwo.currentCell = mazeGrid[mazeWidth-1, mazeHeight-1];
        // agenttwo.transform.position = new Vector3(mazeWidth-1, 1, mazeHeight-1);
        
        // agenttwo.mazeGrid = mazeGrid;
        // agenttwo.redpill = otherPill;
        // agenttwo.mazeWidth = mazeWidth;
        // agenttwo.mazeHeight = mazeHeight;
        // agenttwo.StartToken = startToken;
        // agenttwo.EndToken = endToken;
        // agenttwo.PlayerPill = bluepill;
        // agentComponent.moveAgentUp(mazeGrid);
        // StartCoroutine(agentComponent.SolveDFS());
        
        int targetx = Random.Range(0, mazeWidth - 1);
        int targetz = Random.Range(0, mazeHeight - 1);
        bluepill.transform.position = new Vector3(targetx, 1, targetz);

        Debug.Log(targetx);
        Debug.Log(targetz);
        agentComponent.startAstar(firstx, firstz, targetx, targetz, startToken, endToken);
        
        // agenttwo.startAstar(secondx, secondz, targetx, targetz, startToken, endToken);
        // StartCoroutine(agentComponent.agentAstart(0,0, targetx,targetz, startToken, endToken));
    
    }
     void CreateCentralBox()
    {
        int startX = (mazeWidth - centralBoxWidth) / 2;
        int startZ = (mazeHeight - centralBoxHeight) / 2;

        for (int i = startX; i < startX + centralBoxWidth; i++)
        {
            for (int j = startZ; j < startZ + centralBoxHeight; j++)
            {
                mazeGrid[i, j].ClearAllWalls();
            }
        }
       
        Instantiate(myPlayer);
 
    }

    public void GenerateMaze(){
        //start at random cell
        //mark cell as visited
        //push cell to stack
        
        // var first = mazeGrid[Random.Range(0, mazeWidth), Random.Range(0, mazeHeight)];
        var first = mazeGrid[0, 0];
        lastCell = first;
        // first.Leftwall.SetActive(true);
                

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
        
        
        WallCells = CheckActiveWalls(WallCells);
        // Debug.Log("Wall cell count" + WallCells.Count);
        // Debug.Log("does it contain 0,0" + WallCells.Contains(mazeGrid[0, 0]));
        // Debug.Log("does it contain 10,0" + WallCells.Contains(mazeGrid[0, 0]));

        // Debug.Log (" does it not contain 0,0" + !WallCells.Contains(mazeGrid[0, 0]));
       
        for (int i = 0; i < mazeHeight/2; i++)
        {
            CreateOpenColumn(Random.Range(0, mazeWidth), 0, mazeHeight - 1, WallCells);
            CreateOpenRow(0, Random.Range(0, mazeHeight), Random.Range(1, 3), WallCells);
        }
        ActivateEdgeWall();


    }

    public void CreateOpenRow(int x, int y, int size, List<MazeCell> e)
    {

        for (int i = 0; i < mazeWidth - 1; i++) 
        {
            if (!IsEdgeCell(i, y)) 
            {
                if (i > 0 && !e.Contains(mazeGrid[i - 1, y])) mazeGrid[i - 1, y].ClearRightWall();
                if (i < mazeWidth - 1 && !e.Contains(mazeGrid[i + 1, y])) mazeGrid[i + 1, y].ClearLeftWall();
                if (y > 0 && !e.Contains(mazeGrid[i, y - 1])) mazeGrid[i, y - 1].ClearDownWall();
                if (y < mazeHeight - 1 && !e.Contains(mazeGrid[i, y + 1])) mazeGrid[i, y + 1].ClearUpWall();
            } else{
                clearEdgeWall(i, y);
            
            
            }
        }
    }


    public void CreateOpenColumn(int x, int y, int size, List<MazeCell> e) 
    {
        for (int i = 0; i < mazeHeight - 1; i++) 
        {
            // Debug.Log("is edge cell of 2, 0 " + IsEdgeCell(2, 0));
            if (!IsEdgeCell(x, i)) 
            {
                if (x > 0 && !e.Contains(mazeGrid[x - 1, i])) mazeGrid[x - 1, i].ClearRightWall();
                if (x < mazeWidth - 1 && !e.Contains(mazeGrid[x + 1, i])) mazeGrid[x + 1, i].ClearLeftWall();
                if (i > 0 && !e.Contains(mazeGrid[x, i - 1])) mazeGrid[x, i - 1].ClearDownWall();
                if (i < mazeHeight - 1 && !e.Contains(mazeGrid[x, i + 1])) mazeGrid[x, i + 1].ClearUpWall();
            } else{
                clearEdgeWall(x, i);
            }
        }


    
    }

    public void CreateOpenArea(int x, int y, int size) {
    for (int i = x; i < x + size && i < mazeWidth; i++) {
        for (int j = y; j < y + size && j < mazeHeight; j++) {
            if (i > 0) mazeGrid[i - 1, j].ClearRightWall();
            if (i < mazeWidth - 1) mazeGrid[i + 1, j].ClearLeftWall();
            if (j > 0) mazeGrid[i, j - 1].ClearDownWall();
            if (j < mazeHeight - 1) mazeGrid[i, j + 1].ClearUpWall();
        }
    }
    }
    


    private void ActivateEdgeWall(){
        //this fuction will clear the walls of a cell that is on the edge of the maze without clearing the wall that is on the edge of the maze
        //for example, if the cell is on the left side, the bottom can be clered, the top can be cleared, and the right can be clearaed, but the left wall cannot be cleared since it is on the edge of the maze
        for (int x = 0; x < mazeWidth; x++)
        {
            for (int z = 0; z < mazeHeight; z++)
            {
                if (x == 0){
                    mazeGrid[x, z].Leftwall.SetActive(true);
                }
                if (x == mazeWidth - 1){
                    mazeGrid[x, z].Rightwall.SetActive(true);
                }

                if (z == 0){
                    mazeGrid[x, z].Downwall.SetActive(true);
                }

                if (z == mazeHeight - 1){
                    mazeGrid[x, z].Upwall.SetActive(true);
                }
            }
        }
        
    }

    private void clearEdgeWall(int x, int z){
        //this fuction will clear the walls of a cell that is on the edge of the maze without clearing the wall that is on the edge of the maze
        //for example, if the cell is on the left side, the bottom can be clered, the top can be cleared, and the right can be clearaed, but the left wall cannot be cleared since it is on the edge of the maze

        if (x == 0){
            mazeGrid[x, z].ClearRightWall();
            mazeGrid[x, z].ClearUpWall();
            mazeGrid[x, z].ClearDownWall();

        }
        if (x == mazeWidth - 1){
            mazeGrid[x, z].ClearLeftWall();
            mazeGrid[x, z].ClearUpWall();
            mazeGrid[x, z].ClearDownWall();
        }

        if (z == 0){
            
            mazeGrid[x, z].ClearRightWall();
            mazeGrid[x, z].ClearLeftWall();
            mazeGrid[x, z].ClearUpWall();
            mazeGrid[x, z].Downwall.SetActive(true);
        }

        if (z == mazeHeight - 1){
            mazeGrid[x, z].ClearRightWall();
            mazeGrid[x, z].ClearLeftWall();
            mazeGrid[x, z].ClearDownWall();

    }
    }

    private void ClearWalls(MazeCell previous, MazeCell current){
        //if the current cell is to the left of the previous cell
        int prevX = (int)previous.transform.position.x;
        int prevZ = (int)previous.transform.position.z;
        int currX = (int)current.transform.position.x;
        int currZ = (int)current.transform.position.z;

        // if (IsEdgeCell(prevX, prevZ) || IsEdgeCell(currX, currZ))
        // {
        //     clearEdgeWall(prevX, prevZ);
        //     return;
        // }
        
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

    

    private bool IsEdgeCell(int x, int z)
    {
        return x == 0 || z == 0 || x == mazeWidth - 1 || z == mazeHeight - 1;
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

    public void UpdateTargetPosition(int x, int z, Agent agentComponent)
    {
        
        

        if (agentComponent == null){
            Debug.Log("Agent component is null");
        }
        int currentX = (int)redpill.transform.position.x;
        int currentZ = (int)redpill.transform.position.z;
        startToken.transform.position = new Vector3(currentX, 1, currentZ);
        endToken.transform.position = new Vector3(x, 1, z);
        StopAllCoroutines();
        agentComponent.startAstar(currentX, currentZ, x, z, startToken, endToken);
        // Debug.Log("agent courotutine is running");
    }

    // Update is called once per frame
   
    void Update()
    {


       
    }
}
