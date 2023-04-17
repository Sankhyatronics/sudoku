using Assets.Scripts;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class CellProperties : MonoBehaviour, IPointerClickHandler
{
    public int UnSolvedValue { get; set; }
    public int SolvedValue { get; set; }

    public HighlightedStatus HilightStatus
    {
        get { return _HilightStatus; }
        set
        {
            _HilightStatus = value;
            Image cell = gameObject.GetComponent<Image>();
            switch (_HilightStatus)
            {
                case HighlightedStatus.Normal:
                    {
                        cell.GetComponent<RectTransform>().localScale = Vector3.one;
                        break;
                    }
                case HighlightedStatus.Highlighted:
                    {
                        cell.GetComponent<RectTransform>().localScale = new Vector3(1.1f, 1.1f, 1.1f);
                        break;
                    }
                default: break;
            }
        }
    }
    private HighlightedStatus _HilightStatus = HighlightedStatus.Normal;
    public CellStatus Status
    {
        get { return _Status; }
        set
        {
            _Status = value;
            Image cell = gameObject.GetComponent<Image>();
            switch (_Status)
            {
                case CellStatus.Normal:

                    {
                        cell.color = new Color32(255, 255, 255, 255);

                        break;
                    }
                case CellStatus.ReadOnly:
                    {
                        cell.color = new Color32(219, 219, 219, 128);
                        break;
                    }
                case CellStatus.Correct:

                    {
                        cell.color = new Color32(0, 250, 158, 255);
                        break;
                    }
                case CellStatus.InCorrect:

                    {
                        cell.color = new Color32(255, 28, 32, 255);
                        break;
                    }
                case CellStatus.Selected:
                    {
                        cell.color = new Color32(250, 221, 0, 255);
                        break;
                    }
                default: break;
            }
        }
    }
    private CellStatus _Status = CellStatus.Normal;

    public int Row { get; set; }
    public int Column { get; set; }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (Status == CellStatus.ReadOnly) return;
        if (GameManager.Instance.SelectedCell != null)
        {
            var PreviousCell = GameManager.Instance.SelectedCell;
            var cellProperties = PreviousCell.GetComponent<CellProperties>();
            cellProperties.HilightStatus = HighlightedStatus.Normal;
            cellProperties.Status = CellStatus.Normal;
        }
        GameManager.Instance.SelectedCell = gameObject;
        this.Status = CellStatus.Selected;

        HighlightRelatedCells();
    }

    public void CheckCellValue()
    {
        if (_Status == CellStatus.ReadOnly) return;
        if (this.SolvedValue == this.UnSolvedValue)
        {
            Status = CellStatus.Correct;
        }
        else
        {
            Status = CellStatus.InCorrect;
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
                var selectionImage = GameManager.Instance.cells[row, col].GetComponentsInChildren<Image>(true);
                if (cellProperties.Row == this.Row || cellProperties.Column == this.Column)
                {
                    cellProperties.HilightStatus = HighlightedStatus.Highlighted;
                    selectionImage[1].enabled = true;
                }
                else
                {
                    cellProperties.HilightStatus = HighlightedStatus.Normal;
                    selectionImage[1].enabled = false;
                }
            }
        }
    }
}

