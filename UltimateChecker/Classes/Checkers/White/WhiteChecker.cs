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

        public Coord MoveForwardLeft(int numberOfSteps)
        {
            return CheckerState.MoveForwardLeft(CurrentCoord, numberOfSteps);
        }

        public Coord MoveForwardRight(int numberOfSteps)
        {
            return CheckerState.MoveForwardRight(CurrentCoord, numberOfSteps);
        }

        public Coord MoveBackLeft(int numberOfSteps)
        {
            return (CheckerState is WhiteKingCheckerState) ? CheckerState.MoveBackLeft(CurrentCoord, numberOfSteps) : null;
        }

        public Coord MoveBackRight(int numberOfSteps)
        {
            return (CheckerState is WhiteKingCheckerState) ? CheckerState.MoveBackRight(CurrentCoord, numberOfSteps) : null;
        }
    }

    public interface IWhiteCheckerState : ICheckerState
    {

    }
}

