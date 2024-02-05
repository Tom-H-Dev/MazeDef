using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MazeSpawner : MonoBehaviour
{
    public enum MazeGenerationAlgorithm
    {
        PureRecursive
    }
    public MazeGenerationAlgorithm Algorithm = MazeGenerationAlgorithm.PureRecursive;
    public bool FullRandom = false;
    public int RandomSeed = 12345;
    public GameObject Wall = null;
    public GameObject Pillar = null;
    public int Rows;
    public int Columns;
    public float CellWidth = 5;
    public float CellHeight = 5;
    public bool AddGaps = false;
    public GameObject GoalPrefab = null;
    private MazeGenerator mMazeGenerator = null;
    private Tile cell;

    void Start()
    {
        if (!FullRandom)
        {
            Random.InitState(RandomSeed);
        }
        switch (Algorithm)
        {
            case MazeGenerationAlgorithm.PureRecursive:
                mMazeGenerator = new RecursiveMazeAlgorithm(Rows, Columns);
                break;
        }
        mMazeGenerator.GenerateMaze();
        for (int row = 0; row < Rows; row++)
        {
            for (int column = 0; column < Columns; column++)
            {
                float x = column * (CellWidth + (AddGaps ? .2f : 0));
                float z = row * (CellHeight + (AddGaps ? .2f : 0));
                cell = mMazeGenerator.GetMazeCell(row, column);
                cell.x = row;
                cell.y = column;

                GameObject tmp;

                if (cell.WallRight)
                {
                    tmp = Instantiate(Wall, new Vector3(x + CellWidth / 2, 1.5f, z) + Wall.transform.position, Quaternion.Euler(0, 90, 0)) as GameObject;// right
                    tmp.transform.parent = transform;
                    
                }
                if (cell.WallFront)
                {
                    tmp = Instantiate(Wall, new Vector3(x, 1.5f, z + CellHeight / 2) + Wall.transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;// front
                    tmp.transform.parent = transform;
                    
                }
                if (cell.WallLeft)
                {
                    tmp = Instantiate(Wall, new Vector3(x - CellWidth / 2, 1.5f, z) + Wall.transform.position, Quaternion.Euler(0, 270, 0)) as GameObject;// left
                    tmp.transform.parent = transform;
                    
                }
                if (cell.WallBack)
                {
                    tmp = Instantiate(Wall, new Vector3(x, 1.5f, z - CellHeight / 2) + Wall.transform.position, Quaternion.Euler(0, 180, 0)) as GameObject;// back
                    tmp.transform.parent = transform;
                    
                }
                if (row >= Rows - 1)
                {
                    if (column == Mathf.RoundToInt(Columns / 2))
                    {
                        if (GoalPrefab != null)
                        {
                            tmp = Instantiate(GoalPrefab, new Vector3(x, 0.5f, z), Quaternion.Euler(0, 0, 0)) as GameObject;
                            tmp.transform.parent = transform;
                        }
                    }
                }
            }
        }
        if (Pillar != null)
        {
            for (int row = 0; row < Rows + 1; row++)
            {
                for (int column = 0; column < Columns + 1; column++)
                {
                    float x = column * (CellWidth + (AddGaps ? .2f : 0));
                    float z = row * (CellHeight + (AddGaps ? .2f : 0));
                    GameObject tmp = Instantiate(Pillar, new Vector3(x - CellWidth / 2, 0, z - CellHeight / 2), Pillar.transform.rotation) as GameObject;
                    tmp.transform.parent = transform;
                }
            }
        }
    }
}