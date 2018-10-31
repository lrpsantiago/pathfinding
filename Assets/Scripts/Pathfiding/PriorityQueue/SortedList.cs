using System;
using System.Collections.Generic;

namespace PushingBoxStudios.Pathfinding.PriorityQueues
{
    internal class SortedList<TKey, TValue> : IPriorityQueue<TKey, TValue> where TKey : IComparable, IComparable<TKey>
    {
        private IList<IPair<TKey, TValue>> _list;

        public int Count
        {
            get { return _list.Count; }
        }

        public SortedList()
        {
            _list = new List<IPair<TKey, TValue>>();
        }

        public IPair<TKey, TValue> Push(TKey key, TValue value)
        {
            var node = new SortedListNode<TKey, TValue>
            {
                Key = key,
                Value = value
            };

            LinearAddition(node);

            return node;
        }

        public IPair<TKey, TValue> Pop()
        {
            var node = _list[0];
            _list.RemoveAt(0);

            return node;
        }

        public void DecreaseKey(IPair<TKey, TValue> item, TKey newKey)
        {
            _list.Remove(item);

            var node = item as SortedListNode<TKey, TValue>;
            node.Key = newKey;

            LinearAddition(node);
        }

        private void LinearAddition(IPair<TKey, TValue> item)
        {
            for (int i = 0; i < _list.Count; i++)
            {
                var otherNode = _list[i];

                if (item.Key.CompareTo(otherNode.Key) < 0)
                {
                    _list.Insert(i, item);
                    return;
                }
            }

            _list.Add(item);
        }
    }
}
