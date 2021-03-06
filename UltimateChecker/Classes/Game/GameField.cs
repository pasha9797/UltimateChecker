﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

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
        public Lib.PlayersSide Turn { get; set; }

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
            return new FieldState(grid, stepsHistory, Turn);
        }

        public void RestoreState(FieldState state) //реализация хранителя | Восстановление 
        {
            this.grid = state.Grid;
            this.stepsHistory = state.StepsHistory;
            this.Turn = state.Turn;

            for (int i = 1; i <= 8; i++)
            {
                for (int j = 1; j <= 8; j++)
                {
                    if (grid[i][j] != null)
                    {
                        grid[i][j].CurrentCoord.Row = i;
                        grid[i][j].CurrentCoord.Column = j;
                        if(!formGrid.Children.Contains(grid[i][j].checkerUI))
                        {
                            formGrid.Children.Add(grid[i][j].checkerUI);
                        }
                        if(state.WhoWasKing[grid[i][j]] == false && grid[i][j].IsKing)
                        {
                            grid[i][j].BecomeNormal();
                        }
                        mainWindow.MoveCheckerToAnotherCell(grid[i][j].checkerUI, grid[i][j].CurrentCoord);
                    }
                }
            }
        }

        public void StepsHistoryAdd(string log)
        {
            stepsHistory.Add(log);
        }

        public bool CheckCheckersMovement(Coord newCoord, IChecker checker)
        {
            bool result = false;
            ICommand commandToPlayer = null;

            if (checker.CheckPossibilityToMove(newCoord, this) || checker.CheckPossibilityToKill(newCoord, this))
            {
                result = true;
                SendCommandToPlayerEvent(checker, checker.CurrentCoord, newCoord);
            }
            return result;
        }

        public IChecker GetVictim(Coord newCoord, IChecker checker)
        {
            return checker.GetVictim(newCoord, this);
        }

        public GameField(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            this.formGrid = mainWindow.grid;
            CheckerFactory factory = CheckerFactory.GetInstance();

            grid = new IChecker[9][];
            for (int i = 1; i <= 8; i++)
            {
                grid[i] = new IChecker[9];
            }

            stepsHistory = new List<string>();
            Turn = Lib.PlayersSide.WHITE; // whites start

            for (int i = 1; i <= 8; i++) //генерация шашек
            {
                if (i % 2 == 0)
                {
                    factory.CreateWhite(new Coord(7, i), this);
                    factory.CreateBlack(new Coord(3, i), this);
                    factory.CreateBlack(new Coord(1, i), this);
                }
                else
                {
                    factory.CreateBlack(new Coord(2, i), this);
                    factory.CreateWhite(new Coord(6, i), this);
                    factory.CreateWhite(new Coord(8, i), this);
                }
            }

            //factory.CreateBlack(new Coord(5, 2), this);
            //factory.CreateBlack(new Coord(4, 3), this);
            //IChecker checker = factory.CreateWhite(new Coord(2, 3), this);
            //checker.BecomeKing();
        }
    }
}
