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

namespace UltimateChecker.Classes.Checkers.White
{
    /// <summary>
    /// Interaction logic for WhiteCheckerUI.xaml
    /// </summary>
    public partial class WhiteCheckerUI : UserControl
    {
        int row;
        int column;
        Point anchorPoint;
        Point currentPoint;
        private TranslateTransform transform = new TranslateTransform();
        private TranslateTransform undoTransform = new TranslateTransform();
        private TranslateTransform discreteTransform = new TranslateTransform();
        bool isInDrag = false;
        public bool druggingIsPermitted = false;
        public IPlayer Player = null;

        public delegate Coord TryingToMoveToAnothreCellDel(UserControl checkr, Point coordinates, out Point cellSize);
        public event TryingToMoveToAnothreCellDel TryingToMoveToAnotherCell;

        public delegate void MovingToAnothreCellDel(UserControl checker, Coord newCoord);
        public event MovingToAnothreCellDel MovingToAnotherCell;


        public delegate bool CoordChangedFromFormDel(Coord newCoord);
        public event CoordChangedFromFormDel CoordChangedFromForm;

        public WhiteChecker ConnectedChecker;

        public WhiteCheckerUI(Coord coord)
        {
            InitializeComponent();
            row = coord.Row;
            column = coord.Column;
        }

        private void Ellipse_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (druggingIsPermitted)
            {
                var element = sender as FrameworkElement;
                anchorPoint = e.GetPosition(null);
                element.CaptureMouse();
                isInDrag = true;
                e.Handled = true;
                undoTransform.X = transform.X;
                undoTransform.Y = transform.Y;
            }
        }

        private void Ellipse_MouseMove(object sender, MouseEventArgs e)
        {
            if (isInDrag)
            {
                var element = sender as FrameworkElement;
                currentPoint = e.GetPosition(null);

                transform.X += currentPoint.X - anchorPoint.X;
                transform.Y += currentPoint.Y - anchorPoint.Y;
                this.RenderTransform = transform;
                anchorPoint = currentPoint;
            }
        }

        private void Ellipse_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (isInDrag)
            {
                var element = sender as FrameworkElement;
                element.ReleaseMouseCapture();
                isInDrag = false;
                e.Handled = true;

                Point cellSize;
                Coord newCoord = TryingToMoveToAnotherCell(this, e.GetPosition(null), out cellSize);
                MoveToAnotherCell(newCoord);
            }
        }

        private void MoveToAnotherCell(Coord newCoord)
        {
            if (CoordChangedFromForm(newCoord))
            {
                transform.X = 0;
                transform.Y = 0;
                row = newCoord.Row;
                column = newCoord.Column;
                MovingToAnotherCell(this, newCoord);
                Player.StepFinished(newCoord, ConnectedChecker, null); //victim - null означает что ход без убийства, я хз как запилить с убийством, тут ничего не понятно
            }
            else
            {
                transform.X = 0;
                transform.Y = 0;
            }

        }
    }
}
