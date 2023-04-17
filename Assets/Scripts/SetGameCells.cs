using Assets.Scripts;
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
                GameObject cell;
                cell = Instantiate(CellPreFeb);
                cell.transform.SetParent(gameObject.transform);
                Image button = cell.GetComponent<Image>();
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
            var boardGenerator = new SudokuBoardGenerator();
            boardGenerator.BoardSize = BoardSize;
            solvedBoard = boardGenerator.GenerateSolved();
            unsolvedBoard = boardGenerator.GenerateUnSolved(numNonZeroVars);
        }
        for (int row = 0; row < BoardSize; row++)
        {
            for (int col = 0; col < BoardSize; col++)
            {
                    
                Image button = GameManager.Instance.cells[row, col].GetComponent<Image>();
                var cellProperties = button.GetComponent<CellProperties>();
                cellProperties.UnSolvedValue = unsolvedBoard[row, col];
                cellProperties.SolvedValue = solvedBoard[row, col];
                cellProperties.Row = row;
                cellProperties.Column = col;
                if (unsolvedBoard[row, col] > 0)
                {
                    button.sprite = numberSprits[unsolvedBoard[row, col]];
                    cellProperties.Status = CellStatus.ReadOnly;
                }
                else
                {
                    button.sprite = numberSprits[0];
                    //cellProperties.Status = CellStatus.Normal;
                }
            }
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
}
