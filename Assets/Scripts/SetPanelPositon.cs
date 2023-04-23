using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SetPanelPositon : MonoBehaviour
{
    [SerializeField] private GameObject GamePanel;
    [SerializeField] private GameObject InputPanel;
    [SerializeField] private GameObject TitlePanel;

    void Start()
    {
        FixPanels();
        // SetCellPosition();
    }

    private void FixPanels()
    {
        // Calculating anchar position and height
        RectTransform RectTransform = gameObject.GetComponent<RectTransform>();
        float screenHeight = RectTransform.rect.height;
        float screenWidth = RectTransform.rect.width;
        float remaininHeight = screenHeight - screenWidth;
        //Game panel must be square
        var transformGamePanel = GamePanel.GetComponent<RectTransform>();
        transformGamePanel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, screenWidth);
        transformGamePanel.anchoredPosition = new Vector3(0, 0, 0);

        //Input panel should fill remaining space
        var InputPanelHeight = remaininHeight / 2;
        var transformInputPanel = InputPanel.GetComponent<RectTransform>();
        transformInputPanel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, InputPanelHeight);
        transformInputPanel.anchoredPosition = new Vector3(0, InputPanelHeight / 2, 0);

        ////Title panel should fill remaining space
        //var transformTitlePanel = TitlePanel.GetComponent<RectTransform>();
        //transformTitlePanel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, InputPanelHeight);
        //transformTitlePanel.anchoredPosition = new Vector3(0, -InputPanelHeight / 2, 0);
    }
}
