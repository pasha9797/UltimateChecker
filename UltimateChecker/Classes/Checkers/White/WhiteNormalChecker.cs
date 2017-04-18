using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateChecker
{
   public class WhiteNormalChecker:IWhiteCheckerState
    {
        public Coord CurrentCoord { get; set; }

        public bool CheckPossibility(Coord coord, IGameField field)
        {
            /* if(targetCell.IsEmpty)
             * {
             *     CheckMove();
             * }
             * else
             * {
             *      CheckJump();
             * }
            */
            return false;
        }

        public bool CheckMove(Coord Coord)
        {
            if ((CurrentCoord.Row - 1 == Coord.Row) && (CurrentCoord.Column - 1 == Coord.Column))
            {
                return true;
            }
            if ((CurrentCoord.Row - 1 == Coord.Row) && (CurrentCoord.Column + 1 == Coord.Column))
            {
                return true;
            }
            return false;
        }

        public Coord CheckJump(Coord Coord)
        {
            {
                if ((CurrentCoord.Row + 2 == Coord.Row) && (CurrentCoord.Column - 2 == Coord.Column))
                {
                    return new Coord(CurrentCoord.Row + 1, CurrentCoord.Column - 1);
                }
                if ((CurrentCoord.Row + 2 == Coord.Row) && (CurrentCoord.Column + 2 == Coord.Column))
                {
                    return new Coord(CurrentCoord.Row + 1, CurrentCoord.Column + 1);
                }
            }
            return CurrentCoord;
        }
    }
}
