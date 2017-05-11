using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using UltimateChecker.Classes.Players;

namespace UltimateChecker
{
    class Game : IGame
    {
        public IGameField GameField { get; }
        public IPlayer WhitePlayer { get; }
        public IPlayer BlackPlayer { get; }
        MainWindow mainWindow;

        private Dictionary<ICommand, FieldState> states { get; }

        public void ExecuteStep(IChecker mover, Coord coord)
        {
            if (mover.CheckPossibilityToMove(coord, GameField))
            {
                LogStep(mover.CurrentCoord, coord, mover);
                MoveCheckerDirectly(mover, coord);
                CheckGettingKing(mover);
                SwitchTurn();
                NextTurn();
            }
        }

        public void ExecuteStep(IChecker killer, IChecker victim, Coord coord)
        {
            if (killer.CheckPossibilityToKill(coord, GameField))
            {
                LogStepWithKill(killer.CurrentCoord, coord, killer, victim);
                MoveCheckerDirectly(killer, coord);
                Kill(victim);
                CheckGameOver();
                CheckGettingKing(killer);
                SwitchTurn(killer);
                NextTurn();
            }
        }

        private void Kill(IChecker victim)
        {
            GameField.Grid[victim.CurrentCoord.Row][victim.CurrentCoord.Column] = null;
            GameField.FormGrid.Children.Remove(victim.checkerUI);
        }

        private void SwitchTurn(IChecker checker)
        {
            if (!checker.CheckAllPossibilitiesToKill(GameField))
            {
                SwitchTurn();
            }
        }

        private void SwitchTurn()
        {
            if (GameField.Turn == Lib.PlayersSide.WHITE)
                GameField.Turn = Lib.PlayersSide.BLACK;
            else
                GameField.Turn = Lib.PlayersSide.WHITE;
        }

        private void CheckGettingKing(IChecker checker)
        {
            if (checker is WhiteChecker)
            {
                if (checker.CurrentCoord.Row == 1)
                    checker.BecomeKing();
            }
            else
            {
                if (checker.CurrentCoord.Row == 8)
                    checker.BecomeKing();
            }
        }

        private void CheckGameOver()
        {
            if (GameField.WhiteCheckers.Count == 0)
                MessageBox.Show("Чёрные победили!");
            else if (GameField.BlackCheckers.Count == 0)
                MessageBox.Show("Белые победили!");
        }

        private void MoveCheckerDirectly(IChecker checker, Coord coord)
        {
            GameField.Grid[checker.CurrentCoord.Row][checker.CurrentCoord.Column] = null;
            checker.CurrentCoord = coord;
            GameField.Grid[checker.CurrentCoord.Row][checker.CurrentCoord.Column] = checker;
            mainWindow.MoveCheckerToAnotherCell(checker.checkerUI, coord);
        }

        public void UndoStep(ICommand command)
        {
            if (GameField.Turn == Lib.PlayersSide.WHITE)
                WhitePlayer.CancelStep();
            else
                BlackPlayer.CancelStep();

            GameField.RestoreState(states[command]);
            states.Remove(command);

            NextTurn();
        }

        public void UndoByClick(int CommandID)
        {
            if (CommandID < states.Count)
            {
                for (int i = states.Count - 1; i > CommandID; i--)
                {
                    states.Remove(states.Keys.Last());
                }
                states.Keys.Last().Cansel();
            }
        }

        private void LogStep(Coord prevCoord, Coord newCoord, IChecker checker)
        {
            string message;
            message = (checker is WhiteChecker) ? "White" : "Black";
            message += String.Format(": {0}{1} -> {2}{3}", Lib.Signs[prevCoord.Column], prevCoord.Row, Lib.Signs[newCoord.Column], newCoord.Row);
            GameField.StepsHistoryAdd(message);
            mainWindow.AddLog(message);
        }

