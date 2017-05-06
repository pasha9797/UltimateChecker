using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateChecker
{
    public interface IPlayer
    {
        Task<ICommand> MakeStep(IGameField grid);
        void FinishStep(Coord coord, IChecker mover, IChecker victim);
        void CancelStep();
    }
}
