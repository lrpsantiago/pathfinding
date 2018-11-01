using System;

namespace PushingBoxStudios.Pathfinding.PriorityQueues
{
    internal class PairNode<TKey, TValue> : IPair<TKey, TValue> where TKey : IComparable, IComparable<TKey>
    {
        public TKey Key { get; internal set; }

        public TValue Value { get; internal set; }
    }
}
