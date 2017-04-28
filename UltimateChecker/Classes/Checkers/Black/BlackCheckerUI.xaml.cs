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

namespace UltimateChecker.Classes.Checkers.Black
{
    /// <summary>
    /// Interaction logic for BlackCheckerUI.xaml
    /// </summary>
    public partial class BlackCheckerUI : UserControl
    {
        int row;
        int column;
        Point anchorPoint;
        Point currentPoint;
        private TranslateTransform transform = new TranslateTransform();
        bool isInDrag = false;

        public delegate void MoveCheckerDel(Coord destination);
        public event MoveCheckerDel MoveCheckerEvent;

        public BlackCheckerUI(Coord coord)
        {
            InitializeComponent();
            row = coord.Row;
            column = coord.Column;
        }

        private void Ellipse_MouseDown(object sender, MouseEventArgs e)
        {
            var element = sender as FrameworkElement;
            anchorPoint = e.GetPosition(null);
            element.CaptureMouse();
            isInDrag = true;
            e.Handled = true;
        }

        private void Ellipse_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (isInDrag)
            {
                var element = sender as FrameworkElement;
                element.ReleaseMouseCapture();
                isInDrag = false;
                e.Handled = true;
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
    }
}
