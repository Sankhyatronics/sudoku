using Newtonsoft.Json;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject SelectedCell { get; set; }
    public GameObject[,] cells;
    public int[,] userInputValues;
    public int[,] solvedBoard;
    public int[,] unsolvedBoard;
    public int DifficultyLevel { get; set; }
    public int numNonZeroVars = 22;
    public static GameManager Instance { get; private set; }

    public void LoadSettingScene()
    {
        SceneManager.LoadScene(1);
    }
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
    public void SaveGameData()
    {
        // Create a dictionary to hold both arrays
        Dictionary<string, int[,]> dictionary = new Dictionary<string, int[,]>();
        dictionary.Add("solvedBoard", GameManager.Instance.solvedBoard);
        dictionary.Add("unsolvedBoard", GameManager.Instance.unsolvedBoard);
        dictionary.Add("userInputValues", GameManager.Instance.userInputValues);

        // Serialize the dictionary to JSON
        string json = JsonConvert.SerializeObject(dictionary);

        // Save the JSON string to PlayerPrefs
        PlayerPrefs.SetString("SudokuGameData", json);
    }
}
