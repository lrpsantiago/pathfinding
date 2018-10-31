using System;

namespace PushingBoxStudios.Pathfinding.PriorityQueues
{
    public interface INode<TKey, TValue> : IComparable where TKey : IComparable, IComparable<TKey>
    {
        TKey Key { get; }
        TValue Value { get; }
    }
}
