using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace UltimateChecker
{
    public interface IGameField
    {
        IChecker[][] Grid { get; }
        Grid FormGrid { get; set; }
        MainWindow MainWindow { get; set; }
        FieldState SaveState();
        void RestoreState(FieldState state);
        void StepsHistoryAdd(string log);
        bool CheckCheckersMovement(Coord newCoord, IChecker checker);
        IChecker GetVictim(Coord newCoord, IChecker checker);
        string[] StepsHistory { get; }
        List<IChecker> WhiteCheckers { get; }
        List<IChecker> BlackCheckers { get; }
        Lib.PlayersSide Turn { get; set; }
    }
}
