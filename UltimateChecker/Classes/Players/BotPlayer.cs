using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UltimateChecker.Algorithms;

namespace UltimateChecker
{

    class BotPlayer : IPlayer
    {
        IGame game;
        IGameField gameField;
        ICommand turnCommand;
        IChecker[][] boardState;
        Lib.PlayersSide side;
        PriorityQueue<ICommand, double> commands;
        private bool stepCanceled = false;
        List<TurnNode> TurnNodes = new List<TurnNode>();
        int NumberOfRecurciveLevels = 2;
        public Lib.CapitulateDel Capitulate { get; set; }


        public BotPlayer(IGame game, Lib.PlayersSide side)
        {
            this.side = side;
            this.game = game;
        }

        public void FinishStep(Coord coord, IChecker mover, IChecker victim)//просто для реализации интерфейса
        {
        }

        public void CancelStep()
        {
            stepCanceled = true;
        }

        public async Task<ICommand> MakeStep(IGameField gameField)
        {
            TurnNodes = new List<TurnNode>();
            this.gameField = gameField;
            boardState = gameField.Grid;
            return await Task<ICommand>.Run<ICommand>(new Func<ICommand>(MakeTurn));
        }

        private ICommand MakeTurn()
        {
            stepCanceled = false; //в начале шага эта переменная false
            Thread.Sleep(1000);
           
            MakeTurnNodes(null, Lib.PlayersSide.BLACK, 0);

            if (stepCanceled) return null; //если к концу шага она стала true, значит был откат шагов и этот шаг отменяется
            return FindCommand(TurnNodes);
        }

        private ICommand FindCommand(List<TurnNode> turnNodes)
        {
            if (turnNodes.Count > 0)
            {
                Mixer(ref turnNodes);

                turnNodes.ForEach((node) => node.CountValue());

                ICommand command = turnNodes.Max().turnCommand;
                return command;
            }
            else
            {
                Capitulate(side);
                return null;
            }
        }

        private void Mixer (ref List<TurnNode> nodes)
        {
            int a = (new Random()).Next(0, nodes.Count);
            for (int i = 0; i < a; i++)
            {
                int b = (new Random()).Next(0, nodes.Count);
                int c = (new Random()).Next(0, nodes.Count);
                TurnNode temp = nodes[b];
                nodes[b] = nodes[c];
                nodes[c] = temp;
            }
            if(nodes.Count>0)
                nodes[0] = nodes[a];
        }
            
        private void MakeTurnNodes(TurnNode currentNode, Lib.PlayersSide whoseTurn, int recurciveLevel)
        {
            if (recurciveLevel <= NumberOfRecurciveLevels)
            {
                List<TurnNode> TurnNodes = new List<TurnNode>();
                if (recurciveLevel == 0)
                {
                    currentNode = new TurnNode(boardState, -1, 0); // корень дерева
                }

                List<IChecker> allies = GetAllies(whoseTurn, currentNode.state);
                List<IChecker> enemies = GetEnemies(whoseTurn, currentNode.state);

                foreach (var checker in allies)
                {
                    if (checker.IsKing)
                    {
                        CheckKingKilling(currentNode, whoseTurn, checker, recurciveLevel);
                        CheckKingMovement(currentNode, whoseTurn, checker, recurciveLevel);
                    }
                    else
                    {
                        CheckKilling(currentNode, whoseTurn, checker, recurciveLevel);
                        CheckMovement(currentNode, whoseTurn, checker, recurciveLevel);
                    }                 
                }

            }
        }

        private IChecker[][] CreateStateCopy(IChecker[][] state)
        {
            IChecker[][] newState = new IChecker[9][];
            CheckerFactory factory = CheckerFactory.GetInstance();
            for (int i = 1; i <= 8; i++)
            {
                newState[i] = new IChecker[9];
            }

            for (int i = 1; i <= 8; i++)
            {
                for (int j = 1; j <= 8; j++)
                {
                    if (state[i][j] != null)
                    {
                        if (state[i][j] is BlackChecker)
                        {
                            IChecker black = factory.CreateBlackPhantom(new Coord(i, j), gameField);
                            newState[i][j] = black;
                        }
                        else if (state[i][j] is WhiteChecker)
                        {
                            newState[i][j] = factory.CreateWhitePhantom(new Coord(i, j), gameField);
                        }
                        if (newState[i][j].IsKing) state[i][j].BecomeKing();
                    }
                }
            }
            return newState;
        }

