using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateChecker
{
    public class WhiteNormalCheckerState : IWhiteCheckerState
    {
        public bool CheckPossibility(Coord CurrentCoord, Coord DestCoord, IGameField field)
        {
            if (DestCoord.Row < 1 || DestCoord.Row > 8 || DestCoord.Column < 1 || DestCoord.Column > 8)
                return false; //за пределы поля
            if (field.Grid[DestCoord.Row][DestCoord.Column] != null)
                return false; //там занято

            int dRow = DestCoord.Row - CurrentCoord.Row;
            int dColumn = DestCoord.Column - CurrentCoord.Column;

            if (Math.Abs(dRow) == 1 && Math.Abs(dColumn) == 1)//если лишь один шаг
            {
                if (dRow == -1)
                    return true;//белым только вверх
            }

            else if (Math.Abs(dRow) == 2 && Math.Abs(dColumn) == 2)//если хотим бить
            {
                IChecker neigbour = field.Grid[CurrentCoord.Row + dRow / 2][CurrentCoord.Column + dColumn / 2]; //ищем кого бить
                if (neigbour != null && neigbour is BlackChecker) //если там враг
                {
                    return true;
                }
            }

            return false;
        }
    }
}
