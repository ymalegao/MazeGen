using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField] private MazeCell mazeCellPrefab;
    [SerializeField] private int mazeWidth;
    [SerializeField] private int mazeHeight;

    private List<MazeCell> wallList = new List<MazeCell>();
    private MazeCell[,] mazeGrid;

    public TextMeshProUGUI text;
    public TextMeshProUGUI scoreText;

    public TextMeshProUGUI TotalScore;
    


    public GameObject redpill;
    public GameObject otherPill;
    public GameObject bluepill;
    public GameObject startToken;
    public GameObject endToken;
    public Agent agentComponent;
    public Agent agenttwo;
    public Material startMaterial;

    public void Start()
    {
        //set total score text size to  0 so its hiddden
        TotalScore.fontSize = 0;

        //set score size to 0

        mazeGrid = new MazeCell[mazeWidth, mazeHeight];
        for (int i = 0; i < mazeWidth; i++)
        {
            for (int j = 0; j < mazeHeight; j++)
            {
                mazeGrid[i, j] = Instantiate(mazeCellPrefab, new Vector3(i, 0, j), Quaternion.identity);
                if (Random.Range(0f, 1f) < 0.9f)
                {
                    mazeGrid[i, j].powerPellet.SetActive(false);
                    mazeGrid[i, j].pellet.SetActive(true);
                }else{
                    mazeGrid[i, j].powerPellet.SetActive(true);
                    mazeGrid[i, j].pellet.SetActive(false);
                }
            }
        }

        float r = Random.Range(0f, 1f);
        float g = Random.Range(0f, 1f);
        float b = Random.Range(0f, 1f);
        startMaterial.color = new Color(r, g, b);

        // CreateCentralBox();
        GenerateMaze();

        var first = mazeGrid[0, 0];
        int firstx = (int)first.transform.position.x;
        int firstz = (int)first.transform.position.z;
        redpill.transform.position = new Vector3(firstx, 1, firstz);
        if (agentComponent != null)
        {
            Destroy(agentComponent);
        }

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
        agentComponent.scoreText = scoreText;
        agentComponent.TotalScore = TotalScore;

        int targetx = Random.Range(0, mazeWidth - 1);
        int targetz = Random.Range(0, mazeHeight - 1);
        bluepill.transform.position = new Vector3(targetx, 1, targetz);

        agentComponent.startAstar(firstx, firstz, targetx, targetz, startToken, endToken);

        // Open up additional spaces
    }

    void CreateCentralBox()
    {
        int startX = (mazeWidth - 3) / 2;
        int startZ = (mazeHeight - 3) / 2;

        for (int i = startX; i < startX + 3; i++)
        {
            for (int j = startZ; j < startZ + 3; j++)
            {
                mazeGrid[i, j].ClearAllWalls();
            }
        }
       
        // Instantiate(myPlayer);
 
    }

    public void GenerateMaze()
    {
        List<MazeCell> frontierCells = new List<MazeCell>();
        MazeCell startCell = mazeGrid[Random.Range(0, mazeWidth), Random.Range(0, mazeHeight)];
        AddFrontierCells(startCell, frontierCells);

        while (frontierCells.Count > 0)
        {
            MazeCell currentCell = frontierCells[Random.Range(0, frontierCells.Count)];
            frontierCells.Remove(currentCell);

            List<MazeCell> neighbors = GetVisitedNeighbors(currentCell);
            if (neighbors.Count > 0)
            {
                MazeCell neighbor = neighbors[Random.Range(0, neighbors.Count)];
                ClearWalls(neighbor, currentCell);
            }
            AddFrontierCells(currentCell, frontierCells);
        }

        RemoveRandomWalls(0.2f);  // Remove 20% of the walls randomly


        ActivateEdgeWall();
    }

    private void AddFrontierCells(MazeCell cell, List<MazeCell> frontierCells)
    {
        int x = (int)cell.transform.position.x;
        int z = (int)cell.transform.position.z;

        if (x > 0 && !mazeGrid[x - 1, z].IsVisited) frontierCells.Add(mazeGrid[x - 1, z]);
        if (x < mazeWidth - 1 && !mazeGrid[x + 1, z].IsVisited) frontierCells.Add(mazeGrid[x + 1, z]);
        if (z > 0 && !mazeGrid[x, z - 1].IsVisited) frontierCells.Add(mazeGrid[x, z - 1]);
        if (z < mazeHeight - 1 && !mazeGrid[x, z + 1].IsVisited) frontierCells.Add(mazeGrid[x, z + 1]);

        cell.Visit();
    }

    private List<MazeCell> GetVisitedNeighbors(MazeCell cell)
    {
        List<MazeCell> neighbors = new List<MazeCell>();
        int x = (int)cell.transform.position.x;
        int z = (int)cell.transform.position.z;

        if (x > 0 && mazeGrid[x - 1, z].IsVisited) neighbors.Add(mazeGrid[x - 1, z]);
        if (x < mazeWidth - 1 && mazeGrid[x + 1, z].IsVisited) neighbors.Add(mazeGrid[x + 1, z]);
        if (z > 0 && mazeGrid[x, z - 1].IsVisited) neighbors.Add(mazeGrid[x, z - 1]);
        if (z < mazeHeight - 1 && mazeGrid[x, z + 1].IsVisited) neighbors.Add(mazeGrid[x, z + 1]);

        return neighbors;
    }

    private void ClearWalls(MazeCell previous, MazeCell current)
    {
        int prevX = (int)previous.transform.position.x;
        int prevZ = (int)previous.transform.position.z;
        int currX = (int)current.transform.position.x;
        int currZ = (int)current.transform.position.z;

        if (currX < prevX) { current.ClearRightWall(); previous.ClearLeftWall(); return; }
        if (currX > prevX) { current.ClearLeftWall(); previous.ClearRightWall(); return; }
        if (currZ > prevZ) { current.ClearDownWall(); previous.ClearUpWall(); return; }
        if (currZ < prevZ) { current.ClearUpWall(); previous.ClearDownWall(); return; }
    }

    private void ActivateEdgeWall()
    {
        for (int x = 0; x < mazeWidth; x++)
        {
            for (int z = 0; z < mazeHeight; z++)
            {
                if (x == 0) mazeGrid[x, z].Leftwall.SetActive(true);
                if (x == mazeWidth - 1) mazeGrid[x, z].Rightwall.SetActive(true);
                if (z == 0) mazeGrid[x, z].Downwall.SetActive(true);
                if (z == mazeHeight - 1) mazeGrid[x, z].Upwall.SetActive(true);
            }
        }
    }

    private void RemoveRandomWalls(float percentage)
    {
        int totalCells = mazeWidth * mazeHeight;
        int wallsToRemove = Mathf.FloorToInt(totalCells * percentage);

        for (int i = 0; i < wallsToRemove; i++)
        {
            int x = Random.Range(0, mazeWidth);
            int z = Random.Range(0, mazeHeight);
            MazeCell cell = mazeGrid[x, z];

            List<string> possibleWalls = new List<string>();

            if (cell.Leftwall.activeSelf) possibleWalls.Add("Left");
            if (cell.Rightwall.activeSelf) possibleWalls.Add("Right");
            if (cell.Upwall.activeSelf) possibleWalls.Add("Up");
            if (cell.Downwall.activeSelf) possibleWalls.Add("Down");

            if (possibleWalls.Count > 0)
            {
                string wallToRemove = possibleWalls[Random.Range(0, possibleWalls.Count)];
                switch (wallToRemove)
                {
                    case "Left":
                        if (x > 0) cell.ClearLeftWall(); break;
                    case "Right":
                        if (x < mazeWidth - 1) cell.ClearRightWall(); break;
                    case "Up":
                        if (z < mazeHeight - 1) cell.ClearUpWall(); break;
                    case "Down":
                        if (z > 0) cell.ClearDownWall(); break;
                }
            }
        }
    }

    public void UpdateTargetPosition(int x, int z, Agent agentComponent)
    {
        if (agentComponent == null)
        {
            Debug.Log("Agent component is null");
        }
        int currentX = (int)redpill.transform.position.x;
        int currentZ = (int)redpill.transform.position.z;
        startToken.transform.position = new Vector3(currentX, 1, currentZ);
        endToken.transform.position = new Vector3(x, 1, z);
        StopAllCoroutines();
        agentComponent.startAstar(currentX, currentZ, x, z, startToken, endToken);
    }


    public void RestartGame()
    {
        Agent existingAgent = gameObject.GetComponent<Agent>();


        if (existingAgent != null)
        {
            Destroy(existingAgent);
        }


        for (int i = 0; i < mazeWidth; i++)
        {
            for (int j = 0; j < mazeHeight; j++)
            {
                mazeGrid[i, j].gameObject.SetActive(false);
            }
        }

    // Clear the wall list
        wallList.Clear();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);


    }

    
}