        private TurnNode KillingRecurcion(TurnNode currentNode, Lib.PlayersSide whoseTurn, IChecker checker, Coord destination, int level)
        {
            int killingValue = (whoseTurn == Lib.PlayersSide.BLACK) ? 100 : 0;
            IChecker[][] state = CreateStateCopy(currentNode.state);
            TurnNode newNode = new TurnNode(state, killingValue, level);//чем больше уровень рекурсии, тем меньше         
            if (checker.CurrentCoord.Row == 4 && checker.CurrentCoord.Column == 3 && level == 0)
            {

            }

            bool a = (side == Lib.PlayersSide.BLACK && checker is BlackChecker) || (side == Lib.PlayersSide.WHITE && checker is WhiteChecker);

            if (level == 0 && a)
            {
                KillingCommand command = new KillingCommand(game, checker, checker.GetVictim(destination, gameField), destination);
                newNode.turnCommand = command; //присваеваем команду на первом уровне рекурсии
                TurnNodes.Add(newNode);
            }
            state = ExecuteOperationToKill(state, state[checker.CurrentCoord.Row][checker.CurrentCoord.Column], checker.GetVictim(destination, gameField), destination);
            MakeTurnNodes(newNode, AnotherPlayer(whoseTurn), level + 1);
            return newNode;
        }

        private TurnNode MovingRecurcion(TurnNode currentNode, Lib.PlayersSide whoseTurn, IChecker checker, Coord destination, int level)
        {
            IChecker[][] state = CreateStateCopy(currentNode.state);
            state = ExecuteOperationToMove(state, state[checker.CurrentCoord.Row][checker.CurrentCoord.Column], destination);
            TurnNode newNode = new TurnNode(state, EstimatingFunction(state), level);

            bool a = (side == Lib.PlayersSide.BLACK && checker is BlackChecker) || (side == Lib.PlayersSide.WHITE && checker is WhiteChecker);

            if (level == 0 && a)
            {
                MovingCommand command = new MovingCommand(game, checker, destination);
                newNode.turnCommand = command; //присваеваем команду на первом уровне рекурсии
                TurnNodes.Add(newNode);
            }
            MakeTurnNodes(newNode, AnotherPlayer(whoseTurn), level + 1);
            return newNode;
        }

        private void CheckKingMovement(TurnNode currentNode, Lib.PlayersSide whoseTurn, IChecker checker, int level)
        {
            int i = checker.CurrentCoord.Row;
            int j = checker.CurrentCoord.Column;
            while ((i <= 8) && (j <= 8))
            {
                if (checker.CheckPossibilityToKill(new Coord(i, j), gameField))
                {
                    currentNode.AddChild(MovingRecurcion(currentNode, whoseTurn, checker, new Coord(i, j), level));
                }
                i++;
                j++;
            }

            i = checker.CurrentCoord.Row;
            j = checker.CurrentCoord.Column;
            while ((i <= 8) && (j >= 1))
            {
                if (checker.CheckPossibilityToKill(new Coord(i, j), gameField))
                {
                    currentNode.AddChild(MovingRecurcion(currentNode, whoseTurn, checker, new Coord(i, j), level));
                }
                i++;
                j--;
            }

            i = checker.CurrentCoord.Row;
            j = checker.CurrentCoord.Column;
            while ((i >= 1) && (j <= 8))
            {
                if (checker.CheckPossibilityToKill(new Coord(i, j), gameField))
                {
                    currentNode.AddChild(MovingRecurcion(currentNode, whoseTurn, checker, new Coord(i, j), level));
                }
                i--;
                j++;
            }

            i = checker.CurrentCoord.Row;
            j = checker.CurrentCoord.Column;
            while ((i >= 1) && (j >= 1))
            {
                if (checker.CheckPossibilityToKill(new Coord(i, j), gameField))
                {
                    currentNode.AddChild(MovingRecurcion(currentNode, whoseTurn, checker, new Coord(i, j), level));
                }
                i--;
                j--;
            }
        }

