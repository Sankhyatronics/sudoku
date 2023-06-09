using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject SelectedCell { get; set; }
    public GameObject[,] cells;
    public int[,] userInputValues;
    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
}
