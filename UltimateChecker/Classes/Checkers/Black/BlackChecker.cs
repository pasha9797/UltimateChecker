using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

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

        public UserControl checkerUI { get; set; }

        private IBlackCheckerState checkerState;
        public Coord newCoord { get; set; }

        public event Lib.CoordChangedDel CoordChanged;

        public BlackChecker(IBlackCheckerState state, UserControl checkerUI)
        {
            CheckerState = state;
            this.checkerUI = checkerUI;
        }

        public bool CheckPossibilityToMove(Coord coord, IGameField field)
        {
            return CheckerState.CheckPossibilityToMove(CurrentCoord, coord, field);
        }

        public bool CheckPossibilityToKill(Coord coord, IGameField field)
        {
            return CheckerState.CheckPossibilityToKill(CurrentCoord, coord, field);
        }

        public bool CheckAllPossibilitiesToKill(IGameField field)
        {
            return
                CheckerState.CheckPossibilityToKill(CurrentCoord, new Coord(CurrentCoord.Row + 2, CurrentCoord.Column + 2), field) ||
                CheckerState.CheckPossibilityToKill(CurrentCoord, new Coord(CurrentCoord.Row + 2, CurrentCoord.Column - 2), field) ||
                CheckerState.CheckPossibilityToKill(CurrentCoord, new Coord(CurrentCoord.Row - 2, CurrentCoord.Column - 2), field) ||
                CheckerState.CheckPossibilityToKill(CurrentCoord, new Coord(CurrentCoord.Row - 2, CurrentCoord.Column + 2), field);
        }

        public IChecker GetVictim(Coord coord, IGameField field)
        {
            return CheckerState.GetVictim(CurrentCoord, coord, field);
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
            return (CheckerState is BlackKingCheckerState) ? CheckerState.MoveBackLeft(CurrentCoord, numberOfSteps) : null;
        }

        public Coord MoveBackRight(int numberOfSteps)
        {
            return (CheckerState is BlackKingCheckerState) ? CheckerState.MoveBackRight(CurrentCoord, numberOfSteps) : null;
        }

        public bool CoordChangedFromForm(Coord newCoord)
        {
            this.newCoord = newCoord;
            bool result = CoordChanged(newCoord, this); //проверка координат, присваивание, если перемещение возможно
            return result;
        }

        public void BecomeKing()
        {
            CheckerState = new BlackKingCheckerState();
            (checkerUI as CheckerUI).BecomeKing();
        }
    }

    public interface IBlackCheckerState : ICheckerState
    {

    }
}
