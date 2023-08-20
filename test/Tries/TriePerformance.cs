/*
VDS.Common is licensed under the MIT License

Copyright (c) 2012-2015 Robert Vesse

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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using NUnit.Framework;

namespace VDS.Common.Tries
{
    [TestFixture, Category("Tries")]
    public class TriePerformance
    {
        private static List<string> GenerateWords(int numWords, int minLength, int maxLength)
        {
            var pool = Enumerable.Range('a', 26).Select(c => (char)c).ToArray();
            return GenerateWords(numWords, pool, minLength, maxLength);
        }

        private static List<string> GenerateWords(int numWords, char[] pool, int minLength, int maxLength)
        {
            var words = new List<string>();

            var random = new Random();
            while (words.Count < numWords)
            {
                var builder = new StringBuilder();

                // Select word length
                var wordLength = random.Next(minLength, maxLength + 1);

                // Fill word with random characters from the pool
                while (builder.Length < wordLength)
                {
                    builder.Append(pool[random.Next(pool.Length)]);
                }
                words.Add(builder.ToString());
            }

            return words;
        }

        private static List<TrieFiller<string, char>> CreateFillers(List<List<string>> threadWords, ITrie<string, char, string> trie)
        {
            var fillers = new List<TrieFiller<string, char>>();
            foreach (var threadWordList in threadWords)
            {
                fillers.Add(new TrieFiller<string, char>(trie, threadWordList));
            }
            return fillers;
        }

        private static List<List<string>> CreateThreadWordLists(List<string> words, int numThreads, int repeatsPerWord, int threadsPerWord, bool requireDistinctThreads)
        {
            if (threadsPerWord > numThreads) throw new ArgumentException("threadsPerWord must be <= numThreads", nameof(threadsPerWord));

            var threadWords = new List<List<string>>();
            for (var i = 0; i < numThreads; i++)
            {
                threadWords.Add(new List<string>());
            }

            var random = new Random();
            foreach (var word in words)
            {
                for (var i = 0; i < repeatsPerWord; i++)
                {
                    if (threadsPerWord == numThreads && requireDistinctThreads)
                    {
                        // Add to all threads
                        foreach (var threadWordList in threadWords)
                        {
                            threadWordList.Add(word);
                        }
                        continue;
                    }

                    // Add to one thread at random
                    var thread = random.Next(threadWords.Count);
                    threadWords[thread].Add(word);

                    if (threadsPerWord == 1) continue;

                    // At to further threads at random
                    if (!requireDistinctThreads)
                    {
                        // Threads are not required to be distinct so add to any threads at random
                        var added = 1;
                        while (added < threadsPerWord)
                        {
                            thread = random.Next(threadWords.Count);
                            threadWords[thread].Add(word);
                            added++;
                        }
                    }
                    else
                    {
                        // Threads are required to be distinct so track which threads we have added to
                        var selectedThreads = new HashSet<int>();
                        selectedThreads.Add(thread);
                        while (selectedThreads.Count < threadsPerWord)
                        {
                            thread = random.Next(threadWords.Count);
                            if (!selectedThreads.Add(thread)) continue;
                            threadWords[thread].Add(word);
                        }
                    }
                }
            }
            return threadWords;
        }

        private static void RunFillers(List<TrieFiller<string, char>> fillers)
        {
            // Launch threads
            var threads = new List<Thread>();
            foreach (var filler in fillers)
            {
                var start = new ThreadStart(filler.Run);
                var t = new Thread(start);
                t.Start();
                threads.Add(t);
            }

            // Wait for threads to finish
            while (threads.Count > 0)
            {
                for (var i = 0; i < threads.Count; i++)
                {
                    var filler = fillers[i];
                    if (filler.IsFinished) threads.RemoveAt(i);
                    i--;
                }
                Thread.Sleep(50);
            }

            // Report Performance
            foreach (var filler in fillers)
            {
                Console.Write(filler.HasError ? "[Error]" : "[Success]");
                Console.WriteLine(" Added {0} objects and Read {1} in {2}", filler.Added, filler.Read, filler.Elapsed);
            }
        }

        [TestCase(100, 15, 100, 1, 1, 1, true),
         TestCase(100, 15, 100, 2, 1, 1, true),
         TestCase(100, 15, 100, 4, 1, 2, true),
         TestCase(100, 15, 100, 4, 1, 4, true),
         TestCase(100, 15, 100, 4, 2, 2, true),
         TestCase(100, 15, 100, 4, 2, 4, true),
         TestCase(100, 15, 100, 4, 8, 2, true),
         TestCase(100, 15, 100, 4, 8, 4, true),
         TestCase(1000, 15, 100, 2, 1, 1, true),
         TestCase(1000, 15, 100, 4, 1, 2, true),
         TestCase(1000, 15, 100, 4, 1, 4, true),
         TestCase(100, 15, 100, 2, 1, 1, false),
         TestCase(100, 15, 100, 4, 1, 2, false),
         TestCase(100, 15, 100, 4, 1, 4, false),
         TestCase(1000, 15, 100, 2, 1, 1, false),
         TestCase(1000, 15, 100, 4, 1, 2, false),
         TestCase(1000, 15, 100, 4, 1, 4, false),
         TestCase(1000, 15, 100, 2, 8, 1, false),
         TestCase(1000, 15, 100, 4, 8, 2, false),
         TestCase(1000, 15, 100, 4, 8, 4, false)]
        public void TrieMultiThreadedPerformance(int numWords, int minLength, int maxLength, int numThreads, int repeatsPerWord, int threadsPerWord, bool requireDistinct)
        {
            var words = GenerateWords(numWords, minLength, maxLength);
            var threadWords = CreateThreadWordLists(words, numThreads, repeatsPerWord, threadsPerWord, requireDistinct);

            Console.WriteLine("StringTrie:");
            var fillers = CreateFillers(threadWords, new StringTrie<string>());
            RunFillers(fillers);
            Console.WriteLine();
            Console.WriteLine("SparseStringTrie:");
            fillers = CreateFillers(threadWords, new SparseStringTrie<string>());
            RunFillers(fillers);
        }
    }

    class TrieFiller<TKey, TKeyBit>
        where TKey : class
    {
        private readonly ITrie<TKey, TKeyBit, TKey> _trie;
        private readonly Queue<TKey> _keysToInsert = new Queue<TKey>(); 
        private readonly Queue<TKey> _keysToRead = new Queue<TKey>();
        private readonly Stopwatch _timer = new Stopwatch();

        public TrieFiller(ITrie<TKey, TKeyBit, TKey> trie, IEnumerable<TKey> keysToInsert)
        {
            _trie = trie;
            foreach (var key in keysToInsert)
            {
                _keysToInsert.Enqueue(key);
            }
        }

        public void Run()
        {
            Error = null;
            IsFinished = false;
            Added = 0;
            Read = 0;
            _timer.Stop();
            _timer.Reset();
            _timer.Start();

            try
            {
                while (_keysToInsert.Count > 0)
                {
                    var key = _keysToInsert.Dequeue();
                    _trie.Add(key, key);
                    Added++;
                    _keysToRead.Enqueue(key);
                }
                while (_keysToRead.Count > 0)
                {
                    var key = _keysToRead.Dequeue();
                    var value = _trie[key];
                    Read++;
                }
            }
            catch (Exception ex)
            {
                Error = ex;
            }
            finally
            {
                _timer.Stop();
                IsFinished = true;
            }
        }

        public bool HasError { get { return Error != null; } }

        public Exception Error { get; private set; }

        public int Added { get; private set; }

        public int Read { get; private set; }

        public TimeSpan Elapsed { get { return _timer.Elapsed; } }

        public bool IsFinished { get; private set; }
    }
}
