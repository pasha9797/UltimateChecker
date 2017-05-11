using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace UltimateChecker
{
    public class Lib
    {
        public static Dictionary<int, char> Signs = new Dictionary<int, char>() {
            { 1, 'A' },
            { 2, 'B' },
            { 3, 'C' },
            { 4, 'D' },
            { 5, 'E' },
            { 6, 'F' },
            { 7, 'G' },
            { 8, 'H' }
        };

        public enum PlayersSide
        {
            WHITE,
            BLACK
        }

        public delegate bool CoordChangedDel(Coord newCoord, IChecker instance);
        public delegate void CapitulateDel(PlayersSide side);

        public static UserControl DraggingChecker { get; set; }
    }
}
