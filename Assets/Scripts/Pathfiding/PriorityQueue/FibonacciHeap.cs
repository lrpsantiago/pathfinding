using System;
using System.Collections.Generic;

namespace Assets.Scripts.Pathfiding.PriorityQueue
{
    public class FibonacciHeap<TKey, TValue> : IPriorityQueue<TKey, TValue>
        where TKey : IComparable
    {
        private Node _minNode;

        public int Count { get; private set; }

        public bool IsEmpty
        {
            get { return _minNode == null; }
        }

        public FibonacciHeap()
        {
            _minNode = null;
            Count = 0;
        }

        public void Clear()
        {
            _minNode = null;
            Count = 0;
        }

        public INode<TKey, TValue> Push(TKey key, TValue val)
        {
            var node = new Node(key, val);

            _minNode = MergeLists(_minNode, node);
            Count++;

            return node;
        }

        public INode<TKey, TValue> FindMinimum()
        {
            return _minNode;
        }

        public void DecreaseKey(INode<TKey, TValue> node, TKey newKey)
        {
            var casted = node as Node;

            if (casted == null)
            {
                throw new ArgumentException("node must be a FibonacciHeap.Node");
            }

            DecreaseKey(casted, newKey);
        }

        public void DecreaseKey(Node node, TKey newKey)
        {
            if (node == null)
            {
                throw new ArgumentException("node must be non-null.");
            }

            if (newKey.CompareTo(node.Key) > 0)
            {
                throw new ArgumentOutOfRangeException("New key is larger than old key.");
            }

            node.Key = newKey;
            var parent = node.Parent;

            if (parent != null && node.CompareTo(parent) < 0)
            {
                Cut(node, parent);
                CascadingCut(parent);
            }
            if (node.CompareTo(_minNode) < 0)
            {
                _minNode = node;
            }
        }

        public void Delete(INode<TKey, TValue> node)
        {
            var casted = node as Node;

            if (casted == null)
            {
                throw new ArgumentException("node must be a FibonacciHeap.Node");
            }

            Delete(casted);
        }

        public void Delete(Node node)
        {
            // This is a special implementation of decreaseKey that sets the
            // argument to the minimum value. This is necessary to make generic keys
            // work, since there is no MIN_VALUE constant for generic types.
            var parent = node.Parent;

            if (parent != null)
            {
                Cut(node, parent);
                CascadingCut(parent);
            }

            _minNode = node;

            Pop();
        }

        public INode<TKey, TValue> Pop()
        {
            var extractedMin = _minNode;

            if (extractedMin != null)
            {
                // Set parent to null for the minimum's children
                if (extractedMin.Child != null)
                {
                    var child = extractedMin.Child;

                    do
                    {
                        child.Parent = null;
                        child = child.Next;
                    }
                    while (child != extractedMin.Child);
                }

                var nextInRootList = extractedMin.Next == extractedMin ? null : extractedMin.Next;

                // Remove min from root list
                RemoveNodeFromList(extractedMin);
                Count--;

                // Merge the children of the minimum node with the root list
                _minNode = MergeLists(nextInRootList, extractedMin.Child);

                if (nextInRootList != null)
                {
                    _minNode = nextInRootList;
                    Consolidate();
                }
            }
            return extractedMin;
        }

        public void Union(IPriorityQueue<TKey, TValue> other)
        {
            var casted = other as FibonacciHeap<TKey, TValue>;

            if (casted == null)
            {
                throw new ArgumentException("other must be a FibonacciHeap");
            }

            Union(casted);
        }

        public void Union(FibonacciHeap<TKey, TValue> other)
        {
            _minNode = MergeLists(_minNode, other._minNode);
            Count += other.Count;
        }

        private void Cut(Node node, Node parent)
        {
            parent.Degree--;
            parent.Child = (node.Next == node ? null : node.Next);
            RemoveNodeFromList(node);
            MergeLists(_minNode, node);
            node.IsMarked = false;
        }

        private void CascadingCut(Node node)
        {
            var parent = node.Parent;

            if (parent != null)
            {
                if (node.IsMarked)
                {
                    Cut(node, parent);
                    CascadingCut(parent);
                }
                else
                {
                    node.IsMarked = true;
                }
            }
        }

        private void Consolidate()
        {
            if (_minNode == null)
            {
                return;
            }

            var aux = new List<Node>();
            var items = GetRootTrees();

            foreach (var current in items)
            {
                //Node current = it.next();
                var top = current;

                while (aux.Count <= top.Degree + 1)
                {
                    aux.Add(null);
                }

                // If there exists another node with the same degree, merge them
                while (aux[top.Degree] != null)
                {
                    if (top.Key.CompareTo(aux[top.Degree].Key) > 0)
                    {
                        Node temp = top;

                        top = aux[top.Degree];
                        aux[top.Degree] = temp;
                    }

                    LinkHeaps(aux[top.Degree], top);
                    aux[top.Degree] = null;
                    top.Degree++;
                }

                while (aux.Count <= top.Degree + 1)
                {
                    aux.Add(null);
                }

                aux[top.Degree] = top;
            }

            _minNode = null;

            for (int i = 0; i < aux.Count; i++)
            {
                if (aux[i] != null)
                {
                    // Remove siblings before merging
                    aux[i].Next = aux[i];
                    aux[i].Prev = aux[i];
                    _minNode = MergeLists(_minNode, aux[i]);
                }
            }
        }

        private IEnumerable<Node> GetRootTrees()
        {
            var items = new Queue<Node>();
            var current = _minNode;

            do
            {
                items.Enqueue(current);
                current = current.Next;
            }
            while (_minNode != current);

            return items;
        }

        private void RemoveNodeFromList(Node node)
        {
            var prev = node.Prev;
            var next = node.Next;
            prev.Next = next;
            next.Prev = prev;

            node.Next = node;
            node.Prev = node;
        }

        private void LinkHeaps(Node max, Node min)
        {
            RemoveNodeFromList(max);
            min.Child = MergeLists(max, min.Child);
            max.Parent = min;
            max.IsMarked = false;
        }

        private Node MergeLists(Node a, Node b)
        {
            if (a == null && b == null)
            {
                return null;
            }

            if (a == null)
            {
                return b;
            }

            if (b == null)
            {
                return a;
            }

            var temp = a.Next;
            a.Next = b.Next;
            a.Next.Prev = a;
            b.Next = temp;
            b.Next.Prev = b;

            return a.CompareTo(b) < 0 ? a : b;
        }

        public class Node : INode<TKey, TValue>
        {
            public TKey Key { get; set; }
            public TValue Value { get; set; }
            public int Degree { get; set; }
            public Node Parent { get; set; }
            public Node Child { get; set; }
            public Node Prev { get; set; }
            public Node Next { get; set; }
            public bool IsMarked { get; set; }

            public Node()
            {
            }

            public Node(TKey key, TValue val)
            {
                Key = key;
                Value = val;
                Next = this;
                Prev = this;
            }

            public int CompareTo(object other)
            {
                var casted = other as Node;

                if (casted == null)
                {
                    throw new NotImplementedException("Cannot compare to a non-Node object");
                }

                return Key.CompareTo(casted.Key);
            }
        }
    }
}
