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

        public Player(IGame game, PlayersSide side)
        {
            this.side = side;
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
            return null;           
        }

        public void RecieveCommandFromForm(IChecker checker, Coord currentCoord, Coord destination)
        { 
        }
    }
}