        private void LogStepWithKill(Coord prevCoord, Coord newCoord, IChecker checker, IChecker killed)
        {
            string message;

            message = (checker is WhiteChecker) ? "White" : "Black";
            message += String.Format(": {0}{1} -> {2}{3}\n", Lib.Signs[prevCoord.Column], prevCoord.Row, Lib.Signs[newCoord.Column], newCoord.Row);

            message += (killed is WhiteChecker) ? "White" : "Black";
            message += String.Format(": {0}{1} killed", Lib.Signs[killed.CurrentCoord.Column], killed.CurrentCoord.Row);

            GameField.StepsHistoryAdd(message);
            mainWindow.AddLog(message);
        }

        public string[] GetStepsHistory()
        {
            return GameField.StepsHistory;
        }

        async private void NextTurn()
        {
            ICommand command;
            FieldState state;
            switch (GameField.Turn)
            {
                case Lib.PlayersSide.WHITE:
                    BlockBlack();
                    command = await WhitePlayer.MakeStep(GameField);
                    if (command != null)
                    {
                        state = GameField.SaveState();
                        states.Add(command, state);
                        command.Execute();
                    }
                    break;
                case Lib.PlayersSide.BLACK:
                    BlockWhite();
                    command = await BlackPlayer.MakeStep(GameField);
                    if (command != null)
                    {
                        state = GameField.SaveState();
                        states.Add(command, state);
                        command.Execute();
                    }
                    break;
            }

        }

        public void Capitulate(Lib.PlayersSide side)
        {
            for (int i = 1; i <= 8; i++)
            {
                for (int j = 1; j <= 8; j++)
                {
                    if (side==Lib.PlayersSide.BLACK && GameField.Grid[i][j] is BlackChecker)
                    {
                        GameField.Grid[i][j] = null;
                    }
                    if (side == Lib.PlayersSide.WHITE && GameField.Grid[i][j] is WhiteChecker)
                    {
                        GameField.Grid[i][j] = null;
                    }
                }
            }
            CheckGameOver();
        }

        public Game(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            GameField = new GameField(mainWindow);
            states = new Dictionary<ICommand, FieldState>();

            BlackPlayer = new BotPlayer(this, Lib.PlayersSide.BLACK);
            BlackPlayer.Capitulate = Capitulate;

            WhitePlayer = new Player(this, Lib.PlayersSide.WHITE);
            WhitePlayer.Capitulate = Capitulate;

            InitializePlayersForCheckers();

            (this.GameField as GameField).SendCommandToPlayerEvent += (WhitePlayer as Player).RecieveCommandFromForm; 
            NextTurn();
        }

        private void InitializePlayersForCheckers()
        {
            foreach (IChecker checker in GameField.BlackCheckers)
            {
                CheckerUI checkerUI = checker.checkerUI as CheckerUI;
                checkerUI.Player = BlackPlayer;
            }
            foreach (IChecker checker in GameField.WhiteCheckers)
            {
                CheckerUI checkerUI = checker.checkerUI as CheckerUI;
                checkerUI.Player = WhitePlayer;
            }
        }

        private void BlockBlack()
        {
            foreach (WhiteChecker checker in GameField.WhiteCheckers)
            {
                CheckerUI checkUI = checker.checkerUI as CheckerUI;
                checkUI.druggingIsPermitted = true;
            }
            foreach (BlackChecker checker in GameField.BlackCheckers)
            {
                CheckerUI checkUI = checker.checkerUI as CheckerUI;
                checkUI.druggingIsPermitted = false;
            }
        }


        private void BlockWhite()
        {
            foreach (WhiteChecker checker in GameField.WhiteCheckers)
            {
                CheckerUI checkUI = checker.checkerUI as CheckerUI;
                checkUI.druggingIsPermitted = false;
            }
            foreach (BlackChecker checker in GameField.BlackCheckers)
            {
                CheckerUI checkUI = checker.checkerUI as CheckerUI;
                checkUI.druggingIsPermitted = true;
            }
        }
    }
}
