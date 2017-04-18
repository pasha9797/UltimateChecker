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
        public PlayersSide Turn { get; private set; }

        public FieldState(IChecker[][] grid, List<string> stepsHistory, PlayersSide turn)
        {
            this.Grid = grid;
            this.StepsHistory = stepsHistory;
            this.Turn = turn;
        }
    }
}
