using System;

namespace PushingBoxStudios.Pathfinding.PriorityQueues
{
    internal class FibonacciHeapNode<TKey, TValue> : IPair<TKey, TValue> where TKey : IComparable, IComparable<TKey>
    {
        public TKey Key { get; private set; }

        public TValue Value { get; private set; }

        public int Degree { get; set; }

        public FibonacciHeapNode<TKey, TValue> Parent { get; set; }

        public FibonacciHeapNode<TKey, TValue> Pervious { get; set; }

        public FibonacciHeapNode<TKey, TValue> Next { get; set; }

        public FibonacciHeapNode<TKey, TValue> Child { get; set; }

        public bool IsMarked { get; set; }

        public FibonacciHeapNode(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
    }
}
