using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateChecker
{
    public class BlackNormalChecker : IBlackCheckerState
    {
        public Coord CurrentCoord { get; set; }

        public bool CheckPossibility(Coord coord, IGameField field)
        {
            if (field.Grid[coord.Row][coord.Column] == null)
            {
                return CheckMove(coord);
            }
            else
            {
                if (field.Grid[coord.Row][coord.Column] is WhiteChecker)
                {
                   return CheckJump(coord);
                }
                else
                {
                    return false;
                }

            }
        }

        public bool CheckMove(Coord Coord)
        {
            if ((CurrentCoord.Row + 1 == Coord.Row) && (CurrentCoord.Column - 1 == Coord.Column))
            {
                return true;
            }
            if ((CurrentCoord.Row + 1 == Coord.Row) && (CurrentCoord.Column + 1 == Coord.Column))
            {
                return true;
            }
            return false;
        }

        public bool CheckJump(Coord Coord)
        {
            {
                if ((CurrentCoord.Row - 2 == Coord.Row) && (CurrentCoord.Column - 2 == Coord.Column))
                {
                    return true;
                }
                if ((CurrentCoord.Row - 2 == Coord.Row) && (CurrentCoord.Column + 2 == Coord.Column))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
