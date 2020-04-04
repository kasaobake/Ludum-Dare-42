using System;

namespace Kameosa
{
    namespace Collections
    {
        namespace Heap
        {
            public interface IHeapNode<T> : IComparable<T>
            {
                int HeapIndex { get; set; }
            }
        }
    }
}