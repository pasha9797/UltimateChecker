using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateChecker
{
    public class WhiteChecker:IChecker
    {
        public ICheckerState CheckerState
        {
            get
            {
                return checkerState;
            }
            set
            {
                if (value is IWhiteCheckerState)
                    checkerState = value as IWhiteCheckerState;
            }
        }
        public Coord CurrentCoord { get; set; }
        private IWhiteCheckerState checkerState;

        public WhiteChecker(IWhiteCheckerState state)
        {
            CheckerState = state;
        }
        public bool CheckPossibilityToMove(Coord coord, IGameField field)
        {
            return CheckerState.CheckPossibilityToMove(CurrentCoord, coord, field);
        }

        public bool CheckPossibilityToKill(Coord coord, IGameField field)
        {
            return CheckerState.CheckPossibilityToKill(CurrentCoord, coord, field);
        }

    }

    public interface IWhiteCheckerState : ICheckerState
    {

    }
}

