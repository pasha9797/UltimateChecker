using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateChecker
{
    class Game : IGame
    {
        public IGameField GameField { get; }
        private Stack<FieldState> states { get; }

        public void ExecuteStep(IChecker mover, Coord coord)
        {
            if (mover.CheckPossibility(coord, GameField))
            {
                SaveState();
                LogStep(mover.CurrentCoord, coord, mover);
                MoveCheckerDirectly(mover, coord);
                CheckGettingKing(mover);
            }
        }

        public void ExecuteStep(IChecker killer, IChecker victim, Coord coord)
        {
            if (killer.CheckPossibility(coord, GameField))
            {
                SaveState();
                LogStepWithKill(killer.CurrentCoord, coord, killer, victim);
                MoveCheckerDirectly(killer, coord);
                GameField.Grid[victim.CurrentCoord.Row][victim.CurrentCoord.Column] = null;
                CheckGettingKing(killer);
            }
        }

        private void CheckGettingKing(IChecker checker)
        {
            if (checker is WhiteChecker)
            {
                if (checker.CurrentCoord.Row == 1)
                    checker.CheckerState = new WhiteKingChecker();
            }
            else
            {
                if (checker.CurrentCoord.Row == 8)
                    checker.CheckerState = new BlackKingChecker();
            }
        }

        private void MoveCheckerDirectly(IChecker checker, Coord coord)
        {
            GameField.Grid[checker.CurrentCoord.Row][checker.CurrentCoord.Column] = null;
            checker.CurrentCoord = coord;
            GameField.Grid[checker.CurrentCoord.Row][checker.CurrentCoord.Column] = checker;
        }

        private void SaveState()
        {
            states.Push(GameField.SaveState());
        }

        public void UndoStep()
        {
            GameField.RestoreState(states.Pop());
        }

        private void LogStep(Coord prevCoord, Coord newCoord, IChecker checker)
        {
            string message;
            message = (checker is WhiteChecker)?"White":"Black";
            message += String.Format(": {0}{1} -> {2}{3}", Lib.Signs[prevCoord.Column], prevCoord.Row, Lib.Signs[newCoord.Column], newCoord.Row);
            GameField.StepsHistoryAdd(message);
        }

        private void LogStepWithKill(Coord prevCoord, Coord newCoord, IChecker checker, IChecker killed)
        {
            LogStep(prevCoord, newCoord, checker);
            string message;
            message = (killed is WhiteChecker) ? "White" : "Black";
            message += String.Format(": {0}{1} killed", Lib.Signs[killed.CurrentCoord.Column], killed.CurrentCoord.Row);
            GameField.StepsHistoryAdd(message);
        }

        public string[] GetStepsHistory()
        {
            return GameField.StepsHistory;
        }

        public Game()
        {
            GameField = new GameField();
            states = new Stack<FieldState>();
        }
    }
}
