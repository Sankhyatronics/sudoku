using UnityEngine;

public class OnNumberInput : MonoBehaviour
{
    [SerializeField] private Sprite ValueImage;
    public void SetCellValue(int value)
    {
        if (GameManager.Instance.SelectedCell != null)
        {
            var cellProperties = GameManager.Instance.SelectedCell.GetComponent<CellProperties>();
            cellProperties.SetCellValue(value, ValueImage);
            GameManager.Instance.userInputValues[cellProperties.Row, cellProperties.Column] = value;
            GameManager.Instance.SaveGameData();
        }
    }
}
