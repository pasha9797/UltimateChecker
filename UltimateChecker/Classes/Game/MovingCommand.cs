using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateChecker.Classes.Game
{
    class MovingCommand : ICommand
    {
        IGame resiver;
        IChecker mover;
        Coord destination;

        public MovingCommand(IGame game, IChecker mover, Coord destination)
        {
            resiver = game;
            this.mover = mover;
            this.destination = destination;
        }

        public void Execute()
        {
            resiver.ExecuteStep(mover, destination);
        }

        public void Cansel()
        {
            resiver.UndoStep(this);
        }

        public string Name()
        {
            return "Moving Command";
        }
    }
}
