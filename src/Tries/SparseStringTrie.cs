namespace VDS.Common.Tries;

/// <summary>
/// Represents the classic use case of a Trie data structure, keys are strings with a character stored at each Node
/// </summary>
/// <typeparam name="T">Type of values to be stored</typeparam>
/// <remarks>
/// This is a sparse implementation so should be more memory efficient than the <see cref="StringTrie{T}"/> for many use cases
/// </remarks>
public class SparseStringTrie<T>
    : SparseCharacterTrie<string, T>
    where T : class
{
    /// <summary>
    /// Creates a new sparse String Trie
    /// </summary>
    public SparseStringTrie()
        : base(StringTrie<T>.KeyMapperFunction) { }
}