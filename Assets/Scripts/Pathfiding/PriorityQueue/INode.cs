using System;

namespace Assets.Scripts.Pathfiding.PriorityQueue
{
    public interface INode<TKey, TValue> : IComparable where TKey : IComparable
    {
        TKey Key { get; }
        TValue Value { get; }
    }
}
