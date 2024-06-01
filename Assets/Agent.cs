using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    // Start is called before the first frame update
    public MazeCell currentCell;
    public MazeCell[,] mazeGrid;
    public GameObject redpill;

    public int mazeWidth;
    public int mazeHeight;

    private Stack <MazeCell> cellStack = new Stack<MazeCell>();

    private Queue <MazeCell> cellQueue = new Queue<MazeCell>();

    public GameObject StartToken;
    public GameObject EndToken;



    public bool HasTopWall(int i, int j , MazeCell[,] mazeGrid){
        if (mazeGrid[i, j].Upwall.activeSelf){
            Debug.Log("Up wall active");
            return true;
        }

        return false;
    }

    public bool HasBottomWall(int i, int j , MazeCell[,] mazeGrid){
        if (mazeGrid[i, j].Downwall.activeSelf){
            Debug.Log("Down wall active");
            return true;
        }

        return false;
    }

    public bool HasLeftWall(int i, int j , MazeCell[,] mazeGrid){
        if (mazeGrid[i, j].Leftwall.activeSelf){
            Debug.Log("Left wall active");
            return true;
        }

        return false;
    }

    public bool HasRightWall(int i, int j , MazeCell[,] mazeGrid){
        if (mazeGrid[i, j].Rightwall.activeSelf){
            Debug.Log("Right wall active");
            return true;
        }

        return false;
    }

    public void moveAgentUp(MazeCell[,] mazeGrid)
    {
        int x = (int)redpill.transform.position.x;
        int z = (int)redpill.transform.position.z;
        

        if (!HasTopWall(x, z, mazeGrid) )
        {
            currentCell = mazeGrid[x, z + 1];
            redpill.transform.position = new Vector3(x, 1, z + 1);
        }
       
    }

    public void moveAgentDown(MazeCell[,] mazeGrid)
    {
        int x = (int)redpill.transform.position.x;
        int z = (int)redpill.transform.position.z;
        

        if (!HasBottomWall(x, z, mazeGrid) )
        {
            currentCell = mazeGrid[x, z - 1];
            redpill.transform.position = new Vector3(x, 1, z - 1);
        }
       
    }

    public bool canAgentMoveDirection(string direction, MazeCell[,] mazeGrid){
        int x = (int)redpill.transform.position.x;
        int z = (int)redpill.transform.position.z;
        if (direction == "up"){
            if (!HasTopWall(x, z, mazeGrid)){
                return true;
            }
            return false;
        }
        if (direction == "down"){
            if (!HasBottomWall(x, z, mazeGrid)){
                return true;
            }
            return false;

        }
        if (direction == "left"){
            if (!HasLeftWall(x, z, mazeGrid)){
                return true;
            }
            return false;

        }
        if (direction == "right"){
            if (!HasRightWall(x, z, mazeGrid)){
                return true;
            }
            return false;

        }
        return false;
    }

    public void moveAgentLeft(MazeCell[,] mazeGrid)
    {
        int x = (int)redpill.transform.position.x;
        int z = (int)redpill.transform.position.z;
        

        if (!HasLeftWall(x, z, mazeGrid) )
        {
            currentCell = mazeGrid[x - 1, z];
            redpill.transform.position = new Vector3(x - 1, 1, z);
        }
       
    }

    public void moveAgentRight(MazeCell[,] mazeGrid)
    {
        int x = (int)redpill.transform.position.x;
        int z = (int)redpill.transform.position.z;
        

        if (!HasRightWall(x, z, mazeGrid) )
        {
            currentCell = mazeGrid[x + 1, z];
            redpill.transform.position = new Vector3(x + 1, 1, z);
        }
       
    }

    public void AgentMoveTo(int x, int z){
        redpill.transform.position = new Vector3(x, 1, z);
    }
    public void ResetVisited()
    {
        for (int i = 0; i < mazeGrid.GetLength(0); i++)
        {
            for (int j = 0; j < mazeGrid.GetLength(1); j++)
            {
                mazeGrid[i, j].AgentVisited = false;
            }
        }
    }
    
    public IEnumerator SolveDFS(){
        //so we start at the first cell of the maze[0,0] oe whatever the starting point is
        //we mark the cell as visited
        //then we check the neighbors of the cell
        //so for each left right up and down, we have to first see if there is no wall between those directions, so that we can move to that cell
        //if there is no wall, we check if the cell is visited or not
        //if the cell is not visited, we mark it as visited and move to that cell
        //if the cell is visited, we move to the next cell
        //if there are no neighbors, we backtrack to the previous cell
        //we keep doing this until we reach the end of the maze, or we reach the exit of the maze, which will be passed as a parameter to the function later on

        // ResetVisited();
        //start at the firsts cell
        //mark the cell as visited
        //check the neighbors of the cell
        //if the neighbor is not visited, mark it as visited and move to that cell
        var first = mazeGrid[0, 0];
        first.AgentVisit();
        AgentMoveTo(0, 0);
        Debug.Log("Agent visited first cell");
        cellStack.Push(first);
        while (cellStack.Count > 0){
            var currentCell = cellStack.Pop();
            var neighbors = new List<MazeCell>();
            int x = (int)currentCell.transform.position.x;
            int z = (int)currentCell.transform.position.z;
            AgentMoveTo(x, z);
            // Debug.Log("can move up: " + canAgentMoveDirection("up", mazeGrid));
            //  Debug.Log("Agent visited: " + mazeGrid[x, z+1].AgentVisited);
            if (z < mazeHeight - 1 && canAgentMoveDirection("up", mazeGrid) && !mazeGrid[x, z + 1].AgentVisited){
                Debug.Log("Agent can move up");   
                neighbors.Add(mazeGrid[x, z + 1]);
            }
            else{
                Debug.Log("Agent cannot move up");
            }

            if (z > 0 && canAgentMoveDirection("down", mazeGrid) && !mazeGrid[x, z - 1].AgentVisited){
                Debug.Log("Agent can move down");
                neighbors.Add(mazeGrid[x, z - 1]);
            }
            else{
                Debug.Log("Agent cannot move down");
            }

            if (x > 0 && canAgentMoveDirection("left", mazeGrid) && !mazeGrid[x - 1, z].AgentVisited){
                Debug.Log("Agent can move left");
                neighbors.Add(mazeGrid[x - 1, z]);
            } 
            else{
                Debug.Log("Agent cannot move left");
            }

            if (x < mazeWidth - 1 && canAgentMoveDirection("right", mazeGrid) && !mazeGrid[x + 1, z].AgentVisited){
                Debug.Log("Agent can move right");
                neighbors.Add(mazeGrid[x + 1, z]);
            }
            else{
                Debug.Log("Agent cannot move right");
            }

            for (int i = 0; i < neighbors.Count; i++){
                Debug.Log("Neighbors: " + neighbors[i].transform.position);
            }

            if (neighbors.Count > 0){
                cellStack.Push(currentCell);
                int index = Random.Range(0, neighbors.Count);
                var nextCell = neighbors[index];
                nextCell.AgentVisit();
                cellStack.Push(nextCell);
                AgentMoveTo((int)nextCell.transform.position.x, (int)nextCell.transform.position.z);
                yield return new WaitForSeconds(0.1f);
            }

        }

    }

    public IEnumerator agentGoToFrom(int x1, int z1, int x2, int z2, GameObject startToken, GameObject endToken){
        startToken.transform.position = new Vector3(x1, 3, z1);
        endToken.transform.position = new Vector3(x2, 3, z2);

        startToken.SetActive(true);
        endToken.SetActive(true);
        
        if (x1 < 0 || x1 >= mazeWidth || z1 < 0 || z1 >= mazeHeight || mazeGrid[x1, z1] == null || !mazeGrid[x1, z1].IsVisited){
            Debug.Log("Invalid starting point");
            yield break;
        }

        var startCell = mazeGrid[x1, z1];
        AgentMoveTo(x1, z1);
        startCell.PathFindingVisit();
        cellQueue.Enqueue(startCell);
        while (cellQueue.Count > 0){
            var currentCell = cellQueue.Dequeue();
            AgentMoveTo((int)currentCell.transform.position.x, (int)currentCell.transform.position.z);
            yield return new WaitForSeconds(0.3f);
            if (currentCell.transform.position.x == x2 && currentCell.transform.position.z == z2){
                Debug.Log("Agent reached destination");
                AgentMoveTo((int)currentCell.transform.position.x, (int)currentCell.transform.position.z);
                break;
            }
            
            var BFSneighbors = new List<MazeCell>();
            BFSneighbors = FindNeighbors(BFSneighbors, currentCell);
            while (BFSneighbors.Count > 0){
                var nextCell = BFSneighbors[0];
                nextCell.PathFindingVisit();
                cellQueue.Enqueue(nextCell);

                BFSneighbors.RemoveAt(0);
            }



        }
            

    }


    public IEnumerator agentAstart(int x1, int z1, int x2, int z2, GameObject startToken, GameObject endToken ){
        for (int i = 0; i < mazeWidth; i++){
            for (int j = 0; j < mazeHeight; j++){
                mazeGrid[i, j].g = 999;
                mazeGrid[i, j].f = 999;
                mazeGrid[i, j].h = 0;
            }
        }
        
        
        startToken.transform.position = new Vector3(x1, 3, z1);
        endToken.transform.position = new Vector3(x2, 3, z2);
        startToken.SetActive(true);
        endToken.SetActive(true);
        var openList = new List<MazeCell>();
        var closedList = new List<MazeCell>();
        openList.Add(mazeGrid[x1, z1]);
        mazeGrid[x1, z1].g = 0;
        while (openList.Count > 0){
            var currentCell = openList[0];
            for (int i = 0; i < openList.Count; i++){
                if (openList[i].f < currentCell.f || openList[i].f == currentCell.f && openList[i].h < currentCell.h){
                    currentCell = openList[i];
                }
            }
            if (currentCell.transform.position.x == x2 && currentCell.transform.position.z == z2){
                Debug.Log("Agent reached destination");
                AgentMoveTo((int)currentCell.transform.position.x, (int)currentCell.transform.position.z);
                yield break;
            }
            AgentMoveTo((int)currentCell.transform.position.x, (int)currentCell.transform.position.z);
            yield return new WaitForSeconds(0.3f);
            openList.Remove(currentCell);
            var neighbors = new List<MazeCell>();
            neighbors = FindNeighbors(neighbors, currentCell);
            for (int j = 0; j < neighbors.Count; j++){
                var neighbor = neighbors[j];
                int tentativeG = currentCell.g + 1;
                Debug.Log("Tentative G: " + tentativeG);
                Debug.Log("Neighbor G: " + neighbor.g);
                if (tentativeG < neighbor.g ){
                    neighbor.g = tentativeG;
                    neighbor.h = heuristic((int)neighbor.transform.position.x, (int)neighbor.transform.position.z, x2, z2);
                    neighbor.f = neighbor.g + neighbor.h;
                    if (!openList.Contains(neighbor)){
                        openList.Add(neighbor);
                    }
                
                
                }


            }



        }


    }


    public List <MazeCell> FindNeighbors(List <MazeCell> neighbors, MazeCell currentCell){
        int x = (int)currentCell.transform.position.x;
        int z = (int)currentCell.transform.position.z;
            // AgentMoveTo(x, z);
            // Debug.Log("can move up: " + canAgentMoveDirection("up", mazeGrid));
            //  Debug.Log("Agent visited: " + mazeGrid[x, z+1].AgentVisited);
            if (z < mazeHeight - 1 && canAgentMoveDirection("up", mazeGrid) && !mazeGrid[x, z + 1].PathFindingVisited){
                Debug.Log("Agent can move up");   
                neighbors.Add(mazeGrid[x, z + 1]);
            }
            else{
                Debug.Log("Agent cannot move up");
            }

            if (z > 0 && canAgentMoveDirection("down", mazeGrid) && !mazeGrid[x, z - 1].PathFindingVisited){
                Debug.Log("Agent can move down");
                neighbors.Add(mazeGrid[x, z - 1]);
            }
            else{
                Debug.Log("Agent cannot move down");
            }

            if (x > 0 && canAgentMoveDirection("left", mazeGrid) && !mazeGrid[x - 1, z].PathFindingVisited){
                Debug.Log("Agent can move left");
                neighbors.Add(mazeGrid[x - 1, z]);
            } 
            else{
                Debug.Log("Agent cannot move left");
            }

            if (x < mazeWidth - 1 && canAgentMoveDirection("right", mazeGrid) && !mazeGrid[x + 1, z].PathFindingVisited){
                Debug.Log("Agent can move right");
                neighbors.Add(mazeGrid[x + 1, z]);
            }
            else{
                Debug.Log("Agent cannot move right");
            }

            for (int i = 0; i < neighbors.Count; i++){
                Debug.Log("Neighbors: " + neighbors[i].transform.position);
            }

            return neighbors;




    }


    public int heuristic (int x1, int z1, int x2, int z2){
        //heuristic function to calculate the distance between two points
        //we use the manhattan distance
        //the manhattan distance is the sum of the absolute values of the differences of the x and y coordinates
        int h = Mathf.Abs(x1 - x2) + Mathf.Abs(z1 - z2);
        return h;
    }
}
