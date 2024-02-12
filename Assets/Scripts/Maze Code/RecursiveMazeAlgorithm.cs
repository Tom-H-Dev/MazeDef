using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecursiveMazeAlgorithm : MazeGenerator
{
    /// <summary>
    /// Calls the MazeGenerator contructor and sets sets the rows and columns in the MazeGenerator script.
    /// </summary>
    /// <param name="l_rows"></The rows of the maze.>
    /// <param name="l_columns"></The columns of the maze.>
    public RecursiveMazeAlgorithm(int l_rows, int l_columns) : base(l_rows, l_columns)
    {
    }

    //Generates the maze
    public override void GenerateMaze()
    {
        VisitCell(0, 0, Direction.Start);
    }

    /// <summary>
    /// Generates the maze and will make a path trough the maze.
    /// </summary>
    /// <param name="l_row"></The row of the tile which the path is being made.>
    /// <param name="l_column"></The column of the tile which the path is being made.>
    /// <param name="l_moveMade"></The direction the current visited tile has been from so it doesn't go back into itself.>
    private void VisitCell(int l_row, int l_column, Direction l_moveMade)
    {
        Direction[] l_movesAvailable = new Direction[4];
        int l_movesAvailableCount = 0;

        do
        {
            l_movesAvailableCount = 0;
            //check move right
            if (l_column + 1 < ColumnCount && !GetMazeCell(l_row, l_column + 1).IsVisited)
            {
                l_movesAvailable[l_movesAvailableCount] = Direction.Right;
                l_movesAvailableCount++;
            }
            else if (!GetMazeCell(l_row, l_column).IsVisited && l_moveMade != Direction.Left)
            {
                GetMazeCell(l_row, l_column).WallRight = true;
            }
            //check move forward
            if (l_row + 1 < RowCount && !GetMazeCell(l_row + 1, l_column).IsVisited)
            {
                l_movesAvailable[l_movesAvailableCount] = Direction.Front;
                l_movesAvailableCount++;
            }
            else if (!GetMazeCell(l_row, l_column).IsVisited && l_moveMade != Direction.Back)
            {
                GetMazeCell(l_row, l_column).WallFront = true;
            }
            //check move left
            if (l_column > 0 && l_column - 1 >= 0 && !GetMazeCell(l_row, l_column - 1).IsVisited)
            {
                l_movesAvailable[l_movesAvailableCount] = Direction.Left;
                l_movesAvailableCount++;
            }
            else if (!GetMazeCell(l_row, l_column).IsVisited && l_moveMade != Direction.Right)
            {
                GetMazeCell(l_row, l_column).WallLeft = true;
            }
            //check move backward
            if (l_row > 0 && l_row - 1 >= 0 && !GetMazeCell(l_row - 1, l_column).IsVisited)
            {
                l_movesAvailable[l_movesAvailableCount] = Direction.Back;
                l_movesAvailableCount++;
            }
            else if (!GetMazeCell(l_row, l_column).IsVisited && l_moveMade != Direction.Front)
            {
                GetMazeCell(l_row, l_column).WallBack = true;
            }
            GetMazeCell(l_row, l_column).IsVisited = true;

            //If there are no possible move left anymore.
            if (l_movesAvailableCount > 0)
            {
                //Gets a random direction.
                switch (l_movesAvailable[Random.Range(0, l_movesAvailableCount)])
                {
                    case Direction.Start:
                        break;
                    case Direction.Right:
                        VisitCell(l_row, l_column + 1, Direction.Right);
                        break;
                    case Direction.Front:
                        VisitCell(l_row + 1, l_column, Direction.Front);
                        break;
                    case Direction.Left:
                        VisitCell(l_row, l_column - 1, Direction.Left);
                        break;
                    case Direction.Back:
                        VisitCell(l_row - 1, l_column, Direction.Back);
                        break;
                }
            }

        } while (l_movesAvailableCount > 0);
    }
}
