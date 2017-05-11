using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateChecker.Classes.Game
{
    class KillingCommand : ICommand
    {
        IGame resiver;
        IChecker killer;
        IChecker victim;
        Coord destination;

        public KillingCommand(IGame game, IChecker killer, IChecker victim, Coord destination)
        {
            resiver = game;
            this.killer = killer;
            this.victim = victim;
            this.destination = destination;
        }

        public void Execute()
        {
            resiver.ExecuteStep(killer, victim, destination);
        }

        public void Cansel()
        {
            resiver.UndoStep(this);
        }

        public string Name()
        {
            return "Killing Command";
        }
    }
}
