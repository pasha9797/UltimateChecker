using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UltimateChecker
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            PaintCircles(grid);
            Game game = new Game(this);
        }

        private void PaintCircles(Grid gridForm)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    var rect = new Rectangle();
                    //rect.Height = 68;
                    //rect.Width = 68;
                    rect.Fill = ((i + j) % 2 != 0) ? Brushes.DarkRed : Brushes.LightYellow;
                    Grid.SetColumn(rect, j);
                    Grid.SetRow(rect, i);
                    gridForm.Children.Add(rect);
                }
            }
        }

        public Coord TryToMoveCheckerToAnotherCell(UserControl checker, Point checkersCoordinates, out Point newCoordinates)
        {
            Coord newCheckersCoord = new Coord(0, 0);

            double cellHeight = grid.ActualHeight / 8;
            double cellWidth = grid.ActualWidth / 8;

            newCheckersCoord.Column = Convert.ToInt32(Math.Ceiling(checkersCoordinates.X / cellWidth));
            newCheckersCoord.Row = Convert.ToInt32(Math.Ceiling(checkersCoordinates.Y / cellHeight));

            newCoordinates = default(Point);

            newCoordinates.X = cellWidth;
            newCoordinates.Y = cellHeight;

            return newCheckersCoord;
        }

        public void MoveCheckerToAnotherCell(UserControl checker, Coord newCheckersCoord)
        {
            Grid.SetColumn(checker, newCheckersCoord.Column - 1);
            Grid.SetRow(checker, newCheckersCoord.Row - 1);
        }
    }
}
