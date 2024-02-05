using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MazeGenerator : MonoBehaviour
{
    //Used to obtain the Row and Column from the private variables 
    public int RowCount { get { return mMazeRows; } }
    public int ColumnCount { get { return mMazeColumns; } }
    private int mMazeRows;
    private int mMazeColumns;
    public Tile[,] mMaze;
    //A constructor that makes the rows and columns non-zero
    //and instantiates a new MazeCell at that specific rank and range
    public MazeGenerator(int rows, int columns)
    {
        mMazeRows = Mathf.Abs(rows);
        mMazeColumns = Mathf.Abs(columns);
        if (mMazeRows == 0)
        {
            mMazeRows = 1;
        }
        if (mMazeColumns == 0)
        {
            mMazeColumns = 1;
        }
        mMaze = new Tile[rows, columns];
        for (int row = 0; row < rows; row++)
        {
            for (int column = 0; column < columns; column++)
            {
                mMaze[row, column] = new Tile();
            }
        }
    }
    //called by the algorithim class to start the algorithm
    public abstract void GenerateMaze();

    public Tile GetMazeCell(int row, int column)
    {
        if (row >= 0 && column >= 0 && row < mMazeRows && column < mMazeColumns)
        {
            return mMaze[row, column];
        }
        else
        {
            Debug.Log(row + " " + column);
            throw new System.ArgumentOutOfRangeException();
        }
    }
}
