using System;
using System.Collections.Generic;

namespace PushingBoxStudios.Pathfinding.PriorityQueues
{
    public class FibonacciHeap<TKey, TValue> : IPriorityQueue<TKey, TValue>
        where TKey : IComparable, IComparable<TKey>
    {
        private LinkedList<FibonacciHeapNode<TKey, TValue>> _rootList;
        private FibonacciHeapNode<TKey, TValue> _minNode;

        public int Count { get; private set; }

        public void Clear()
        {
            _rootList = new LinkedList<FibonacciHeapNode<TKey, TValue>>();
            _minNode = null;
            Count = 0;
        }

        public void DecreaseKey(IPair<TKey, TValue> node, TKey newKey)
        {
            var hNode = node as FibonacciHeapNode<TKey, TValue>;

            hNode.Key = newKey;

            var parent = hNode.Parent;

            do
            {
                if (parent.IsMarked)
                {
                    Cut(parent);
                }
                else
                {
                    parent.IsMarked = true;
                    break;
                }
            }
            while (parent != null);
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
            var node = _minNode;

            return node;
        }

        public IPair<TKey, TValue> Push(TKey key, TValue value)
        {
            var node = new FibonacciHeapNode<TKey, TValue>(key, value);

            _rootList.AddLast(node);
            Count++;

            if (node.CompareTo(_minNode) < 0)
            {
                _minNode = node;
            }

            return node;
        }

        private void Union(FibonacciHeapNode<TKey, TValue> rootNodeA, FibonacciHeapNode<TKey, TValue> rootNodeB)
        {
            throw new System.NotImplementedException();
        }

        private void Consolidate()
        {
            return;
        }

        private void Cut(FibonacciHeapNode<TKey, TValue> node)
        {
            node.Parent = null;

            var prev = node.Previous;
            var next = node.Next;

            if (prev != null)
            {
                prev.Next = next;
            }

            if (next != null)
            {
                next.Previous = prev;
            }

            node.Next = null;
            node.Previous = null;
        }
    }
}