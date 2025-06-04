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

namespace VDS.Common.Collections;

/// <summary>
/// Possible overflow policies for bounded lists
/// </summary>
public enum BoundedListOverflowPolicy
{
    /// <summary>
    /// When this policy is used attempting to add more items to a bounded list than there is capacity for <strong>must</strong> result in an <see cref="InvalidOperationException"/>
    /// </summary>
    Error,

    /// <summary>
    /// When this policy is used attempting to add more items to a bounded list than there is capacity for <strong>must</strong> result in the excess items being silenty discarded.  When attempting to insert items then the behaviour will depend on where you are inserting. If inserting prior to the end of the list then inserting should cause items at the end of the list to be discarded if the capacity would be exceeded.  If inserting at the end of a list that is at capacity then the item to be inserted is itself discarded.
    /// </summary>
    Discard
}