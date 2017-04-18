using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateChecker
{
    public class CheckerFactory
    {
        public static BlackChecker CreateBlack(Coord coord, IGameField gameField)
        {
            BlackChecker black = new BlackChecker(new BlackNormalChecker());
            black.CurrentCoord = coord;
            gameField.Grid[coord.Row][coord.Column] = black;
            return black;
        }

        public static WhiteChecker CreateWhite(Coord coord, IGameField gameField)
        {
            WhiteChecker white = new WhiteChecker(new WhiteNormalChecker());
            white.CurrentCoord = coord;
            gameField.Grid[coord.Row][coord.Column] = white;
            return white;
        }
    }
}
