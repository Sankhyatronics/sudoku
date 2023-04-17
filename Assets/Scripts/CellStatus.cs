using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    [Flags]
    public enum CellStatus
    {
        Normal = 1,
        ReadOnly = 2,
        Selected = 4,
        Correct = 8,
        InCorrect = 16
    }
    public enum HighlightedStatus
    {
        Normal = 1,
        Highlighted = 2,
    }
}
