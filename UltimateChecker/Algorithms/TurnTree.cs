using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateChecker.Algorithms
{
    class TurnNode : IComparable
    {
        public IChecker[][] state;
        public double value;
        public ICommand turnCommand;
        public int level; // четный - черные

        List<TurnNode> childs = new List<TurnNode>();

        public TurnNode(IChecker[][] state, double value, int level)
        {
            this.state = state;
            this.value = value;
            this.level = level;
        }

        public int CompareTo(object other)
        {
            return value.CompareTo((other as TurnNode).value);
        }

        public void AddChild(TurnNode child)
        {
            childs.Add(child);
        }

        public void AddChild(List<TurnNode> childs)
        {
            childs.ForEach((node) => this.childs.Add(node));
        }

        public double CountValue()
        {
            if (childs.Count == 0) return value;
            else
            {
                double newValue = 50;

                if (level % 2 == 0) //для черных
                {
                    double max = value;

                    foreach (TurnNode node in childs)
                    {
                        double nodeValue = node.CountValue();
                        if (nodeValue > max) max = nodeValue;
                    }
                    newValue = max;
                }
                else if (level % 2 == 1) //для белых
                {
                    double min = 1000;

                    foreach (TurnNode node in childs)
                    {
                        double nodeValue = node.CountValue();
                        if (nodeValue < min) min = nodeValue;
                    }
                    newValue = min;
                }
                return newValue;
            }
        }
    }

    class TurnTree
    {

    }
}
