using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateChecker
{
    public class BlackKingCheckerState : IBlackCheckerState
    {
        public bool CheckPossibility(Coord CurrentCoord, Coord DestCoord, IGameField field) //ррррекурсия
        {
            if (DestCoord.Row < 1 || DestCoord.Row > 8 || DestCoord.Column < 1 || DestCoord.Column > 8)
                return false; //за пределы поля
            if (field.Grid[DestCoord.Row][DestCoord.Column] != null)
                return false; //там занято

            if (CurrentCoord.Row == DestCoord.Row && CurrentCoord.Column == DestCoord.Column)
                return true; //выход из рекурсии

            int dRow = DestCoord.Row - CurrentCoord.Row;
            int dColumn = DestCoord.Column - CurrentCoord.Column;

            if (Math.Abs(dRow) != Math.Abs(dColumn))
                return false; //если ход не по диагонали

            int sRow = dRow / Math.Abs(dRow); //определяем знак dRow
            int sColumn = dColumn / Math.Abs(dColumn); //определяем знак dColumn

            IChecker neigbour = field.Grid[CurrentCoord.Row + sRow / 2][CurrentCoord.Column + sColumn / 2]; //ищем кого бить
            if (neigbour != null)
            {
                if (neigbour is BlackChecker)
                    return false; //если на пути своя шашка
            }
            return CheckPossibility(new Coord(CurrentCoord.Row + sRow, CurrentCoord.Column + sColumn), DestCoord, field);
        }
    }
}
