using System;

namespace PushingBoxStudios.Pathfinding.PriorityQueues
{
    public interface IPriorityQueue<TKey, TValue> where TKey : IComparable, IComparable<TKey>
    {
        int Count { get; }

        void Clear();
        void DecreaseKey(INode<TKey, TValue> node, TKey newKey);
        void Delete(INode<TKey, TValue> node);
        INode<TKey, TValue> Pop();
        INode<TKey, TValue> FindMinimum();
        INode<TKey, TValue> Push(TKey key, TValue val);
        void Union(IPriorityQueue<TKey, TValue> other);
    }
}
