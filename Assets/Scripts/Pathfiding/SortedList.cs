using System;
using System.Collections.Generic;

namespace PushingBoxStudios.Pathfinding
{
    internal class SortedList<T> : List<T>, IPriorityQueue<T> where T : class, IComparable
    {
        public void Push(T item)
        {
            this.LinearAddition(item);
            //this.BinarySearchAddition(item);
        }

        public T Pop()
        {
            T item = this[0];
            this.Remove(item);

            return item;
        }

        private void LinearAddition(T item)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (item.CompareTo(this[i]) < 0)
                {
                    this.Insert(i, item);
                    return;
                }
            }

            base.Add(item);
        }

        private void BinarySearchAddition(T item)
        {
            if (this.Count > 0)
            {
                int minIndex = 0;
                int maxIndex = this.Count - 1;

                while (minIndex <= maxIndex)
                {
                    int midIndex = (minIndex + maxIndex) / 2;
                    T middle = this[midIndex];

                    int comp = item.CompareTo(middle);

                    if (comp > 0)
                    {
                        minIndex = midIndex + 1;
                    }
                    else if (comp < 0)
                    {
                        maxIndex = midIndex - 1;
                    }
                    else
                    {
                        for (int i = midIndex; i < this.Count; i++)
                        {
                            if (item.CompareTo(this[i]) < 0)
                            {
                                this.Insert(i, item);
                                return;
                            }
                        }

                        base.Add(item);
                        return;
                    }
                }
            }

            base.Add(item);
        }
    }
}
