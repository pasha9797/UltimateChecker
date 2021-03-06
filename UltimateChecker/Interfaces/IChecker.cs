﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace UltimateChecker
{
    public class Coord
    {
        public int Row { get; set; }
        public int Column { get; set; }

        public Coord(int row, int column)
        {
            Row = row;
            Column = column;
        }

    }

    public interface IChecker:ICloneable
    {
        Coord CurrentCoord { get; set; }
        UserControl checkerUI { get; set; }        
        ICheckerState CheckerState { get; set; }
        bool IsKing { get; }

        bool CheckPossibilityToMove(Coord coord, IGameField field);
        bool CheckPossibilityToKill(Coord coord, IGameField field);
        bool CheckAllPossibilitiesToKill(IGameField filed);

        event Lib.CoordChangedDel CoordChanged;

        IChecker GetVictim(Coord coord, IGameField field);
        bool CoordChangedFromForm(Coord newCoord);
        void BecomeKing();
        void BecomeNormal();
        void BecomeKingVirtualy();
        void BecomeNormalVirtualy();



        Coord MoveForwardLeft(int numberOfSteps);
        Coord MoveForwardRight(int numberOfSteps);
        Coord MoveBackLeft(int numberOfSteps);
        Coord MoveBackRight(int numberOfSteps);
    }

    public interface ICheckerState
    {
        bool CheckPossibilityToMove(Coord CurrentCoord, Coord DestCoord, IGameField field);
        bool CheckPossibilityToKill(Coord CurrentCoord, Coord DestCoord, IGameField field);
        IChecker GetVictim(Coord CurrentCoord, Coord DestCoord, IGameField field);
        Coord MoveForwardLeft(Coord CurrentCoord, int numberOfSteps);
        Coord MoveForwardRight(Coord CurrentCoord, int numberOfSteps);
        Coord MoveBackLeft(Coord CurrentCoord, int numberOfSteps);
        Coord MoveBackRight(Coord CurrentCoord, int numberOfSteps);
    }
}
