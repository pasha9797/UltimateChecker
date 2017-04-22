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
        public IPlayer WhitePlayer { get; }
        public IPlayer BlackPlayer { get; }

        private Dictionary<ICommand,FieldState> states { get; }

        public void ExecuteStep(IChecker mover, Coord coord)
        {
            if (mover.CheckPossibilityToMove(coord, GameField))
            {

                LogStep(mover.CurrentCoord, coord, mover);
                MoveCheckerDirectly(mover, coord);
                CheckGettingKing(mover);
                NextTurn();
            }
        }

        public void ExecuteStep(IChecker killer, IChecker victim, Coord coord)
        {
            if (killer.CheckPossibilityToMove(coord, GameField))
            {
                LogStepWithKill(killer.CurrentCoord, coord, killer, victim);
                MoveCheckerDirectly(killer, coord);
                GameField.Grid[victim.CurrentCoord.Row][victim.CurrentCoord.Column] = null;
                CheckGettingKing(killer);
                NextTurn();
            }
        }

        private void CheckGettingKing(IChecker checker)
        {
            if (checker is WhiteChecker)
            {
                if (checker.CurrentCoord.Row == 1)
                    checker.CheckerState = new WhiteKingCheckerState();
            }
            else
            {
                if (checker.CurrentCoord.Row == 8)
                    checker.CheckerState = new BlackKingCheckerState();
            }
        }

        private void MoveCheckerDirectly(IChecker checker, Coord coord)
        {
            GameField.Grid[checker.CurrentCoord.Row][checker.CurrentCoord.Column] = null;
            checker.CurrentCoord = coord;
            GameField.Grid[checker.CurrentCoord.Row][checker.CurrentCoord.Column] = checker;
        }

        public void UndoStep(ICommand command)
        {
            GameField.RestoreState(states[command]);
            states.Remove(command);
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

        async private void NextTurn()
        {
            switch (GameField.Turn)
            {
                case PlayersSide.WHITE:
                    states.Add(await WhitePlayer.MakeStep(GameField), GameField.SaveState());
                    GameField.Turn = PlayersSide.BLACK;
                    break;
                case PlayersSide.BLACK:
                    states.Add(await BlackPlayer.MakeStep(GameField),GameField.SaveState());
                    GameField.Turn = PlayersSide.WHITE;
                    break;
            }

        }

        public Game()
        {
            GameField = new GameField();
            states = new Dictionary<ICommand, FieldState>();

            //at first will be like that
            WhitePlayer = new BotPlayer(this, PlayersSide.WHITE);
            //BlackPlayer = new Player(this, PlayersSide.BLACK);

            NextTurn();
        }
    }
}
