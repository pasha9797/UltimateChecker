using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateChecker
{
    public interface ICommand
    {
        void Execute();
        void Cansel();
        string Name();
    }
}