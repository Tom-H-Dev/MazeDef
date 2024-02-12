using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MazeGenerator : MonoBehaviour
{
    //Used to obtain the Row and Column from the private variables .
    [Tooltip("The amount of rows that are in the maze.")]
    public int RowCount { get { return _mazeRows; } }

    [Tooltip("The amount of columns that are in the maze.")]
    public int ColumnCount { get { return _mazeColumns; } }

    [Tooltip("The amount of rows.")]
    private int _mazeRows;

    [Tooltip("The amount of culums.")]
    private int _mazeColumns;

    [Tooltip("A 2d array of all tiles of the maze.")]
    public Tile[,] mazeTiles;

    //A constructor that makes the rows and columns non-zero.
    //and instantiates a new MazeCell at that specific rank and range.
    public MazeGenerator(int l_rows, int l_columns)
    {
        _mazeRows = Mathf.Abs(l_rows);
        _mazeColumns = Mathf.Abs(l_columns);
        //If there is no size set always set to a 1x1 maze.
        if (_mazeRows == 0)
        {
            _mazeRows = 1;
        }
        if (_mazeColumns == 0)
        {
            _mazeColumns = 1;
        }

        mazeTiles = new Tile[l_rows, l_columns];
        //Generates all the tiles in the 2d array of mMaze.
        for (int row = 0; row < l_rows; row++)
        {
            for (int column = 0; column < l_columns; column++)
            {
                mazeTiles[row, column] = new Tile();
            }
        }
    }
    //called by the algorithim class to start the algorithm.
    public abstract void GenerateMaze();

    /// <summary>
    /// Returns a specific tile
    /// </summary>
    /// <param name="l_row"></ Which row the tile is in.>
    /// <param name="l_column"></Which column thte tile is in.>
    /// <returns></returns>
    /// <exception cref="System.ArgumentOutOfRangeException"></exception>
    public Tile GetMazeCell(int l_row, int l_column)
    {
        if (l_row >= 0 && l_column >= 0 && l_row < _mazeRows && l_column < _mazeColumns)
        {
            return mazeTiles[l_row, l_column];
        }
        else
        {
            Debug.Log(l_row + " " + l_column);
            throw new System.ArgumentOutOfRangeException();
        }
    }
}
