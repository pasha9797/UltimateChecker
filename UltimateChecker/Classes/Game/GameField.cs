using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using UltimateChecker.Classes.Game;

namespace UltimateChecker
{
    class GameField : IGameField
    {
        private IChecker[][] grid;
        private Grid formGrid;
        private List<String> stepsHistory;
        public MainWindow mainWindow;

        public IChecker[][] Grid
        {
            get
            {
                return grid;
            }
        }

        public Grid FormGrid
        {
            get
            {
                return formGrid;
            }
            set
            {
                formGrid = value;
            }
        }

        public string[] StepsHistory
        {
            get
            {
                return stepsHistory.ToArray();
            }
        }
        public List<IChecker> BlackCheckers
        {
            get
            {
                List<IChecker> blackList = new List<IChecker>();
                for (int i = 1; i <= 8; i++)
                {
                    for (int j = 1; j <= 8; j++)
                    {
                        if (grid[i][j] is BlackChecker)
                            blackList.Add(grid[i][j] as BlackChecker);
                    }
                }
                return blackList;
            }
        }
        public List<IChecker> WhiteCheckers
        {
            get
            {
                List<IChecker> whiteList = new List<IChecker>();
                for (int i = 1; i <= 8; i++)
                {
                    for (int j = 1; j <= 8; j++)
                    {
                        if (grid[i][j] is WhiteChecker)
                            whiteList.Add(grid[i][j] as WhiteChecker);
                    }
                }
                return whiteList;
            }
        }
        public PlayersSide Turn { get; set; }

        public delegate void SendCommandToPlayerDel(IChecker checker, Coord currentCoord, Coord destination);
        public event SendCommandToPlayerDel SendCommandToPlayerEvent;

        public MainWindow MainWindow
        {
            get
            {
                return mainWindow;
            }

            set
            {
                mainWindow = value;
            }
        }

        public FieldState SaveState() //реализация хранителя | Сохранение
        {
            return new FieldState(
                Lib.CreateCopy(grid),  //копия поля
                Lib.CreateCopy(stepsHistory), //копия истории ходов
                Lib.CreateCopy(Turn) //копия инф-ии о том чей ход
                );
        }

        public void RestoreState(FieldState state) //реализация хранителя | Восстановление 
        {
            this.grid = state.Grid;
            this.stepsHistory = state.StepsHistory;
            this.Turn = state.Turn;
        }

        public void StepsHistoryAdd(string log)
        {
            stepsHistory.Add(log);
        }

        public bool CheckCheckersMovement(Coord newCoord, IChecker checker)
        {
            bool result = false;
            ICommand commandToPlayer = null;

            if(checker.CheckPossibilityToMove(newCoord, this) || checker.CheckPossibilityToKill(newCoord, this))
            {
                result = true;
                SendCommandToPlayerEvent(checker, checker.CurrentCoord, newCoord);
            }
            return result;
        }

        public GameField(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            this.formGrid = mainWindow.grid;

            grid = new IChecker[9][];
            for (int i = 1; i <= 8; i++)
            {
                grid[i] = new IChecker[9];
            }

            stepsHistory = new List<string>();
            Turn = PlayersSide.WHITE; // whites start

            for (int i = 1; i <= 8; i++) //генерация шашек
            {
                if (i % 2 == 0)
                {
                    CheckerFactory.CreateWhite(new Coord(7, i), this);
                    CheckerFactory.CreateBlack(new Coord(3, i), this);
                    CheckerFactory.CreateBlack(new Coord(1, i), this);
                }
                else
                {
                    CheckerFactory.CreateBlack(new Coord(2, i), this);
                    CheckerFactory.CreateWhite(new Coord(6, i), this);
                    CheckerFactory.CreateWhite(new Coord(8, i), this);
                }
            }
        }
    }
}
