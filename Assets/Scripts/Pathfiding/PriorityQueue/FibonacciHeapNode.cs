using System;

namespace PushingBoxStudios.Pathfinding.PriorityQueues
{
    internal class FibonacciHeapNode<TKey, TValue> : INode<TKey, TValue>, IComparable,
        IComparable<FibonacciHeapNode<TKey, TValue>> where TKey : IComparable, IComparable<TKey>
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

        public int CompareTo(object obj)
        {
            return CompareTo(obj as FibonacciHeap<TKey, TValue>);
        }

        public int CompareTo(FibonacciHeapNode<TKey, TValue> other)
        {
            return Key.CompareTo(other.Key);
        }
    }
}
