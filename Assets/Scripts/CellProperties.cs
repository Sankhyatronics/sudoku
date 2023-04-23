using Assets.Scripts;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class CellProperties : MonoBehaviour, IPointerClickHandler
{
    public int UnSolvedValue { get; set; }
    public int SolvedValue { get; set; }
    private Animator cellAnimator;
    private void Awake()
    {
        cellAnimator = GetComponent<Animator>();
    }
    public CellState Status
    {
        get { return _Status; }
        set
        {
            _Status = value;
            if (cellAnimator != null) cellAnimator.Play(_Status.ToString(), 0, 0f);
        }
    }
    private CellState _Status = CellState.Normal;

    public int Row { get; set; }
    public int Column { get; set; }

    public int InnerGrid { get; set; }
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (_Status == CellState.ReadOnly || _Status == CellState.ReadSelected) return;
        if (GameManager.Instance.SelectedCell != null)
        {
            var PreviousCell = GameManager.Instance.SelectedCell;
            var cellProperties = PreviousCell.GetComponent<CellProperties>();
            cellProperties.Status = CellState.Normal;
        }
        GameManager.Instance.SelectedCell = gameObject;
        this.Status = CellState.PrimarySelected;
        HighlightRelatedCells();
    }

    public void CheckCellValue()
    {
        if (_Status == CellState.ReadOnly || _Status == CellState.ReadSelected) return;
        if (this.SolvedValue == this.UnSolvedValue)
        {
            Status = CellState.Correct;
        }
        else
        {
            Status = CellState.InCorrect;
        }
    }
    public void HighlightRelatedCells()
    {
        int size = GameManager.Instance.cells.GetLength(0);
        for (int row = 0; row < size; row++)
        {
            for (int col = 0; col < size; col++)
            {
                var cellProperties = GameManager.Instance.cells[row, col].GetComponent<CellProperties>();
                //Skip status set for clicked cell
                if (cellProperties.Status == CellState.PrimarySelected) continue;
                // Checking quaified cells
                if (cellProperties.Row == this.Row || cellProperties.Column == this.Column || cellProperties.InnerGrid == this.InnerGrid)
                {
                    if (cellProperties.Status == CellState.ReadSelected || cellProperties.Status == CellState.ReadOnly)
                        cellProperties.Status = CellState.ReadSelected;
                    else
                        cellProperties.Status = CellState.SecondarySelected;
                }
                else
                {
                    if (cellProperties.Status == CellState.ReadSelected || cellProperties.Status == CellState.ReadOnly)
                        cellProperties.Status = CellState.ReadOnly;
                    else
                        cellProperties.Status = CellState.Normal;
                }
            }
        }
    }
    public void SetCellValue(int value, Sprite ValueImage)
    {
        if (this.Status == CellState.PrimarySelected)
        {
            UnSolvedValue = value;
            this.GetComponent<Image>().sprite = ValueImage;
        }
    }
}

