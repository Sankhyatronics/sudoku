using Assets.Scripts;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class CellProperties : MonoBehaviour, IPointerClickHandler
{
    public int UnSolvedValue { get; set; }
    public int SolvedValue { get; set; }

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
            cellProperties.Status = CellStatus.Normal;
        }
        GameManager.Instance.SelectedCell = gameObject;
        this.Status = CellStatus.Selected;
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
}

