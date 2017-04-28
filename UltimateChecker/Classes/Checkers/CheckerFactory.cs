using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltimateChecker.Classes.Checkers.White;
using UltimateChecker.Classes.Checkers.Black;
using System.Windows.Controls;

namespace UltimateChecker
{
    public class CheckerFactory
    {
        public static BlackChecker CreateBlack(Coord coord, IGameField gameField)
        {
            BlackChecker black = new BlackChecker(new BlackNormalCheckerState(), new BlackCheckerUI(coord));
            black.CurrentCoord = coord;
            gameField.Grid[coord.Row][coord.Column] = black;
            Grid.SetColumn(black.checkerUI, coord.Column - 1);
            Grid.SetRow(black.checkerUI, coord.Row - 1);
            gameField.FormGrid.Children.Add(black.checkerUI);
            return black;
        }

        public static WhiteChecker CreateWhite(Coord coord, IGameField gameField)
        {
            
            WhiteChecker white = new WhiteChecker(new WhiteNormalCheckerState(), new WhiteCheckerUI(coord));
            white.CurrentCoord = coord;
            gameField.Grid[coord.Row][coord.Column] = white;
            Grid.SetColumn(white.checkerUI, coord.Column - 1);
            Grid.SetRow(white.checkerUI, coord.Row - 1);
            gameField.FormGrid.Children.Add(white.checkerUI);
            WhiteCheckerUI checkerUI = white.checkerUI as WhiteCheckerUI;
            checkerUI.MovingToAnotherCell += gameField.MainWindow.TryToMoveCheckerToAnotherCell;
            return white;
        }
    }
}
