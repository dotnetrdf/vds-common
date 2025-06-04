using System;
using System.Collections;
using System.Collections.Generic;

namespace VDS.Common.Collections;

class LinkedSparseArrayEnumerator<T>
    : IEnumerator<T>
{
    public LinkedSparseArrayEnumerator(LinkedList<SparseArrayEntry<T>> linkedList, int length)
    {
        LinkedList = linkedList;
        Length = length;
        Index = -1;
    }

    private int Index { get; set; }

    private LinkedList<SparseArrayEntry<T>> LinkedList { get; set; }

    private LinkedListNode<SparseArrayEntry<T>> CurrentNode { get; set; } 

    private int Length { get; set; }

    public void Dispose()
    {
        // No dispose actions needed
    }

    public bool MoveNext()
    {
        if (Index == -1)
        {
            // First time we've been called so get the first node of the list
            CurrentNode = LinkedList.First;
        }
        if (CurrentNode != null)
        {
            // If we're at the index of the current node we are about to move past it and so should move to the next node
            if (CurrentNode.Value.Index == Index) CurrentNode = CurrentNode.Next;
        }
        Index++;
        // There are still values available if the index is less than the length
        return Index < Length;
    }

    public void Reset()
    {
        Index = -1;
        CurrentNode = null;
    }

    public T Current
    {
        get
        {
            if (Index == -1) throw new InvalidOperationException("Currently before the start of the enumerator, please call MoveNext() before accessing this property");
            if (Index >= Length) throw new InvalidOperationException("Past the end of the enumerator");

            // If no node either the linked list is empty or we've reached the end of it in which case simply return the default value
            if (CurrentNode == null) return default(T);
            // If we reached the index of the current node then return the value otherwise we have not reached it yet and we return the default value
            return CurrentNode.Value.Index == Index ? CurrentNode.Value.Value : default(T);
        }
    }

    object IEnumerator.Current => Current;
}