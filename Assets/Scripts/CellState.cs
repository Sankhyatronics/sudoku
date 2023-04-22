using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public enum CellState
    {
        Normal = 0,
        ReadOnly = 1,
        PrimarySelected = 2,
        SecondarySelected = 3,
        ReadSelected = 4,
        Correct = 5,
        InCorrect = 6,

    }
    public enum HighlightedStatus
    {
        NormalLight = 1,
        HighLight = 2,
    }
}
