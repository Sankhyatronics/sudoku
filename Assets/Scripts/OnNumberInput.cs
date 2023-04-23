using UnityEngine;

public class OnNumberInput : MonoBehaviour
{
    [SerializeField] private Sprite ValueImage;
    public void SetCellValue(int value)
    {

        var cellProperties = GameManager.Instance.SelectedCell.GetComponent<CellProperties>();
        //var cellProperties = button.GetComponent<CellProperties>();
        if (cellProperties != null)
        {
            cellProperties.SetCellValue(value, ValueImage);
            GameManager.Instance.userInputValues[cellProperties.Row, cellProperties.Column] = value;
            //cellProperties.UnSolvedValue = value;
            //button.sprite = ValueImage;
        }
    }
}
