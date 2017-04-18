using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace UltimateChecker.Algorithms
{
    internal struct Node<V, P>
    {
        public P priority;
        public V value;

        public Node(V value, P priority)
        {
            this.priority = priority;
            this.value = value;
        }
    }

    class PriorityQueue<V, P> where P : IComparable
    {
        List<Node<V, P>> list;

        public PriorityQueue()
        {
            list = new List<Node<V, P>>();
        }

        public void Enqueue(V value, P priority)
        {
            var node = new Node<V, P>(value, priority);
            list.Add(node);
            up(list.Count() - 1);
        }

        public V Dequeue()
        {
            Swap(0, list.Count() - 1);
            V element = list[list.Count() - 1].value;
            list.RemoveAt(list.Count() - 1);
            down(0);
            return element;
        }

        int Parent(int child)
        {
            return (child - 1) / 2;
        }

        int YoungChild(int parent)
        {
            return parent * 2 + 1;
        }

        int OldChild(int parent)
        {
            return parent * 2 + 2;
        }

        bool CompareListElements(int index1, int index2)
        {
            return list[index1].priority.CompareTo(list[index2].priority) >= 0;
        }

        void Swap(int index1, int index2)
        {
            var tmp = list[index1];
            list[index1] = list[index2];
            list[index2] = tmp;
        }

        void up(int index)
        {
            while (index != 0 && CompareListElements(index, Parent(index)))
            {
                Swap(index, Parent(index));
                index = Parent(index);
            }
        }

        void down(int index)
        {
            while (index < list.Count() / 2)
            {
                int maxI = YoungChild(index);
                if (OldChild(index) < list.Count() && CompareListElements(OldChild(index), YoungChild(index)))
                {
                    maxI = OldChild(index);
                }

                if (CompareListElements(index, maxI))
                {
                    return;
                }

                Swap(index, maxI);
                index = maxI;
            }
        }
    }
}