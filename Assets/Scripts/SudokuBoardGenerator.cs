using System.Collections.Generic;
using UnityEngine;

public class SudokuBoardGenerator
{
    private int[,] solvedBoard;
    private int[,] unsolvedBoard;
    public int BoardSize { get; set; }
    public int[,] GenerateSolved()
    {
        solvedBoard = new int[BoardSize, BoardSize];
        GenerateHelper(0, 0);
        return solvedBoard;
    }

    public int[,] GenerateUnSolved(int numNonZeroVars)
    {
        unsolvedBoard = new int[BoardSize, BoardSize];
        GenerateHelper(0, 0);

        // copy the solved board to the unsolved board
        for (int i = 0; i < BoardSize; i++)
        {
            for (int j = 0; j < BoardSize; j++)
            {
                unsolvedBoard[i, j] = solvedBoard[i, j];
            }
        }

        // create a list of all cell positions
        List<Vector2Int> cellPositions = new List<Vector2Int>();
        for (int i = 0; i < BoardSize; i++)
        {
            for (int j = 0; j < BoardSize; j++)
            {
                cellPositions.Add(new Vector2Int(i, j));
            }
        }

        // shuffle the list of cell positions
        for (int i = 0; i < cellPositions.Count; i++)
        {
            int randomIndex = UnityEngine.Random.Range(i, cellPositions.Count);
            Vector2Int temp = cellPositions[i];
            cellPositions[i] = cellPositions[randomIndex];
            cellPositions[randomIndex] = temp;
        }

        // remove some numbers to create the unsolved board
        int numToRemove = BoardSize * BoardSize - numNonZeroVars;
        for (int i = 0; i < numToRemove; i++)
        {
            Vector2Int cellPosition = cellPositions[i];
            int rowToRemove = cellPosition.x;
            int colToRemove = cellPosition.y;
            unsolvedBoard[rowToRemove, colToRemove] = 0;
        }

        return unsolvedBoard;
    }

    private bool GenerateHelper(int row, int col)
    {
        if (col == BoardSize)
        {
            col = 0;
            row++;

            if (row == BoardSize)
            {
                return true;
            }
        }

        List<int> possibleValues = GetPossibleValues(row, col);
        while (possibleValues.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, possibleValues.Count);
            int value = possibleValues[randomIndex];
            solvedBoard[row, col] = value;

            if (GenerateHelper(row, col + 1))
            {
                return true;
            }

            solvedBoard[row, col] = 0;
            possibleValues.RemoveAt(randomIndex);
        }

        return false;
    }

    private List<int> GetPossibleValues(int row, int col)
    {
        List<int> possibleValues = new List<int>();

        for (int i = 1; i <= BoardSize; i++)
        {
            possibleValues.Add(i);
        }

        for (int i = 0; i < BoardSize; i++)
        {
            possibleValues.Remove(solvedBoard[row, i]);
            possibleValues.Remove(solvedBoard[i, col]);
        }

        int boxRow = (row / 3) * 3;
        int boxCol = (col / 3) * 3;
        for (int i = boxRow; i < boxRow + 3; i++)
        {
            for (int j = boxCol; j < boxCol + 3; j++)
            {
                possibleValues.Remove(solvedBoard[i, j]);
            }
        }

        return possibleValues;
    }
}
