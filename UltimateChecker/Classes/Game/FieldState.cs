using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateChecker
{
    public class FieldState
    {
        public IChecker[][] Grid { get; private set; }
        public List<string> StepsHistory { get; private set; }
        public Lib.PlayersSide Turn { get; private set; }
        public Dictionary<IChecker, bool> WhoWasKing { get; private set; }

        public FieldState(IChecker[][] grid, List<string> stepsHistory, Lib.PlayersSide turn)
        {
            Grid = new IChecker[9][];
            WhoWasKing = new Dictionary<IChecker, bool>();
            for (int i = 1; i <= 8; i++)
            {
                Grid[i] = new IChecker[9];
                for(int j=1;j<=8;j++)
                {
                    Grid[i][j] = grid[i][j];
                    if(grid[i][j] != null)
                    {
                        WhoWasKing.Add(grid[i][j], grid[i][j].IsKing);
                    }
                }
            }

            StepsHistory = new List<string>();
            foreach (string message in stepsHistory)
            {
                StepsHistory.Add(message);
            }

            Turn = turn;
        }
    }
}
