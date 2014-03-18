using System.Collections.Generic;

namespace VDS.Common.Collections
{
    /// <summary>
    /// Interface for bounded lists which are extensions to the standard list contract with some additional constraints
    /// </summary>
    /// <remarks>
    /// <para>
    /// The primary constraint on a bounded list is that it is guaranteed to never grow beyond it's configured capacity.  Implementations may internally grow the data structures used to store the elements up to that limit using whatever strategy they desire but they <strong>must</strong> never allow the size of the list to grow beyond the capacity.
    /// </para>
    /// <para>
    /// The second constraint on a bounded list is with regards to its behaviour when a user attempts to assert more elements than the list has capacity for.  The behaviour in this regard is declared via the <see cref="OverflowPolicy"/> property, depending on the policy declared different behaviours may occur.
    /// </para>
    /// </remarks>
    /// <typeparam name="T">Element type</typeparam>
    public interface IBoundedList<T> 
        : IList<T>
    {
        /// <summary>
        /// Gets the overflow policy that applies when attempting to add more elements to the list than there is capacity for
        /// </summary>
        BoundedListOverflowPolicy OverflowPolicy { get; }

        /// <summary>
        /// Gets the maximum capacity of the bounded list i.e. the maximum number of elements it holds
        /// </summary>
        int MaxCapacity { get; }
    }
}
