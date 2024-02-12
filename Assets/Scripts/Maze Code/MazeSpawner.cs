using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeSpawner : MonoBehaviour
{
    public enum MazeGenerationAlgorithm
    {
        PureRecursive
    }

    [Header("Algorithm")]
    [Tooltip("The spesific algorithm that the maze generation that has been chosen to generate the maze.")]
    public MazeGenerationAlgorithm algorithm = MazeGenerationAlgorithm.PureRecursive;


    [Header("Randomization")]
    [Tooltip("Does the maze need to be generated fully random?")]
    public bool fullRandom = false;
    
    [Tooltip("The standard seed if the maze is not full randomly generated.")]
    public int randomSeed = 12345;


    [Header("Prefabs")]
    [Tooltip("The prefab for the wall object.")]
    public GameObject wallPrefab = null;
    
    [Tooltip("The prefab for the pillar object.")]
    public GameObject pillarPrefab = null;

    [Tooltip("The finish that the player needs to reach.")]
    public GameObject finishPrefab = null;


    [Header("Maze Stats")]
    [Tooltip("The amount of rows the maze needs to have.")]
    public int rows;
    
    [Tooltip("The amount of columns the maze needs to have.")]
    public int columns;
    
    [Tooltip("The width of the tiles.")]
    public float tileWidth = 5;
    
    [Tooltip("The height of the tiels.")]
    public float tileHeight = 5;
    
    [Tooltip("Do there need to be gaps between the walls?")]
    public bool addGaps = false;
    
    [Tooltip("The maze generator.")]
    private MazeGenerator _mazeGenerator = null;
    
    [Tooltip("The tile of which we are getting information from.")]
    private Tile _tile;

    void Start()
    {
        //If it is not a random generated maze gointinue with a seed.
        if (!fullRandom)
        {
            Random.InitState(randomSeed);
        }
        //Gets the algorith for the maze.
        _mazeGenerator = new RecursiveMazeAlgorithm(rows, columns);

        //Generate the maze
        _mazeGenerator.GenerateMaze();

        //For each tile the walls need to be instantiated.
        for (int row = 0; row < rows; row++)
        {
            for (int column = 0; column < columns; column++)
            {
                //Current cell and current position.
                float x = column * (tileWidth + (addGaps ? .2f : 0));
                float z = row * (tileHeight + (addGaps ? .2f : 0));
                _tile = _mazeGenerator.GetMazeCell(row, column);
                _tile.x = row;
                _tile.y = column;

                GameObject tmp;

                //Instantiate the walls based on what walls need to be enabled.
                if (_tile.WallRight)
                {
                    tmp = Instantiate(wallPrefab, new Vector3(x + tileWidth / 2, 1.5f, z) + wallPrefab.transform.position, Quaternion.Euler(0, 90, 0));// right
                    tmp.transform.parent = transform;
                    
                }
                if (_tile.WallFront)
                {
                    tmp = Instantiate(wallPrefab, new Vector3(x, 1.5f, z + tileHeight / 2) + wallPrefab.transform.position, Quaternion.Euler(0, 0, 0));// front
                    tmp.transform.parent = transform;
                    
                }
                if (_tile.WallLeft)
                {
                    tmp = Instantiate(wallPrefab, new Vector3(x - tileWidth / 2, 1.5f, z) + wallPrefab.transform.position, Quaternion.Euler(0, 270, 0));// left
                    tmp.transform.parent = transform;
                    
                }
                if (_tile.WallBack)
                {
                    tmp = Instantiate(wallPrefab, new Vector3(x, 1.5f, z - tileHeight / 2) + wallPrefab.transform.position, Quaternion.Euler(0, 180, 0));// back
                    tmp.transform.parent = transform;
                    
                }
                //If it is the last row.
                if (row >= rows - 1)
                {
                    //Get the center column tile of the last row of the maze.
                    if (column == Mathf.RoundToInt(columns / 2))
                    {
                        //If the goal prefab is set place it in the center of the maze.
                        if (finishPrefab != null)
                        {
                            tmp = Instantiate(finishPrefab, new Vector3(x, 0.5f, z), Quaternion.Euler(0, 0, 0));
                            tmp.transform.parent = transform;
                        }
                    }
                }
            }
        }
        //If there is a pillar prefab the pillars are placed on each corner of the maze so the corners look better and there are no gaps between the walls of the maze.
        if (pillarPrefab != null)
        {
            for (int row = 0; row < rows + 1; row++)
            {
                for (int column = 0; column < columns + 1; column++)
                {
                    //Calculate the position of the walls.
                    float x = column * (tileWidth + (addGaps ? .2f : 0));
                    float z = row * (tileHeight + (addGaps ? .2f : 0));
                    GameObject tmp = Instantiate(pillarPrefab, new Vector3(x - tileWidth / 2, 0, z - tileHeight / 2), pillarPrefab.transform.rotation);
                    tmp.transform.parent = transform;
                }
            }
        }
    }
}
