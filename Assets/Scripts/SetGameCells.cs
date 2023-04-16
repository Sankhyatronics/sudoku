using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Localization.LocalizationTableCollection;

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
    public int DifficultyLevel { get; set; }
    private int numNonZeroVars = 22;
    [SerializeField] Sprite[] numberSprits;
    // [SerializeField] Button[] inputButtons;

    void Start()
    {
        CreateCells();
        SetGameBoardTiles(true);
    }

    private void CreateCells()
    {
        RectTransform panelReact = gameObject.GetComponent<RectTransform>();
        //Calculating cell size basedon screen width
        float panelWidth = panelReact.rect.width;
        float cellWidth = (panelWidth - (4 * BoarderDistane)) / BoardSize;

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

    public void SetGameBoardTiles(bool isNew)
    {
        if (isNew)
        {
            var boardGenerator = new SudokuBoardGenerator();
            boardGenerator.BoardSize = BoardSize;
            solvedBoard = boardGenerator.GenerateSolved();
            unsolvedBoard = boardGenerator.GenerateUnSolved(numNonZeroVars);
        }
        for (int row = 0; row < BoardSize; row++)
        {
            for (int col = 0; col < BoardSize; col++)
            {

                Button button = cells[row, col].GetComponentInChildren<Button>();
                var cellProperties = button.GetComponent<CellProperties>();
                // button.onClick.RemoveAllListeners();
                cellProperties.UnSolvedValue = unsolvedBoard[row, col];
                cellProperties.SolvedValue = solvedBoard[row, col];
                cellProperties.Row = row;
                cellProperties.Column = col;
                if (unsolvedBoard[row, col] > 0)
                {
                    button.GetComponentInChildren<Image>().sprite = numberSprits[unsolvedBoard[row, col]];
                    button.interactable = false;
                    button.GetComponentsInChildren<Image>().Where(x => x.gameObject.name == "Lock").FirstOrDefault().enabled = true;
                }
                else
                {
                //    int cellRow = row; // create new local variable and assign loop variable's value to it
                //    int cellCol = col; //
                    button.GetComponentInChildren<Image>().sprite = numberSprits[0];
                    button.interactable = true;
                    button.GetComponentsInChildren<Image>().Where(x => x.gameObject.name == "Lock").FirstOrDefault().enabled = false;
                   // button.onClick.AddListener(() => SetSelectCell(cellRow, cellCol));
                }
            }
        }
    }

    private void SetCellColor(GameObject cell, bool resetColor, int correctValue)
    {
        Button button = cell.GetComponentInChildren<Button>();
        int CellValue = button.GetComponent<CellProperties>().Value;
        var colors = button.colors;
        if (resetColor || CellValue == 0)
            colors.normalColor = Color.white;
        else if (CellValue == correctValue)
            colors.normalColor = Color.green;
        else
            colors.normalColor = Color.red;
        button.colors = colors;
    }


    public void ShowGameStatus()
    {
        for (int row = 0; row < BoardSize; row++)
        {
            for (int col = 0; col < BoardSize; col++)
            {
                SetCellColor(cells[row, col], false, solvedBoard[row, col]);
            }
        }
    }
    private void SetSelectCell(int cellRow, int cellCol)
    {
        print($"132 Row is {cellRow} and column is {cellCol}");
        if (selectedRow > -1)
        {
            SetCellColor(cells[cellRow, cellCol], true, 0);
            //    Resetting previous selected button
            //    Button button = cells[selectedRow, selectedCol].GetComponentInChildren<Button>();
            //    var colors = button.colors;
            //    colors.normalColor = Color.white;
            //    button.colors = colors;
        }
        print($"141 Selected Row is {selectedRow} and column is {selectedCol}");
        selectedRow = cellRow;
        selectedCol = cellCol;
        print($"144 Selected Row is {selectedRow} and column is {selectedCol}");
        SetCellColor(cells[cellRow, cellCol], true, 0);
        //Button button1 = cells[cellRow, cellCol].GetComponentInChildren<Button>();
        //var colors1 = button1.colors;
        //colors1.normalColor = Color.yellow;
        //button1.colors = colors1;

    }

    public void OnInput(int inputNumber)
    {
        if (selectedRow > -1)
        {
            SetCellColor(cells[selectedRow, selectedCol], true, 0);
            Button button = cells[selectedRow, selectedCol].GetComponentInChildren<Button>();
            // Assgining new value image
            button.GetComponentInChildren<Image>().sprite = numberSprits[inputNumber];
            // Assgining new value 
            button.GetComponent<CellProperties>().Value = inputNumber;
        }
    }
}
