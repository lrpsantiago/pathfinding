using System;
using System.Collections.Generic;

namespace PushingBoxStudios.Pathfinding.PriorityQueues
{
    internal class FibonacciHeapNode<TKey, TValue> : IPair<TKey, TValue> where TKey : IComparable, IComparable<TKey>
    {
        public TKey Key { get; internal set; }

        public TValue Value { get; internal set; }

        public int Degree { get; set; }

        public bool IsMarked { get; set; }

        public FibonacciHeapNode<TKey, TValue> Parent { get; set; }

        public FibonacciHeapNode<TKey, TValue> Previous { get; set; }

        public FibonacciHeapNode<TKey, TValue> Next { get; set; }

        public IList<FibonacciHeapNode<TKey, TValue>> Children { get; set; }

        public FibonacciHeapNode(TKey key, TValue value)
        {
            Key = key;
            Value = value;
            Previous = this;
            Next = this;
            Children = new List<FibonacciHeapNode<TKey, TValue>>();
        }
    }
}
