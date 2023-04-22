using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Localization.LocalizationTableCollection;

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
            //cellProperties.UnSolvedValue = value;
            //button.sprite = ValueImage;
        }
    }
}
