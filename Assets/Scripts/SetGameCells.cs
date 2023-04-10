using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetGameCells : MonoBehaviour
{

    [SerializeField] private GameObject CellPreFeb;
    [SerializeField] float BoarderDistane = 8;
    [SerializeField] private int BoardSize;
    private GameObject[,] cells;
    private int selectedRow = -1;
    private int selectedCol = -1;
    private int[,] solvedBoard;
    private int[,] unsolvedBoard;
    public int numNonZeroVars = 22;
    [SerializeField] Sprite[] numberSprits;
    [SerializeField] Button[] inputButtons;

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
        float cellWidth = (panelWidth / BoardSize) - 4;

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
                Button button = cell.GetComponentInChildren<Button>();
                cells[row, col] = cell;
                if (unsolvedBoard[row, col] > 0)
                {
                    button.GetComponentInChildren<Image>().sprite = numberSprits[unsolvedBoard[row, col]];
                    button.interactable = false;

                }
                else
                {
                    int cellRow = row; // create new local variable and assign loop variable's value to it
                    int cellCol = col; //
                    button.GetComponentInChildren<Image>().sprite = numberSprits[0];
                    button.GetComponentsInChildren<Image>().Where(x => x.gameObject.name == "Lock").FirstOrDefault().enabled = false;
                    button.onClick.AddListener(() => SetSelectCell(cellRow, cellCol));
                }

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

    private void SetSelectCell(int cellRow, int cellCol)
    {
        selectedRow = cellRow;
        selectedCol = cellCol;
    }

    public void OnInput(int inputNumber)
    {
        if (selectedRow > -1)
        {
            Button button = cells[selectedRow, selectedCol].GetComponentInChildren<Button>();
            button.GetComponentInChildren<Image>().sprite = numberSprits[inputNumber];
        }
    }
}
