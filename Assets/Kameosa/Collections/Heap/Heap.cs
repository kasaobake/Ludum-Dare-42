using System;

namespace Kameosa
{
    namespace Collections
    {
        namespace Heap
        {
            public class Heap<T> where T : IHeapNode<T>
            {
                T[] nodes;
                int currentNodeCount;

                public int Count
                {
                    get
                    {
                        return this.currentNodeCount;
                    }
                }

                public Heap(int maxHeapSize)
                {
                    this.nodes = new T[maxHeapSize];
                }

                public void Add(T node)
                {
                    node.HeapIndex = this.currentNodeCount;
                    this.nodes[this.currentNodeCount] = node;

                    SortUp(node);

                    this.currentNodeCount++;
                }

                public T RemoveFirst()
                {
                    T firstNode = this.nodes[0];
                    this.currentNodeCount--;
                    this.nodes[0] = this.nodes[this.currentNodeCount];
                    this.nodes[0].HeapIndex = 0;

                    SortDown(this.nodes[0]);

                    return firstNode;
                }

                public void UpdateNode(T node)
                {
                    SortUp(node);
                }

                public bool Contains(T node)
                {
                    return Equals(this.nodes[node.HeapIndex], node);
                }

                void SortDown(T node)
                {
                    while (true)
                    {
                        int childIndexLeft = (node.HeapIndex * 2) + 1;
                        int childIndexRight = (node.HeapIndex * 2) + 2;
                        int swapIndex = 0;

                        if (childIndexLeft < this.currentNodeCount)
                        {
                            swapIndex = childIndexLeft;

                            if (childIndexRight < this.currentNodeCount)
                            {
                                if (this.nodes[childIndexLeft].CompareTo(this.nodes[childIndexRight]) < 0)
                                {
                                    swapIndex = childIndexRight;
                                }
                            }

                            if (node.CompareTo(this.nodes[swapIndex]) < 0)
                            {
                                Swap(node, this.nodes[swapIndex]);
                            }
                            else
                            {
                                return;
                            }

                        }
                        else
                        {
                            return;
                        }
                    }
                }

                void SortUp(T node)
                {
                    int parentIndex = (node.HeapIndex - 1) / 2;

                    while (true)
                    {
                        T parentNode = this.nodes[parentIndex];

                        if (node.CompareTo(parentNode) > 0)
                        {
                            Swap(node, parentNode);
                        }
                        else
                        {
                            break;
                        }

                        parentIndex = (node.HeapIndex - 1) / 2;
                    }
                }

                void Swap(T nodeA, T nodeB)
                {
                    this.nodes[nodeA.HeapIndex] = nodeB;
                    this.nodes[nodeB.HeapIndex] = nodeA;

                    int nodeAIndex = nodeA.HeapIndex;

                    nodeA.HeapIndex = nodeB.HeapIndex;
                    nodeB.HeapIndex = nodeAIndex;
                }
            }
        }
    }
}