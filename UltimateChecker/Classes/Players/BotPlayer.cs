using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltimateChecker.Algorithms;

namespace UltimateChecker
{

    class BotPlayer : IPlayer
    {
        IGame game;
        IGameField gameField;
        ICommand turnCommand;
        Lib.PlayersSide side;
        PriorityQueue<ICommand, double> commands;
        private bool stepCanceled=false;



        public BotPlayer(IGame game, Lib.PlayersSide side)
        {
            this.side = side;
        }

        public async Task<ICommand> MakeStep(IGameField gameField)
        {
            commands = new PriorityQueue<ICommand, double>();
            this.gameField = gameField;
            return await MakeTurnTask();
        }

        Task<ICommand> MakeTurnTask()
        {
            return Task<ICommand>.Run<ICommand>(new Func<ICommand>(MakeTurn));
        }

        private ICommand MakeTurn()
        {
            stepCanceled = false; //в начале шага эта переменная false

            CheckAllChreckersForPossibilityToKill(side); //проверка на возможность убийства

            if (stepCanceled) return null; //если к концу шага она стала true, значит был откат шагов и этот шаг отменяется
            return commands.Dequeue();//возврат самой приоритетной операции
        }

        public void FinishStep(Coord coord, IChecker mover, IChecker victim)//просто для реализации интерфейса
        {
        }

        public void CancelStep()
        {
            stepCanceled = true;
        }

        private void CheckAllChreckersForPossibilityToKill(Lib.PlayersSide side)
        {
            List<IChecker> allies = GetAllies(side);

            foreach (var checker in allies)
            {
                Coord destination;
                IChecker victim = CheckPossibilityToKill(checker, out destination);
                if (victim != null) commands.Enqueue(new KillingCommand(game, checker, victim, destination), 10);
            }
        }

        private void CheckAllCheckersForPossibilityToMove(Lib.PlayersSide side)
        {
            List<IChecker> allies = GetAllies(side);

            foreach (var checker in allies)
            {
                CheckPossibilityToMove(side, checker);
            }
        }

        private IChecker CheckPossibilityToKill(IChecker checker, out Coord destination)
        {
            IChecker[][] grid = gameField.Grid;
            List<IChecker> enemies = GetEnemies(side);

            int row = checker.CurrentCoord.Row;
            int column = checker.CurrentCoord.Column;
            destination = default(Coord);

            try
            {
                foreach (IChecker enemy in enemies)
                {
                    destination = Destination(checker.CurrentCoord, enemy.CurrentCoord);
                    if (checker.CheckPossibilityToKill(destination, gameField))
                    {
                        return enemy;
                    }
                }
            }
            catch (Exception e) { }
            return null; //нет возможности убийства
        }

        private void CheckPossibilityToMove(Lib.PlayersSide side, IChecker checker)
        {
            // WARNING : ГОВНОКОД, тк не могу понять сразу является-ли шашка дамкой или нет. Приходится выяснять. Портит ООП пиздец.
            switch (side)
            {
                case Lib.PlayersSide.WHITE:
                    if(checker.CheckerState is WhiteNormalCheckerState)
                    {
                        double stepPriority = 1;
                        stepPriority = MoveForwardLeft(checker, 1);
                        if (stepPriority != 0)//если == 0 - ход невозможен
                            commands.Enqueue(new MovingCommand(game, checker, checker.MoveForwardLeft(1)), stepPriority); // команда сдвига вперед-влево

                        stepPriority = 1;
                        stepPriority = MoveForwardRight(checker, 1);
                        if (stepPriority != 0)//если == 0 - ход невозможен
                            commands.Enqueue(new MovingCommand(game, checker, checker.MoveForwardRight(1)), stepPriority); // команда сдвига вперед-влево
                    }
                    else if(checker.CheckerState is WhiteKingCheckerState)
                    {

                    }
                    break;
                case Lib.PlayersSide.BLACK:
                    if (checker.CheckerState is BlackNormalCheckerState)
                    {
                        double stepPriority = 1;
                        stepPriority = MoveForwardLeft(checker, 1);
                        if (stepPriority != 0)//если == 0 - ход невозможен
                            commands.Enqueue(new MovingCommand(game, checker, checker.MoveForwardLeft(1)), stepPriority); // команда сдвига вперед-влево

                        stepPriority = 1;
                        stepPriority = MoveForwardRight(checker, 1);
                        if (stepPriority != 0)//если == 0 - ход невозможен
                            commands.Enqueue(new MovingCommand(game, checker, checker.MoveForwardRight(1)), stepPriority); // команда сдвига вперед-влево
                    }
                    else if (checker.CheckerState is WhiteKingCheckerState)
                    {

                    }
                    break;
                default:
                    break;
            }
        }

        private double MoveForwardLeft(IChecker mover, int numberOfSteps)
        {
            double stepPriority = 0;
            Coord destination = mover.MoveForwardLeft(numberOfSteps);
            if (!mover.CheckPossibilityToMove(destination, gameField)) return stepPriority;
            else stepPriority += CheckPossibilityToBeKilled(destination);
            return stepPriority;
        }

        private double MoveForwardRight(IChecker mover, int numberOfSteps)
        {
            double stepPriority = 0;
            Coord destination = mover.MoveForwardRight(numberOfSteps);
            if (!mover.CheckPossibilityToMove(destination, gameField)) return stepPriority;
            else stepPriority += CheckPossibilityToBeKilled(destination);
            return stepPriority;
        }

        private double MoveBackLeft(IChecker mover, int numberOfSteps)
        {
            double stepPriority = 0;
            Coord destination = mover.MoveBackLeft(numberOfSteps);
            if (!mover.CheckPossibilityToMove(destination, gameField)) return stepPriority;
            else stepPriority += CheckPossibilityToBeKilled(destination);
            return stepPriority;
        }

        private double MoveBackRight(IChecker mover, int numberOfSteps)
        {
            double stepPriority = 0;
            Coord destination = mover.MoveBackRight(numberOfSteps);
            if (!mover.CheckPossibilityToMove(destination, gameField)) return stepPriority;
            else stepPriority += CheckPossibilityToBeKilled(destination);
            return stepPriority;
        }

        private double CheckPossibilityToBeKilled(Coord destination)
        {
            // допилить
            return 1;
        }

        private List<IChecker> GetAllies(Lib.PlayersSide side)
        {
            return (side == Lib.PlayersSide.WHITE) ? gameField.WhiteCheckers : gameField.BlackCheckers;
        }

        private List<IChecker> GetEnemies(Lib.PlayersSide side)
        {
            return (side == Lib.PlayersSide.WHITE) ? gameField.BlackCheckers : gameField.WhiteCheckers;
        }

        private Coord Destination(Coord killerCoord, Coord victimCoord)
        {
            int moveVertical;
            int moveHorizontal;
            Coord destination;

            moveVertical = (killerCoord.Row > victimCoord.Row) ? -1 : 1;
            moveHorizontal = (killerCoord.Column > victimCoord.Column) ? -1 : 1;

            destination = new Coord(victimCoord.Row + moveVertical, victimCoord.Column + moveHorizontal);

            return destination;
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
