namespace VDS.Common.Tries;

/// <summary>
/// Sparse Node of a Trie
/// </summary>
/// <typeparam name="TValue">Value Type</typeparam>
public class SparseCharacterTrieNode<TValue>
    : AbstractSparseTrieNode<char, TValue>
    where TValue : class
{
    private char _singleton = '\0';
    private ITrieNode<char, TValue> _singletonNode;

    /// <summary>
    /// Creates a new Sparse Character Trie Node
    /// </summary>
    /// <param name="parent">Parent Node</param>
    /// <param name="key">Key Bit</param>
    public SparseCharacterTrieNode(ITrieNode<char, TValue> parent, char key)
        : base(parent, key) { }

    /// <summary>
    /// Gets whether the given key matches the singleton
    /// </summary>
    /// <param name="key">Key Bit</param>
    /// <returns>True if it matches, false otherwise</returns>
    protected override bool MatchesSingleton(char key)
    {
        return key == _singleton;
    }

    /// <summary>
    /// Clears the singleton
    /// </summary>
    protected override void ClearSingleton()
    {
        _singleton = '\0';
        _singletonNode = null;
    }

    /// <summary>
    /// Creates a new child
    /// </summary>
    /// <param name="key">Key Bit</param>
    /// <returns>New Child</returns>
    protected override ITrieNode<char, TValue> CreateNewChild(char key)
    {
        return new SparseCharacterTrieNode<TValue>(this, key);
    }

    /// <summary>
    /// Gets/Sets the singleton child
    /// </summary>
    protected override ITrieNode<char, TValue> SingletonChild
    {
        get => _singletonNode;
        set
        {
            _singleton = value.KeyBit;
            _singletonNode = value;
        }
    }
}