        private void CheckKingKilling(TurnNode currentNode, Lib.PlayersSide whoseTurn, IChecker checker, int level)
        {
            int i = checker.CurrentCoord.Row;
            int j = checker.CurrentCoord.Column;
            while ((i <= 8) && (j <= 8))
            {
                if (checker.CheckPossibilityToKill(new Coord(i, j), gameField))
                {
                    currentNode.AddChild(KillingRecurcion(currentNode, whoseTurn, checker, new Coord(i, j), level));
                }
                i++;
                j++;
            }

            i = checker.CurrentCoord.Row;
            j = checker.CurrentCoord.Column;
            while ((i <= 8) && (j >= 1))
            {
                if (checker.CheckPossibilityToKill(new Coord(i, j), gameField))
                {
                    currentNode.AddChild(KillingRecurcion(currentNode, whoseTurn, checker, new Coord(i, j), level));
                }
                i++;
                j--;
            }

            i = checker.CurrentCoord.Row;
            j = checker.CurrentCoord.Column;
            while ((i >= 1) && (j <= 8))
            {
                if (checker.CheckPossibilityToKill(new Coord(i, j), gameField))
                {
                    currentNode.AddChild(KillingRecurcion(currentNode, whoseTurn, checker, new Coord(i, j), level));
                }
                i--;
                j++;
            }

            i = checker.CurrentCoord.Row;
            j = checker.CurrentCoord.Column;
            while ((i >= 1) && (j >= 1))
            {
                if (checker.CheckPossibilityToKill(new Coord(i, j), gameField))
                {
                    currentNode.AddChild(KillingRecurcion(currentNode, whoseTurn, checker, new Coord(i, j), level));
                }
                i--;
                j--;
            }
        }

        private void CheckMovement(TurnNode currentNode, Lib.PlayersSide whoseTurn, IChecker checker, int level)
        {
            List<TurnNode> turnNodes = new List<TurnNode>();

            if (checker.CheckPossibilityToMove(new Coord(checker.CurrentCoord.Row + 1, checker.CurrentCoord.Column + 1), gameField))
            {
                turnNodes.Add(MovingRecurcion(currentNode, whoseTurn, checker, new Coord(checker.CurrentCoord.Row + 1, checker.CurrentCoord.Column + 1), level));
            }
            if (checker.CheckPossibilityToMove(new Coord(checker.CurrentCoord.Row - 1, checker.CurrentCoord.Column + 1), gameField))
            {
                turnNodes.Add(MovingRecurcion(currentNode, whoseTurn, checker, new Coord(checker.CurrentCoord.Row - 1, checker.CurrentCoord.Column + 1), level));
            }
            if (checker.CheckPossibilityToMove(new Coord(checker.CurrentCoord.Row + 1, checker.CurrentCoord.Column - 1), gameField))
            {
                turnNodes.Add(MovingRecurcion(currentNode, whoseTurn, checker, new Coord(checker.CurrentCoord.Row + 1, checker.CurrentCoord.Column - 1), level));
            }
            if (checker.CheckPossibilityToMove(new Coord(checker.CurrentCoord.Row - 1, checker.CurrentCoord.Column - 1), gameField))
            {
                turnNodes.Add(MovingRecurcion(currentNode, whoseTurn, checker, new Coord(checker.CurrentCoord.Row - 1, checker.CurrentCoord.Column - 1), level));
            }

            currentNode.AddChild(turnNodes);
        }

