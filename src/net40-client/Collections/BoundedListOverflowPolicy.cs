using System;

namespace VDS.Common.Collections
{
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
        /// When this policy is used attempting to add more items to a bounded list than there is capacity for <strong>must</strong> result in the excess items being silenty discarded
        /// </summary>
        Discard,
        /// <summary>
        /// When this policy is used attempting to add more items to a bounded list than there is capacity for <strong>must</strong> result in the excess items overwriting previously added items.  Which items are overwritten is a detail of the specific <see cref="IBoundedList{T}"/> implementation.
        /// </summary>
        Overwrite
    }
}