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
            return await Task<ICommand>.Run<ICommand>(new Func<ICommand>(MakeTurn));
        }

        private ICommand MakeTurn()
        {
            WaitingForStep = true;
            while (WaitingForStep) ;
            if (IsVictim())
            {
                return new KillingCommand(game, mover, victim, dest);
            }
            else
            {
                return new MovingCommand(game, mover, dest);
            }
        }

        public void StepFinished(Coord coord, IChecker mover, IChecker victim)
        {
            WaitingForStep = false;
            dest = coord;
            this.mover = mover;
            this.victim = victim;
        }

        private bool IsVictim()
        {
            return victim != null;
        }

        public void RecieveCommandFromForm(IChecker checker, Coord currentCoord, Coord destination)
        { 
        }
    }
}
