using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateChecker
{
    public class BlackChecker : IChecker
    {
        public ICheckerState CheckerState
        {
            get
            {
                return checkerState;
            }
            set
            {
                if (value is IBlackCheckerState)
                    checkerState = value as IBlackCheckerState;
            }
        }
        public Coord CurrentCoord { get; set; }
        private IBlackCheckerState checkerState;

        public BlackChecker(IBlackCheckerState state)
        {
            CheckerState = state;
        }
        public bool CheckPossibility(Coord coord, IGameField field)
        {
            return CheckerState.CheckPossibility(CurrentCoord, coord, field);

        }


    }

    public interface IBlackCheckerState : ICheckerState
    {

    }
}
