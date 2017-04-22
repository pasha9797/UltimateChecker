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
        bool CheckPossibilityToMove(Coord coord, IGameField field);
        bool CheckPossibilityToKill(Coord coord, IGameField field);
    }

    public interface ICheckerState
    {
        bool CheckPossibilityToMove(Coord CurrentCoord, Coord DestCoord, IGameField field);
        bool CheckPossibilityToKill(Coord CurrentCoord, Coord DestCoord, IGameField field);
    }
}
