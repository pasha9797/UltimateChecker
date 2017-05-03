using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateChecker.Classes.Players
{
    class Player : IPlayer
    {
        IGame game;
        ICommand turnCommand;
        PlayersSide side;

        private bool WaitingForStep = false;
        private IChecker victim;
        private Coord dest;
        private IChecker mover;

        public Player(IGame game, PlayersSide side)
        {
            this.side = side;
            this.game = game;
        }

        public async Task<ICommand> MakeStep(IGameField grid)
        {
            return await MakeTurnTask();
        }

        Task<ICommand> MakeTurnTask()
        {
            return Task<ICommand>.Run<ICommand>(new Func<ICommand>(MakeTurn));
        }

        private ICommand MakeTurn()
        {
            WaitingForStep = true;
            while (WaitingForStep) ;
            if (victim == null)
                return new MovingCommand(game, mover, dest);
            else return new KillingCommand(game, mover, victim, dest);
        }

        public void StepFinished(Coord coord, IChecker mover, IChecker victim)
        {
            WaitingForStep = false;
            dest = coord;
            this.mover = mover;
            this.victim = victim;
        }

        public void RecieveCommandFromForm(IChecker checker, Coord currentCoord, Coord destination)
        { 
        }
    }
}
