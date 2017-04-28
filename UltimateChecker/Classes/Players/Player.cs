using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateChecker.Classes.Players
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

    public enum PlayersSide
    {
        WHITE,
        BLACK
    }

    

    class Player : IPlayer
    {

        public Task<ICommand> MakeStep(IGameField grid)
        {
            return null;
        }
    }
}
