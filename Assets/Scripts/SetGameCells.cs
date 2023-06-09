using Assets.Scripts;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetGameCells : MonoBehaviour
{

    [SerializeField] private GameObject CellPreFeb;
    [SerializeField] float BoarderDistane = 8;
    [SerializeField] private int BoardSize;


    private int[,] solvedBoard;
    private int[,] unsolvedBoard;
    public int DifficultyLevel { get; set; }
    private int numNonZeroVars = 22;
    [SerializeField] Sprite[] numberSprits;

    void Start()
    {
        CreateCells();
        SetGameBoardTiles(true);
    }

    private void CreateCells()
    {
        RectTransform panelReact = gameObject.GetComponent<RectTransform>();
        //Calculating cell size based on screen width
        float panelWidth = panelReact.rect.width;
        float cellWidth = (panelWidth - (4 * BoarderDistane)) / BoardSize;

        GameManager.Instance.cells = new GameObject[BoardSize, BoardSize];
        RectTransform gameCellPrefebTransform = CellPreFeb.GetComponent<RectTransform>();
        gameCellPrefebTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, cellWidth);
        gameCellPrefebTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, cellWidth);

        float statrupXPosition = (cellWidth / 2) + BoarderDistane;
        float statrupYPosition = statrupXPosition;
        for (int row = 0; row < BoardSize; row++)
        {
            for (int col = 0; col < BoardSize; col++)
            {
                GameObject cell = Instantiate(CellPreFeb);
                cell.transform.SetParent(gameObject.transform);
                GameManager.Instance.cells[row, col] = cell;
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
            LoadGameData();
        }
        int GridRow = 0;
        int GridCol;
        for (int row = 0; row < BoardSize; row++)
        {
            GridCol = GridRow;
            for (int col = 0; col < BoardSize; col++)
            {
                Image cellImage = GameManager.Instance.cells[row, col].GetComponent<Image>();
                var cellProperties = cellImage.GetComponent<CellProperties>();
                cellProperties.UnSolvedValue = unsolvedBoard[row, col];
                cellProperties.SolvedValue = solvedBoard[row, col];
                cellProperties.Row = row;
                cellProperties.Column = col;
                cellProperties.InnerGrid = GridCol;

                //Setting read only cells value
                if (unsolvedBoard[row, col] > 0)
                {
                    cellImage.sprite = numberSprits[unsolvedBoard[row, col]];
                    cellProperties.Status = CellState.ReadOnly;
                }
                // Setting user filled value
                else
                {
                    int CellValue = (GameManager.Instance.userInputValues[row, col] > 0) ? GameManager.Instance.userInputValues[row, col] : 0;
                    cellImage.sprite = numberSprits[CellValue];
                    cellProperties.Status = CellState.Normal;
                }
                if (col > 0 && (col + 1) % 3 == 0)
                    GridCol++;

            }
            if (row > 0 && (row + 1) % 3 == 0)
                GridRow = GridRow + 3;
        }
    }
    public void ShowGameStatus()
    {
        for (int row = 0; row < BoardSize; row++)
        {
            for (int col = 0; col < BoardSize; col++)
            {
                var cellProperties = GameManager.Instance.cells[row, col].GetComponent<CellProperties>();
                cellProperties.CheckCellValue();
            }
        }
    }
    public void SaveGameData()
    {
        // Create a dictionary to hold both arrays
        Dictionary<string, int[,]> dictionary = new Dictionary<string, int[,]>();
        dictionary.Add("solvedBoard", solvedBoard);
        dictionary.Add("unsolvedBoard", unsolvedBoard);
        dictionary.Add("userInputValues", GameManager.Instance.userInputValues);

        // Serialize the dictionary to JSON
        string json = JsonConvert.SerializeObject(dictionary);

        // Save the JSON string to PlayerPrefs
        PlayerPrefs.SetString("SudokuGameData", json);
    }

    private void LoadGameData()
    {
        try
        {
            // Retrieve the JSON string from PlayerPrefs
            string json = PlayerPrefs.GetString("SudokuGameData");

            // Deserialize the JSON string to a dictionary
            Dictionary<string, int[,]> dictionary = JsonConvert.DeserializeObject<Dictionary<string, int[,]>>(json);

            // Get the arrays from the dictionary
            solvedBoard = dictionary["solvedBoard"];
            unsolvedBoard = dictionary["unsolvedBoard"];
            GameManager.Instance.userInputValues = dictionary["userInputValues"];

        }
        catch
        {
            var boardGenerator = new SudokuBoardGenerator();
            boardGenerator.BoardSize = BoardSize;
            solvedBoard = boardGenerator.GenerateSolved();
            unsolvedBoard = boardGenerator.GenerateUnSolved(numNonZeroVars);
            GameManager.Instance.userInputValues = new int[BoardSize, BoardSize];
            for (int i = 0; i < BoardSize; i++)
            {
                for (int j = 0; j < BoardSize; j++)
                {
                    GameManager.Instance.userInputValues[i, j] = 0;
                }
            }
        }
    }
}
