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
        Image button = GameManager.Instance.SelectedCell.GetComponent<Image>();
        var cellProperties = button.GetComponent<CellProperties>();
        cellProperties.UnSolvedValue = value;
        button.sprite = ValueImage;
    }
}
