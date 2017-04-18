using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateChecker
{
    public struct Coord
    {
        public int Row { get; set; }
        public int Column { get; set; }

        public Coord(int row, int column)
        {
            Row = row;
            Column = column;
        }

    }

    public interface IChecker
    {
        Coord CurrentCoord { get; set; }
        ICheckerState CheckerState { get; set; }
        bool CheckPossibility(Coord coord, IGameField field);
    }
    public interface ICheckerState
    {
        bool CheckPossibility(Coord coord, IGameField field);
    }
}
