using System;
using System.Collections;
using System.Collections.Generic;
using VDS.Common.Trees;

namespace VDS.Common.Collections;

class BinarySparseArrayEnumerator<T>
    : IEnumerator<T>
{
    public BinarySparseArrayEnumerator(int length, IEnumerator<IBinaryTreeNode<int, T>> nodesEnumerator)
    {
        Length = length;
        Nodes = nodesEnumerator;
        Index = -1;
    }

    private IEnumerator<IBinaryTreeNode<int, T>> Nodes { get; set; }

    private IBinaryTreeNode<int, T> CurrentNode { get; set; } 

    private int Length { get; set; }

    private int Index { get; set; }

    public void Dispose()
    {
        // No dispose actions
    }

    public bool MoveNext()
    {
        if (Index == -1)
        {
            if (Nodes.MoveNext()) CurrentNode = Nodes.Current;
        }
        if (CurrentNode != null)
        {
            if (CurrentNode.Key == Index)
            {
                CurrentNode = Nodes.MoveNext() ? Nodes.Current : null;
            }
        }
        Index++;
        return Index < Length;
    }

    public void Reset()
    {
        Index = -1;
        CurrentNode = null;
        Nodes.Reset();
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
            return CurrentNode.Key == Index ? CurrentNode.Value : default(T);
        }
    }

    object IEnumerator.Current => Current;
}