using System;
using System.Collections.Generic;

namespace PushingBoxStudios.Pathfinding.PriorityQueues
{
    public enum HeapType
    {
        MinHeap,
        MaxHeap
    }

    //public class BinaryHeap<T> : IPriorityQueue<T> where T : IComparable
    //{
    //    private List<T> m_heap;

    //    public int Count
    //    {
    //        get { return m_heap.Count; }
    //    }

    //    public BinaryHeap()
    //    {
    //        m_heap = new List<T>();
    //    }

    //    public void Push(T item)
    //    {
    //        m_heap.Add(item);
    //        int lastIndex = m_heap.Count - 1;

    //        if (m_heap[lastIndex].CompareTo(m_heap[0]) < 0)
    //        {
    //            this.Swap(0, lastIndex);
    //        }

    //        this.BottomUpHeapfy();
    //    }

    //    private void BottomUpHeapfy()
    //    {
    //        int nodeIndex = this.Count - 1;

    //        while (nodeIndex > 0)
    //        {
    //            T current = m_heap[nodeIndex];

    //            int parentIndex = this.GetParentIndex(nodeIndex);
    //            T parent = m_heap[parentIndex];

    //            if (current.CompareTo(parent) < 0)
    //            {
    //                this.Swap(nodeIndex, parentIndex);
    //                nodeIndex = parentIndex;
    //            }
    //            else
    //            {
    //                break;
    //            }
    //        }
    //    }

    //    public T Pop()
    //    {
    //        T aux = m_heap[0];
    //        int lastIndex = this.Count - 1;

    //        this.Swap(0, lastIndex);
    //        m_heap.RemoveAt(lastIndex);

    //        this.TopDownHeapfy();

    //        return aux;
    //    }

    //    private void TopDownHeapfy()
    //    {
    //        int currentIndex = 0;

    //        while (true)
    //        {
    //            int leftIndex = this.GetLeftChildIndex(currentIndex);
    //            int rightIndex = this.GetRightChildIndex(currentIndex);
    //            int bestIndex = currentIndex;

    //            if (leftIndex < this.Count)
    //            {
    //                T best = m_heap[bestIndex];
    //                T left = m_heap[leftIndex];

    //                if (best.CompareTo(left) > 0)
    //                {
    //                    bestIndex = leftIndex;
    //                }
    //            }

    //            if (rightIndex < this.Count)
    //            {
    //                T best = m_heap[bestIndex];
    //                T right = m_heap[rightIndex];

    //                if (best.CompareTo(right) > 0)
    //                {
    //                    bestIndex = rightIndex;
    //                }
    //            }

    //            if (bestIndex != currentIndex)
    //            {
    //                this.Swap(currentIndex, bestIndex);
    //                currentIndex = bestIndex;
    //            }
    //            else
    //            {
    //                break;
    //            }
    //        }
    //    }

    //    private int GetParentIndex(int nodeIndex)
    //    {
    //        return (nodeIndex - 1) / 2;
    //    }

    //    private int GetRightChildIndex(int nodeIndex)
    //    {
    //        return (nodeIndex + 1) * 2 - 1;
    //    }

    //    private int GetLeftChildIndex(int nodeIndex)
    //    {
    //        return (nodeIndex + 1) * 2;
    //    }

    //    private void Swap(int index1, int index2)
    //    {
    //        T aux = m_heap[index1];

    //        m_heap[index1] = m_heap[index2];
    //        m_heap[index2] = aux;
    //    }

    //    public void Print()
    //    {
    //        for (int i = 0; i < m_heap.Count; i++)
    //        {
    //            Console.Out.Write(m_heap[i].ToString() + "\t");
    //        }
    //    }
    //}

    public class Heap<TKey, TValue> : IPriorityQueue<TKey, TValue> where TKey : IComparable, IComparable<TKey>
    {
        private List<IPair<TKey, TValue>> _heap;

        public HeapType Type { get; private set; }

        public int Count
        {
            get { return _heap.Count; }
        }

        public Heap(HeapType type = HeapType.MinHeap)
        {
            _heap = new List<IPair<TKey, TValue>>();
            Type = type;
        }

        public IPair<TKey, TValue> Push(TKey key, TValue value)
        {
            var node = new SortedListNode<TKey, TValue>
            {
                Key = key,
                Value = value
            };

            _heap.Add(node);

            var i = _heap.Count - 1;
            var flag = true;

            if (Type == HeapType.MaxHeap)
            {
                flag = false;
            }

            while (i > 0)
            {
                if ((_heap[i].Key.CompareTo(_heap[(i - 1) / 2].Key) > 0) ^ flag)
                {
                    var temp = _heap[i];
                    _heap[i] = _heap[(i - 1) / 2];
                    _heap[(i - 1) / 2] = temp;
                    i = (i - 1) / 2;
                }
                else
                {
                    break;
                }
            }

            return node;
        }

        public IPair<TKey, TValue> Pop()
        {
            var result = _heap[0];

            DeleteRoot();

            return result;
        }

        public void DecreaseKey(IPair<TKey, TValue> node, TKey newKey)
        {
            var item = node as SortedListNode<TKey, TValue>;

            item.Key = newKey;

            //throw new NotImplementedException();
        }

        private void DeleteRoot()
        {
            int i = _heap.Count - 1;

            _heap[0] = _heap[i];
            _heap.RemoveAt(i);

            i = 0;

            bool flag = true;

            if (Type == HeapType.MaxHeap)
                flag = false;

            while (true)
            {
                int leftInd = 2 * i + 1;
                int rightInd = 2 * i + 2;
                int largest = i;

                if (leftInd < _heap.Count)
                {
                    if ((_heap[leftInd].Key.CompareTo(_heap[largest].Key) > 0) ^ flag)
                    {
                        largest = leftInd;
                    }
                }

                if (rightInd < _heap.Count)
                {
                    if ((_heap[rightInd].Key.CompareTo(_heap[largest].Key) > 0) ^ flag)
                    {
                        largest = rightInd;
                    }
                }

                if (largest != i)
                {
                    var temp = _heap[largest];
                    _heap[largest] = _heap[i];
                    _heap[i] = temp;
                    i = largest;
                }
                else
                {
                    break;
                }
            }
        }
    }
}
