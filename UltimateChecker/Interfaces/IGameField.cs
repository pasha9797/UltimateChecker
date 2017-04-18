using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateChecker
{
    public interface IGameField
    {
        IChecker[][] Grid { get; }
        FieldState SaveState();
        void RestoreState(FieldState state);
        void StepsHistoryAdd(string log);
        string[] StepsHistory { get; }
    }
}
