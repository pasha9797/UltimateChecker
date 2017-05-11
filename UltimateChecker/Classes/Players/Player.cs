using System;
using System.Threading.Tasks;

namespace UltimateChecker.Classes.Players
{
    class Player : IPlayer
    {
        IGame game;
        ICommand turnCommand;
        Lib.PlayersSide side;
        public Lib.CapitulateDel Capitulate { get; set; }

        private bool waitingForStep = false;
        private bool stepCanceled = false;
        private IChecker victim;
        private Coord dest;
        private IChecker mover;

        public Player(IGame game, Lib.PlayersSide side)
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
            waitingForStep = true;
            stepCanceled = false;

            while (waitingForStep && !stepCanceled) ;

            if (stepCanceled)
            {
                return null;
            }

            if (IsVictim())
            {
                return new KillingCommand(game, mover, victim, dest);
            }
            else
            {
                return new MovingCommand(game, mover, dest);
            }
        }

        public void FinishStep(Coord coord, IChecker mover, IChecker victim)
        {
            waitingForStep = false;
            dest = coord;
            this.mover = mover;
            this.victim = victim;
        }

        public void CancelStep()
        {
            stepCanceled = true;
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
