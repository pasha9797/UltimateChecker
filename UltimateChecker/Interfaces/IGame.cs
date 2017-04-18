using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateChecker
{
    public interface IGame
    {
        void ExecuteStep(IChecker mover, Coord coord);
        void ExecuteStep(IChecker killer, IChecker victim, Coord coord);
        void UndoStep();
        string[] GetStepsHistory();
        IGameField GameField { get; }
    }
}
