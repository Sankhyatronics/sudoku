using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetGameCells : MonoBehaviour
{

    [SerializeField] private GameObject CellPreFeb;
    [SerializeField] float BoarderDistane = 8;
    [SerializeField] private int BoardSize;
    private GameObject[,] cells;
    //private int selectedRow = -1;
    //private int selectedCol = -1;
    private int[,] solvedBoard;
    private int[,] unsolvedBoard;
    public int numNonZeroVars = 22;

   
    void Start()
    {
        var boardGenerator = new SudokuBoardGenerator();
        boardGenerator.BoardSize = BoardSize;
        solvedBoard = boardGenerator.GenerateSolved();
        unsolvedBoard = boardGenerator.GenerateUnSolved(numNonZeroVars);
        CreateCells();
    }

    private void CreateCells()
    {
        RectTransform panelReact = gameObject.GetComponent<RectTransform>();
        //Calculating cell size basedon screen width
        float panelWidth = panelReact.rect.width;
        float cellWidth = (panelWidth / BoardSize) -4;

        cells = new GameObject[BoardSize, BoardSize];
        RectTransform gameCellPrefebTransform = CellPreFeb.GetComponent<RectTransform>();
        gameCellPrefebTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, cellWidth);
        gameCellPrefebTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, cellWidth);

        float statrupXPosition = (cellWidth / 2) + BoarderDistane;
        float statrupYPosition = statrupXPosition;

        for (int row = 0; row < BoardSize; row++)
        {
            for (int col = 0; col < BoardSize; col++)
            {
                GameObject cell;
                cell = Instantiate(CellPreFeb);
                cell.transform.SetParent(gameObject.transform);
                //      cells[row, col] = cell;
                //if (unsolvedBoard[row, col] > 0)
                //{
                //    cells[row, col].GetComponentInChildren<Button>().interactable = false;

                //}
                //else
                //{
                //    int cellRow = row; // create new local variable and assign loop variable's value to it
                //    int cellCol = col; //
                //   cell.GetComponentInChildren<Button>().onClick.AddListener(() => SetSelectCell(cellRow, cellCol));
                //}

                gameCellPrefebTransform = cell.GetComponent<RectTransform>();
                gameCellPrefebTransform.anchoredPosition = new Vector3(statrupXPosition, -statrupYPosition, 0);
                gameCellPrefebTransform.localScale = Vector3.one;


                if ((col + 1) % 3 == 0)
                    statrupXPosition += (cellWidth + BoarderDistane);
                else
                    statrupXPosition += cellWidth;
            }
            if ((row + 1) % 3 == 0)
                statrupYPosition += (cellWidth + BoarderDistane);
            else
                statrupYPosition += cellWidth;
            // Resetting X position for new row
            statrupXPosition = (cellWidth / 2) + BoarderDistane;
        }
    }
}
