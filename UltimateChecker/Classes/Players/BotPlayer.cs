using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltimateChecker.Algorithms;

namespace UltimateChecker
{
    class KillingCommand : ICommand
    {
        IGame resiver;
        IChecker killer;
        IChecker victim;

        public KillingCommand(IGame game, IChecker killer, IChecker victim)
        {
            resiver = game;
            this.killer = killer;
            this.victim = victim;
        }

        public void Execute()
        {
            resiver.ExecuteStep(killer, victim);
        }

        public void Cansel(ICommand command)
        {
            resiver.UndoStep(command);
        }

        public string Name()
        {
            return "Killing Command";
        }
    }

    public enum PlayersSide
    {
        WHITE,
        BLACK
    }

    class BotPlayer : IPlayer
    {
        IGame game;
        IGameField gameField;
        ICommand turnCommand;
        PlayersSide side;
        PriorityQueue<ICommand, int> commands;



        public BotPlayer(IGame game, PlayersSide side)
        {
            this.side = side;
        }

        public async Task<ICommand> MakeStep(IGameField gameField)
        {
            commands = new PriorityQueue<ICommand, int>();
            this.gameField = gameField;
            return await MakeTurnTask();
        }

        Task<ICommand> MakeTurnTask()
        {
            return Task<ICommand>.Run<ICommand>(new Func<ICommand>(MakeTurn));
        }

        private ICommand MakeTurn()
        {
            CheckAllChreckersForPossibilityToKil(side); //проверка на возможность убийства

            return commands.Dequeue();//возврат самой приоритетной операции
        }

        private void CheckAllChreckersForPossibilityToKil(PlayersSide side)
        {
            switch (side)
            {
                case PlayersSide.WHITE:
                    foreach (var checker in gameField.WhiteCheckers)
                    {
                        IChecker victim = CheckPossibilityToKill(checker);
                        if (victim != null) commands.Enqueue(new KillingCommand(game, checker, victim), 10);
                    }
                    break;
                case PlayersSide.BLACK:
                    foreach (var checker in gameField.BlackCheckers)
                    {
                        IChecker victim = CheckPossibilityToKill(checker);
                        if (victim != null) commands.Enqueue(new KillingCommand(game, checker, victim), 10);
                    }
                    break;
                default:
                    break;
            }
        }

        private IChecker CheckPossibilityToKill(IChecker checker)
        {
            IChecker[][] grid = gameField.Grid;

            int row = checker.CurrentCoord.Row;
            int column = checker.CurrentCoord.Column;

            try
            {
                for (int i = row - 1; i <= row + 1; i += 2)
                {
                    for (int j = column - 1; j <= j + 1; j += 2)
                    {
                        if ((i != row || j != column) && // исключая саму себя
                            CheckGameFieldBorders(i, j) && // исключая выход за границы масиива
                            grid[i][j] != null && // исключая пустые
                            grid[i][j].side != checker.side) // исключая свои шашки
                        {
                            IChecker enemy = grid[i][j];
                            return checker.CheckPossibilityToKill(enemy) ? enemy : null; // шашка проверяет возможность убийства врага
                        }
                    }
                }
            }
            catch (Exception e) { }
            return null; //нет возможности убийства
        }

        private bool CheckGameFieldBorders(int row, int colunmn)
        {
            double size = Math.Sqrt(gameField.Grid.Length);
            return (row <= size && colunmn <= size);
        }
    }
}
