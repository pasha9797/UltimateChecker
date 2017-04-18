using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UltimateChecker
{
    class GameField : IGameField
    {
        private IChecker[][] grid;
        private List<String> stepsHistory;

        public IChecker[][] Grid
        {
            get
            {
                return grid;
            }
        }
        public string[] StepsHistory
        {
            get
            {
                return stepsHistory.ToArray();
            }
        }

        public FieldState SaveState() //реализация хранителя | Сохранение
        {
            return new FieldState(Lib.CreateCopy(grid), Lib.CreateCopy(stepsHistory));
        }

        public void RestoreState(FieldState state) //реализация хранителя | Восстановление 
        {
            this.grid = state.Grid;
            this.stepsHistory = state.StepsHistory;
        }

        public void StepsHistoryAdd(string log)
        {
            stepsHistory.Add(log);
        }

        public GameField()
        {
            grid = new IChecker[9][];
            for(int i=1; i<=8;i++)
            {
                grid[i] = new IChecker[9];
            }

            stepsHistory = new List<string>();

            for(int i=1;i<=8;i++) //генерация шашек
            {
                if(i % 2 ==0)
                {
                    grid[7][i] = new WhiteChecker(new WhiteNormalChecker());
                    grid[3][i] = new BlackChecker(new BlackNormalChecker());
                    grid[1][i] = new BlackChecker(new BlackNormalChecker());
                }
                else
                {
                    grid[2][i] = new BlackChecker(new BlackNormalChecker());
                    grid[6][i] = new WhiteChecker(new WhiteNormalChecker());
                    grid[8][i] = new WhiteChecker(new WhiteNormalChecker());
                }
            }
        }
    }
}
