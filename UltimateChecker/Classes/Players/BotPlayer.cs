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
        Coord destination;

        public KillingCommand(IGame game, IChecker killer, IChecker victim, Coord destination)
        {
            resiver = game;
            this.killer = killer;
            this.victim = victim;
            this.destination = destination;
        }

        public void Execute()
        {
            resiver.ExecuteStep(killer, victim, destination);
        }

        public void Cansel()
        {
            //отмена команды
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
                        Coord destination;
                        IChecker victim = CheckPossibilityToKill(checker, out destination);
                        if (victim != null) commands.Enqueue(new KillingCommand(game, checker, victim, destination), 10);
                    }
                    break;
                case PlayersSide.BLACK:
                    foreach (var checker in gameField.BlackCheckers)
                    {
                        Coord destination;
                        IChecker victim = CheckPossibilityToKill(checker, out destination);
                        if (victim != null) commands.Enqueue(new KillingCommand(game, checker, victim, destination), 10);
                    }
                    break;
                default:
                    break;
            }
        }

        private IChecker CheckPossibilityToKill(IChecker checker, out Coord destination)
        {
            IChecker[][] grid = gameField.Grid;

            int row = checker.CurrentCoord.Row;
            int column = checker.CurrentCoord.Column;
            destination = default(Coord);

            try
            {
                for (int i = row - 1; i <= row + 1; i += 2)
                {
                    for (int j = column - 1; j <= j + 1; j += 2)
                    {
                        if (CheckGameFieldBorders(i, j) && // исключая выход за границы масиива
                            grid[i][j] != null && // исключая пустые
                            CheckSides(checker, grid[i][j])) // исключая свои шашки
                        {
                            IChecker enemy = grid[i][j];
                            bool isAbleToMove = false;
                            destination = Destination(checker.CurrentCoord, grid[i][j].CurrentCoord,out isAbleToMove);
                            if(isAbleToMove)
                            return checker.CheckPossibility(checker.CurrentCoord, enemy.CurrentCoord, gameField) ? enemy : null; // шашка проверяет возможность убийства врага
                        }
                    }
                }
            }
            catch (Exception e) { }
            return null; //нет возможности убийства
        }

        private Coord Destination(Coord killerCoord, Coord victimCoord, out bool isAbleToMove)
        {
            int moveVertical;
            int moveHorizontal;
            Coord destination;
            isAbleToMove = false;

            moveVertical = (killerCoord.Row > victimCoord.Row)? -1: 1;
            moveHorizontal = (killerCoord.Column > victimCoord.Column) ? -1 : 1;

            destination = new Coord(victimCoord.Row + moveVertical, victimCoord.Column + moveHorizontal);

            return (CheckGameFieldBorders(destination.Row, destination.Column)) ? destination : default(Coord);
        }

        private bool CheckSides(IChecker killer, IChecker victim)
        {
            return (killer is BlackChecker && victim is WhiteChecker) ||
                   (killer is WhiteChecker && victim is BlackChecker);
        }

        private bool CheckGameFieldBorders(int row, int colunmn)
        {
            double size = Math.Sqrt(gameField.Grid.Length);
            return (row <= size && colunmn <= size && row >= 1 && colunmn >= 1);
        }
    }
}
