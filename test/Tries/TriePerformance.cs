using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using NUnit.Framework;

namespace VDS.Common.Tries
{
    [TestFixture]
    public class TriePerformance
    {
        private List<String> GenerateWords(int numWords, int minLength, int maxLength)
        {
            char[] pool = Enumerable.Range('a', 26).Select(c => (char)c).ToArray();
            return GenerateWords(numWords, pool, minLength, maxLength);
        }

        private List<string> GenerateWords(int numWords, char[] pool, int minLength, int maxLength)
        {
            List<String> words = new List<string>();

            Random random = new Random();
            while (words.Count < numWords)
            {
                StringBuilder builder = new StringBuilder();

                // Select word length
                int wordLength = random.Next(minLength, maxLength + 1);

                // Fill word with random characters from the pool
                while (builder.Length < wordLength)
                {
                    builder.Append(pool[random.Next(pool.Length)]);
                }
                words.Add(builder.ToString());
            }

            return words;
        }


    }

    class TrieFiller<TKey, TKeyBit>
        where TKey : class
    {
        private readonly ITrie<TKey, TKeyBit, TKey> _trie;
        private readonly Queue<TKey> _keysToInsert = new Queue<TKey>(); 
        private readonly Stopwatch _timer = new Stopwatch();
        private Exception _error;


        public TrieFiller(ITrie<TKey, TKeyBit, TKey> trie, IEnumerable<TKey> keysToInsert)
        {
            this._trie = trie;
            foreach (TKey key in keysToInsert)
            {
                this._keysToInsert.Enqueue(key);
            }
        }

        public void Run()
        {
            this._timer.Stop();
            this._timer.Reset();
            this._timer.Start();

            try
            {
                while (this._keysToInsert.Count > 0)
                {
                    TKey key = this._keysToInsert.Dequeue();
                    this._trie.Add(key, key);
                }
            }
            catch (Exception ex)
            {
                this._error = ex;
            }
            finally
            {
                this._timer.Stop();
            }
        }

    }
}
