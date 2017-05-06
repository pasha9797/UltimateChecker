using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace UltimateChecker
{
    public class CheckerFactory
    {
        public static BlackChecker CreateBlack(Coord coord, IGameField gameField)
        {
            BlackChecker black = new BlackChecker(new BlackNormalCheckerState(), new CheckerUI());
            black.CurrentCoord = coord;
            (black.checkerUI as CheckerUI).AssignConnectedChecker(black);

            CheckerUI checkerUI = black.checkerUI as CheckerUI;
            checkerUI.CoordChangedFromForm += black.CoordChangedFromForm;
            black.CoordChanged += gameField.CheckCheckersMovement;
            gameField.Grid[coord.Row][coord.Column] = black;
            Grid.SetColumn(black.checkerUI, coord.Column - 1);
            Grid.SetRow(black.checkerUI, coord.Row - 1);
            gameField.FormGrid.Children.Add(black.checkerUI);
            checkerUI.TryingToMoveToAnotherCell += gameField.MainWindow.TryToMoveCheckerToAnotherCell;
            checkerUI.MovingToAnotherCell += gameField.MainWindow.MoveCheckerToAnotherCell;
            checkerUI.ConnectedChecker = black;
            checkerUI.GetVictim += gameField.GetVictim;
            return black;
        }

        public static WhiteChecker CreateWhite(Coord coord, IGameField gameField)
        {
            
            WhiteChecker white = new WhiteChecker(new WhiteNormalCheckerState(), new CheckerUI());
            white.CurrentCoord = coord;
            (white.checkerUI as CheckerUI).AssignConnectedChecker(white);

            CheckerUI checkerUI = white.checkerUI as CheckerUI;
            checkerUI.CoordChangedFromForm += white.CoordChangedFromForm;
            white.CoordChanged += gameField.CheckCheckersMovement;

            gameField.Grid[coord.Row][coord.Column] = white;
            Grid.SetColumn(white.checkerUI, coord.Column - 1);
            Grid.SetRow(white.checkerUI, coord.Row - 1);
            gameField.FormGrid.Children.Add(white.checkerUI);
            checkerUI.TryingToMoveToAnotherCell += gameField.MainWindow.TryToMoveCheckerToAnotherCell;
            checkerUI.MovingToAnotherCell += gameField.MainWindow.MoveCheckerToAnotherCell;
            checkerUI.ConnectedChecker = white;
            checkerUI.GetVictim += gameField.GetVictim;
            return white;
        }
    }
}
