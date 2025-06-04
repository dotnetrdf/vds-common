/*
VDS.Common is licensed under the MIT License

Copyright (c) 2012-2015 Robert Vesse
Copyright (c) 2016-2025 dotNetRDF Project (https://dotnetrdf.org/)

Permission is hereby granted, free of charge, to any person obtaining a copy of this software
and associated documentation files (the "Software"), to deal in the Software without restriction,
including without limitation the rights to use, copy, modify, merge, publish, distribute,
sublicense, and/or sell copies of the Software, and to permit persons to whom the Software 
is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or
substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND 
NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using System;
using System.Collections.Generic;
using System.Linq;

namespace VDS.Common.Trees;

/// <summary>
/// An enumerable over a binary tree nodes children which ensures that each time it is enumerated the latest state of the tree is enumerated
/// </summary>
/// <typeparam name="TKey">Key type</typeparam>
/// <typeparam name="TValue">Value type</typeparam>
internal abstract class ChildNodesEnumerable<TKey, TValue>
    : IEnumerable<IBinaryTreeNode<TKey,TValue>>
{
    /// <summary>
    /// Parent node
    /// </summary>
    protected readonly IBinaryTreeNode<TKey, TValue> Parent;

    /// <summary>
    /// Creates a new enumerable
    /// </summary>
    /// <param name="parent">Parent node</param>
    protected ChildNodesEnumerable(IBinaryTreeNode<TKey, TValue> parent)
    {
        Parent = parent ?? throw new ArgumentNullException(nameof(parent), "Parent cannot be null");
    }

    /// <summary>
    /// Get the child whose nodes we want to enumerate
    /// </summary>
    protected abstract IBinaryTreeNode<TKey, TValue> Child
    {
        get;
    }

    /// <summary>
    /// Gets an enumerator over the current state of one of the nodes children
    /// </summary>
    /// <returns>Enumerator over nodes</returns>
    public IEnumerator<IBinaryTreeNode<TKey, TValue>> GetEnumerator()
    {
        var child = Child;
        if (child == null) return Enumerable.Empty<IBinaryTreeNode<TKey, TValue>>().GetEnumerator();
        return child.Nodes.GetEnumerator();
    }

    /// <summary>
    /// Gets an enumerator over the current state of one of the nodes children
    /// </summary>
    /// <returns>Enumerator over nodes</returns>
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}