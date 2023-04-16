using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellProperties : MonoBehaviour
{
    public int UnSolvedValue { get; set; }
    public int SolvedValue { get; set; }

    public enum CellStatus { }

    public int Row { get; set; }
    public int Column { get; set; }
}