        private void CheckKilling(TurnNode currentNode, Lib.PlayersSide whoseTurn, IChecker checker, int level)
        {
            List<TurnNode> turnNodes = new List<TurnNode>();

            if (checker.CheckPossibilityToKill(new Coord(checker.CurrentCoord.Row + 2, checker.CurrentCoord.Column + 2), gameField))
            {
                turnNodes.Add(KillingRecurcion(currentNode, whoseTurn, checker, new Coord(checker.CurrentCoord.Row + 2, checker.CurrentCoord.Column + 2), level));
            }
            if (checker.CheckPossibilityToKill(new Coord(checker.CurrentCoord.Row - 2, checker.CurrentCoord.Column + 2), gameField))
            {
                turnNodes.Add(KillingRecurcion(currentNode, whoseTurn, checker, new Coord(checker.CurrentCoord.Row - 2, checker.CurrentCoord.Column + 2), level));
            }
            if (checker.CheckPossibilityToKill(new Coord(checker.CurrentCoord.Row + 2, checker.CurrentCoord.Column - 2), gameField))
            {
                turnNodes.Add(KillingRecurcion(currentNode, whoseTurn, checker, new Coord(checker.CurrentCoord.Row + 2, checker.CurrentCoord.Column - 2), level));
            }
            if (checker.CheckPossibilityToKill(new Coord(checker.CurrentCoord.Row - 2, checker.CurrentCoord.Column - 2), gameField))
            {
                turnNodes.Add(KillingRecurcion(currentNode, whoseTurn, checker, new Coord(checker.CurrentCoord.Row - 2, checker.CurrentCoord.Column - 2), level));
            }

            currentNode.AddChild(turnNodes);
        }

        private Lib.PlayersSide AnotherPlayer(Lib.PlayersSide playerside)
        {
            return (playerside == Lib.PlayersSide.BLACK) ? Lib.PlayersSide.WHITE : Lib.PlayersSide.BLACK;
        }

        private double EstimatingFunction(IChecker[][] boardState)
        {
            double result = 50;

            List<IChecker> allies = GetAllies(side, boardState);
            List<IChecker> enemies = GetEnemies(side, boardState);

            return result;
        } // функция валидации хода

        private IChecker[][] ExecuteOperationToKill(IChecker[][] newBoardState, IChecker killer, IChecker victim, Coord coord)
        {
            if (killer.CheckPossibilityToKill(coord, gameField))
            {
                newBoardState[killer.CurrentCoord.Row][killer.CurrentCoord.Column] = null;
                killer.CurrentCoord = coord;
                newBoardState[killer.CurrentCoord.Row][killer.CurrentCoord.Column] = killer;
                newBoardState[victim.CurrentCoord.Row][victim.CurrentCoord.Column] = null;
                CheckGettingKing(killer);
            }

            return newBoardState;
        } //виртуальное исполнение команды на убийство

        private IChecker[][] ExecuteOperationToMove(IChecker[][] newBoardState, IChecker mover, Coord coord)
        {
            if (mover.CheckPossibilityToMove(coord, gameField))
            {
                newBoardState[mover.CurrentCoord.Row][mover.CurrentCoord.Column] = null;
                mover.CurrentCoord = coord;
                newBoardState[mover.CurrentCoord.Row][mover.CurrentCoord.Column] = mover;
                CheckGettingKing(mover);
            }

            return newBoardState;
        }  //виртуальное исполнение команды на движение

        private void CheckGettingKing(IChecker checker)
        {
            if (checker is WhiteChecker)
            {
                if (checker.CurrentCoord.Row == 1)
                    checker.BecomeKingVirtualy();
            }
            else
            {
                if (checker.CurrentCoord.Row == 8)
                    checker.BecomeKingVirtualy();
            }
        }

        private List<IChecker> GetAllies(Lib.PlayersSide side, IChecker[][] currentBoardState)
        {
            List<IChecker> allies = new List<IChecker>();
            int a = (side == Lib.PlayersSide.BLACK) ? 0 : 1;

            for (int i = 1; i < 9; i++)
            {
                for (int j = 1; j < 9; j++)
                {
                    IChecker checker = currentBoardState[i][j];
                    int b = (checker is BlackChecker) ? 0 : 1;
                    if (currentBoardState[i][j] != null && a == b)
                    {
                        allies.Add(currentBoardState[i][j]);
                    }
                }
            }

            return allies;
        }

        private List<IChecker> GetEnemies(Lib.PlayersSide side, IChecker[][] currentBoardState)
        {
            List<IChecker> enemies = new List<IChecker>();
            int a = (side == Lib.PlayersSide.BLACK) ? 0 : 1;

            for (int i = 1; i < 9; i++)
            {
                for (int j = 1; j < 9; j++)
                {
                    IChecker checker = currentBoardState[i][j];
                    int b = (checker is BlackChecker) ? 0 : 1;
                    if (currentBoardState[i][j] != null && a != b)
                    {
                        enemies.Add(currentBoardState[i][j]);
                    }
                }
            }

            return enemies;
        }
    }
}
