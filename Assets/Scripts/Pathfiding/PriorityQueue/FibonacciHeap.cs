using System;

namespace PushingBoxStudios.Pathfinding.PriorityQueues
{
    public class FibonacciHeap<TKey, TValue> : IPriorityQueue<TKey, TValue> 
        where TKey : IComparable, IComparable<TKey>
    {
        private FibonacciHeapNode<TKey, TValue> _minNode;

        public int Count { get; private set; }

        public void Clear()
        {
            _minNode = null;
            Count = 0;
        }

        public void DecreaseKey(IPair<TKey, TValue> node, TKey newKey)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(IPair<TKey, TValue> node)
        {
            throw new System.NotImplementedException();
        }

        public IPair<TKey, TValue> FindMinimum()
        {
            return _minNode;
        }

        public IPair<TKey, TValue> Pop()
        {
            throw new System.NotImplementedException();
        }

        public IPair<TKey, TValue> Push(TKey key, TValue val)
        {
            throw new System.NotImplementedException();
        }

        public void Union(IPriorityQueue<TKey, TValue> other)
        {
            throw new System.NotImplementedException();
        }
    }
}