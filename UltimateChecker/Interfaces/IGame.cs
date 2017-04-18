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
        void UndoStep(ICommand command);
        string[] GetStepsHistory();

        IPlayer WhitePlayer { get; }
        IPlayer BlackPlayer { get; }
        IGameField GameField { get; }
    }
}
