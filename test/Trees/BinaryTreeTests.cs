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
using System.Linq;
using NUnit.Framework;

namespace VDS.Common.Trees
{
    [TestFixture, Category("Trees")]
    public class BinaryTreeTests
    {
        private readonly Random _rnd = new Random();

        private void TestOrderPreservationOnInsertStructs<TNode, TKey>(IEnumerable<TKey> input, ITree<TNode, TKey, TKey> tree)
            where TNode : class, ITreeNode<TKey, TKey>
            where TKey : struct
        {
            Console.Write("Inputs: ");
            var inputs = input.ToList();
            TestTools.PrintEnumerableStruct(inputs, ",");
            Console.WriteLine();
            foreach (var i in inputs)
            {
                tree.Add(i, i);
            }

            //Force a sort of the inputs because initial inputs may be unsorted
            inputs.Sort();
            Console.Write("Sorted Inputs (Expected Output): ");
            TestTools.PrintEnumerableStruct(inputs, ",");
            Console.WriteLine();

            var outputs = tree.Keys.ToList();
            Console.Write("Outputs: ");
            TestTools.PrintEnumerableStruct(outputs, ",");
            Console.WriteLine();
            TestOrderStructs(inputs, outputs);
        }

        private void TestOrderPreservationOnDeleteStructs<TNode, TKey>(IEnumerable<TKey> input, ITree<TNode, TKey, TKey> tree)
            where TNode : class, ITreeNode<TKey, TKey>
            where TKey : struct
        {
            Console.Write("Inputs: ");
            var inputs = input.ToList();
            TestTools.PrintEnumerableStruct(inputs, ",");
            Console.WriteLine();
            foreach (var i in inputs)
            {
                tree.Add(i, i);
            }

            //Now randomly delete up to half the nodes
            var toDelete = Math.Max(1, _rnd.Next(Math.Max(2, inputs.Count / 2)));
            Console.WriteLine("Going to delete " + toDelete + " nodes from the Tree");
            while (toDelete > 0)
            {
                var r = _rnd.Next(inputs.Count);
                var key = inputs[r];
                Console.Write("Deleting Key " + key + "...");
                Assert.IsTrue(tree.Remove(key), "Removing Key " + key + " Failed");
                inputs.RemoveAt(r);
                TestOrderStructs(inputs.OrderBy(k => k, Comparer<TKey>.Default).ToList(), tree.Keys.ToList());
                Assert.AreEqual(inputs.Count, tree.Nodes.Count(), "Removal of Key " + key + " did not reduce node count as expected");
                toDelete--;
            }

            Console.Write("Inputs after Deletions: ");
            TestTools.PrintEnumerableStruct(inputs, ",");

            //Force a sort of the inputs because initial inputs may be unsorted
            inputs.Sort();
            Console.Write("Sorted Inputs (Expected Output): ");
            TestTools.PrintEnumerableStruct(inputs, ",");
            Console.WriteLine();

            var outputs = tree.Keys.ToList();
            Console.Write("Outputs: ");
            TestTools.PrintEnumerableStruct(outputs, ",");
            Console.WriteLine();
            TestOrderStructs(inputs, outputs);
            Console.WriteLine();
        }

        private void TestOrderStructs<TKey>(List<TKey> inputs, List<TKey> outputs)
        {
            Assert.AreEqual(inputs.Count, outputs.Count, "Expected " + inputs.Count + " Keys in tree but only found " + outputs.Count);

            for (var i = 0; i < inputs.Count; i++)
            {
                if (i >= outputs.Count) Assert.Fail("Too few outputs, expected " + inputs.Count + " but got " + outputs.Count);
                var expected = inputs[i];
                var actual = outputs[i];
                Assert.AreEqual(expected, actual, "Expected " + expected + " at Position " + i + " but got " + actual);
            }
        }

        #region Unbalance Binary Tree

        [Test]
        public void BinaryTreeUnbalancedInsert1()
        {
            var tree = new UnbalancedBinaryTree<int, int>();
            TestOrderPreservationOnInsertStructs(Enumerable.Range(1, 10), tree);
        }

        [Test]
        public void BinaryTreeUnbalancedInsert2()
        {
            var tree = new UnbalancedBinaryTree<int, int>();
            TestOrderPreservationOnInsertStructs(Enumerable.Range(1, 100), tree);
        }

        [Test]
        public void BinaryTreeUnbalancedInsert3()
        {
            var tree = new UnbalancedBinaryTree<int, int>();

            //Randomize the input order
            var pool = Enumerable.Range(1, 100).ToList();
            var input = new List<int>();
            while (pool.Count > 0)
            {
                var r = _rnd.Next(pool.Count);
                input.Add(pool[r]);
                pool.RemoveAt(r);
            }

            TestOrderPreservationOnInsertStructs(input, tree);
        }

        [Test]
        public void BinaryTreeUnbalancedInsert4()
        {
            var tree = new UnbalancedBinaryTree<int, int>();

            //Randomize the input order
            var pool = Enumerable.Range(1, 1000).ToList();
            var input = new List<int>();
            while (pool.Count > 0)
            {
                var r = _rnd.Next(pool.Count);
                input.Add(pool[r]);
                pool.RemoveAt(r);
            }

            TestOrderPreservationOnInsertStructs(input, tree);
        }

        [Test]
        public void BinaryTreeUnbalancedIndexAccess1()
        {
            var tree = new UnbalancedBinaryTree<int, int>();
            var inputs = Enumerable.Range(1, 10).ToList();
            TestOrderPreservationOnInsertStructs(inputs, tree);

            for (var i = 0; i < 10; i++)
            {
                Assert.AreEqual(i + 1, tree.GetValueAt(i));
            }

            // Swap the values using index access
            for (int i = 0, j = 9; i < j; i++, j--)
            {
                var temp = tree.GetValueAt(i);
                tree.SetValueAt(i, tree.GetValueAt(j));
                tree.SetValueAt(j, temp);
            }

            TestTools.PrintEnumerableStruct(tree.Values, ",");
            for (var i = 0; i < 10; i++)
            {
                Assert.AreEqual(10 - i, tree.GetValueAt(i));
            }
        }

        [Test]
        public void BinaryTreeUnbalancedIndexAccess2()
        {
            var tree = new UnbalancedBinaryTree<int, int>();
            var inputs = Enumerable.Range(1, 10).ToList();
            TestOrderPreservationOnInsertStructs(inputs, tree);

            for (var i = 0; i < 10; i++)
            {
                Assert.AreEqual(i + 1, tree.GetValueAt(i));
            }

            // Remove values in random order
            while (tree.Root != null)
            {
                var index = _rnd.Next(tree.Root.Size);
                tree.RemoveAt(index);
                inputs.RemoveAt(index);
                TestOrderStructs(inputs, tree.Keys.ToList());
            }
        }

        private UnbalancedBinaryTree<int, int> GetTreeForDelete()
        {
            var tree = new UnbalancedBinaryTree<int, int>();
            tree.Root = new BinaryTreeNode<int, int>(null, 2, 2);
            tree.Root.LeftChild = new BinaryTreeNode<int, int>(tree.Root, 1, 1);
            tree.Root.RightChild = new BinaryTreeNode<int, int>(tree.Root, 3, 3);
            return tree;
        }

        [Test]
        public void BinaryTreeUnbalancedDelete1()
        {
            var tree = GetTreeForDelete();
            BinaryTreeTools.PrintBinaryTreeStructs(tree);
            tree.Remove(1);
            BinaryTreeTools.PrintBinaryTreeStructs(tree);
            Assert.IsNull(tree.Root.LeftChild, "Left Child should now be null");

            TestOrderStructs(new List<int> { 2, 3 }, tree.Keys.ToList());
        }

        [Test]
        public void BinaryTreeUnbalancedDelete2()
        {
            var tree = GetTreeForDelete();
            BinaryTreeTools.PrintBinaryTreeStructs(tree);
            tree.Remove(3);
            BinaryTreeTools.PrintBinaryTreeStructs(tree);
            Assert.IsNull(tree.Root.RightChild, "Right Child should now be null");

            TestOrderStructs(new List<int> { 1, 2 }, tree.Keys.ToList());
        }

        [Test]
        public void BinaryTreeUnbalancedDelete3()
        {
            var tree = GetTreeForDelete();
            BinaryTreeTools.PrintBinaryTreeStructs(tree);
            tree.Remove(2);
            BinaryTreeTools.PrintBinaryTreeStructs(tree);

            TestOrderStructs(new List<int> { 1, 3 }, tree.Keys.ToList());
        }

        [Test]
        public void BinaryTreeUnbalancedDelete4()
        {
            var tree = new UnbalancedBinaryTree<int, int>();
            var inputs = Enumerable.Range(1, 100).ToList();
            TestOrderPreservationOnInsertStructs(inputs, tree);
            Assert.IsTrue(tree.Remove(inputs[0]));
            inputs.RemoveAt(0);
            TestOrderStructs(inputs, tree.Keys.ToList());
        }

        [Test]
        public void BinaryTreeUnbalancedDelete5()
        {
            var tree = new UnbalancedBinaryTree<int, int>();
            var inputs = Enumerable.Range(1, 100).ToList();
            TestOrderPreservationOnInsertStructs(inputs, tree);
            Assert.IsTrue(tree.Remove(inputs[50]));
            inputs.RemoveAt(50);
            TestOrderStructs(inputs, tree.Keys.ToList());
        }

        [Test]
        public void BinaryTreeUnbalancedDelete6()
        {
            for (var i = 0; i < 10; i++)
            {
                var tree = new UnbalancedBinaryTree<int, int>();
                TestOrderPreservationOnDeleteStructs(Enumerable.Range(1, 10), tree);
            }
        }

        [Test]
        public void BinaryTreeUnbalancedDelete7()
        {
            for (var i = 0; i < 10; i++)
            {
                var tree = new UnbalancedBinaryTree<int, int>();
                TestOrderPreservationOnDeleteStructs(Enumerable.Range(1, 100), tree);
            }
        }

        [Test]
        public void BinaryTreeUnbalancedDelete8()
        {
            for (var i = 0; i < 10; i++)
            {
                //Randomize the input order
                var pool = Enumerable.Range(1, 25).ToList();
                var input = new List<int>();
                while (pool.Count > 0)
                {
                    var r = _rnd.Next(pool.Count);
                    input.Add(pool[r]);
                    pool.RemoveAt(r);
                }

                var tree = new UnbalancedBinaryTree<int, int>();
                TestOrderPreservationOnDeleteStructs(input, tree);
            }
        }

        [Test]
        public void BinaryTreeUnbalancedDelete9()
        {
            for (var i = 0; i < 10; i++)
            {
                //Randomize the input order
                var pool = Enumerable.Range(1, 1000).ToList();
                var input = new List<int>();
                while (pool.Count > 0)
                {
                    var r = _rnd.Next(pool.Count);
                    input.Add(pool[r]);
                    pool.RemoveAt(r);
                }

                var tree = new UnbalancedBinaryTree<int, int>();
                TestOrderPreservationOnDeleteStructs(input, tree);
            }
        }

        [Test]
        public void BinaryTreeUnbalancedDelete10()
        {
            var input = new List<int>() { 19,10,20,14,16,5,2,23,9,1,8,4,15,11,24,7,21,13,6,3,22,18,12,17,25 };
            var tree = new UnbalancedBinaryTree<int, int>();
            foreach (var i in input)
            {
                tree.Add(i, i);
            }
            var deletes = new List<int>() { 3, 11, 25, 2, 15, 19 };
            var count = input.Count;
            foreach (var i in deletes)
            {
                Console.WriteLine("Removing Key " + i);
                Assert.IsTrue(tree.Remove(i), "Failed to remove Key " + i);
                input.Remove(i);
                count--;
                TestOrderStructs(input.OrderBy(k => k, Comparer<int>.Default).ToList(), tree.Keys.ToList());
                Assert.AreEqual(count, tree.Nodes.Count(), "Removal of Key " + i + " did not reduce node count as expected");
            }
        }

        [Test]
        public void BinaryTreeUnbalancedDelete11()
        {
            var input = new List<int>() { 25, 14, 5, 8, 22, 17, 9, 12, 4, 1, 3, 23, 2, 7, 19, 20, 10, 24, 16, 6, 21, 13, 18, 11, 15 };
            var tree = new UnbalancedBinaryTree<int, int>();
            foreach (var i in input)
            {
                tree.Add(i, i);
            }
            var deletes = new List<int>() { 3, 14, 25 };
            var count = input.Count;
            foreach (var i in deletes)
            {
                Console.WriteLine("Removing Key " + i);
                Assert.IsTrue(tree.Remove(i), "Failed to remove Key " + i);
                input.Remove(i);
                count--;
                TestOrderStructs(input.OrderBy(k => k, Comparer<int>.Default).ToList(), tree.Keys.ToList());
                Assert.AreEqual(count, tree.Nodes.Count(), "Removal of Key " + i + " did not reduce node count as expected");
            }
        }

        [Test]
        public void BinaryTreeUnbalancedDelete12()
        {
            var input = new List<int>() { 1, 18, 17, 13, 16, 10, 7, 15, 9, 6, 3, 2, 24, 25, 8, 12, 11, 4, 14, 21, 23, 5, 20, 19, 22 };
            var tree = new UnbalancedBinaryTree<int, int>();
            foreach (var i in input)
            {
                tree.Add(i, i);
            }
            var deletes = new List<int>() { 1, 6, 18, 25, 2 };
            var count = input.Count;
            foreach (var i in deletes)
            {
                Console.WriteLine("Removing Key " + i);
                Assert.IsTrue(tree.Remove(i), "Failed to remove Key " + i);
                input.Remove(i);
                count--;
                TestOrderStructs(input.OrderBy(k => k, Comparer<int>.Default).ToList(), tree.Keys.ToList());
                Assert.AreEqual(count, tree.Nodes.Count(), "Removal of Key " + i + " did not reduce node count as expected");
            }
        }

        #endregion

        #region Scapegoat Tree

        [Test]
        public void BinaryTreeScapegoatInsert1()
        {
            var tree = new ScapegoatTree<int, int>();
            TestOrderPreservationOnInsertStructs(Enumerable.Range(1, 10), tree);
        }

        [Test]
        public void BinaryTreeScapegoatInsert2()
        {
            var tree = new ScapegoatTree<int, int>();
            TestOrderPreservationOnInsertStructs(Enumerable.Range(1, 100), tree);
        }

        [Test]
        public void BinaryTreeScapegoatInsert3()
        {
            var tree = new ScapegoatTree<int, int>();

            //Randomize the input order
            var pool = Enumerable.Range(1, 100).ToList();
            var input = new List<int>();
            while (pool.Count > 0)
            {
                var r = _rnd.Next(pool.Count);
                input.Add(pool[r]);
                pool.RemoveAt(r);
            }

            TestOrderPreservationOnInsertStructs(input, tree);
        }

        [Test]
        public void BinaryTreeScapegoatInsert4()
        {
            var tree = new ScapegoatTree<int, int>();

            //Randomize the input order
            var pool = Enumerable.Range(1, 1000).ToList();
            var input = new List<int>();
            while (pool.Count > 0)
            {
                var r = _rnd.Next(pool.Count);
                input.Add(pool[r]);
                pool.RemoveAt(r);
            }

            TestOrderPreservationOnInsertStructs(input, tree);
        }

        [Test]
        public void BinaryTreeScapegoatInsert5()
        {
            for (var i = 1; i < 10; i++)
            {
                var tree = new ScapegoatTree<int, int>();
                TestOrderPreservationOnInsertStructs(Enumerable.Range(1, i), tree);
            }
        }

        [Test]
        public void BinaryTreeScapegoatInsert6()
        {
            var input = new List<int>() { 790, 73, 443, 954, 1, 551, 592, 738, 621, 395, 775, 730, 787, 744, 887, 614, 325, 26, 607, 423, 97, 57, 521, 261, 943, 598, 259, 682, 686, 233, 18, 10, 632, 492, 583, 893, 969, 700, 823, 93, 314, 516, 230, 870, 65, 797, 609, 405, 17, 279, 468, 283, 600, 78, 953, 735, 547, 182, 13, 400, 292, 911, 55, 386, 378, 768, 106, 295, 162, 996, 653, 631, 206, 519, 116, 575, 829, 659, 201, 151, 989, 855, 669, 409, 767, 737, 410, 556, 539, 854, 341, 791, 690, 419, 805, 403, 465, 848, 486, 297, 697, 317, 246, 24, 629, 845, 83, 86, 965, 373, 913, 898, 982, 866, 412, 437, 240, 645, 620, 644, 352, 536, 752, 303, 656, 868, 941, 784, 718, 703, 112, 980, 165, 903, 448, 667, 594, 641, 780, 925, 651, 320, 581, 163, 942, 438, 806, 824, 8, 354, 161, 822, 217, 844, 940, 152, 558, 944, 358, 759, 900, 285, 351, 252, 262, 929, 723, 81, 404, 739, 833, 582, 864, 691, 988, 33, 948, 567, 355, 202, 61, 120, 771, 348, 526, 276, 751, 312, 961, 289, 102, 652, 270, 20, 28, 360, 967, 330, 894, 14, 709, 715, 408, 256, 515, 554, 958, 510, 856, 908, 199, 985, 564, 154, 695, 777, 502, 4, 712, 983, 172, 399, 124, 117, 628, 174, 311, 842, 666, 612, 66, 599, 678, 346, 481, 331, 701, 498, 827, 973, 321, 224, 456, 50, 95, 121, 87, 128, 308, 236, 841, 962, 602, 912, 834, 301, 588, 290, 613, 353, 242, 111, 847, 225, 843, 577, 293, 218, 660, 369, 77, 7, 175, 746, 648, 935, 945, 74, 30, 513, 96, 195, 527, 435, 779, 107, 2, 766, 506, 190, 731, 414, 886, 472, 720, 216, 385, 231, 203, 493, 815, 861, 457, 946, 167, 835, 788, 867, 260, 896, 364, 523, 272, 103, 265, 56, 565, 420, 452, 743, 584, 520, 705, 473, 626, 664, 126, 692, 32, 434, 546, 714, 876, 512, 706, 227, 736, 576, 268, 939, 309, 138, 499, 610, 825, 198, 137, 794, 209, 665, 637, 974, 781, 447, 907, 287, 149 };

            var tree = new ScapegoatTree<int,int>();
            var count = 0;
            foreach (var i in input)
            {
                Assert.IsTrue(tree.Add(i, i), "Failed to insert key " + i);
                count++;
                Assert.IsTrue(tree.ContainsKey(i), "Failed to find newly inserted Key " + i);
                Assert.AreEqual(count, tree.Nodes.Count(), "Incorrect count after Insert of " + i);
            }

            TestOrderStructs(input.OrderBy(i => i, Comparer<int>.Default).ToList(), tree.Values.ToList());
        }

        [Test]
        public void BinaryTreeScapegoatDelete1()
        {
            var tree = new ScapegoatTree<int, int>();
            var inputs = Enumerable.Range(1, 100).ToList();
            TestOrderPreservationOnInsertStructs(inputs, tree);
            Assert.IsTrue(tree.Remove(inputs[0]));
            inputs.RemoveAt(0);
            TestOrderStructs(inputs, tree.Keys.ToList());
        }

        [Test]
        public void BinaryTreeScapegoatDelete2()
        {
            var tree = new ScapegoatTree<int, int>();
            var inputs = Enumerable.Range(1, 100).ToList();
            TestOrderPreservationOnInsertStructs(inputs, tree);
            Assert.IsTrue(tree.Remove(inputs[50]));
            inputs.RemoveAt(50);
            TestOrderStructs(inputs, tree.Keys.ToList());
        }

        [Test]
        public void BinaryTreeScapegoatDelete3()
        {
            for (var i = 0; i < 10; i++)
            {
                var tree = new ScapegoatTree<int, int>();
                TestOrderPreservationOnDeleteStructs(Enumerable.Range(1, 10), tree);
            }
        }

        [Test]
        public void BinaryTreeScapegoatDelete4()
        {
            for (var i = 0; i < 10; i++)
            {
                var tree = new ScapegoatTree<int, int>();
                TestOrderPreservationOnDeleteStructs(Enumerable.Range(1, 100), tree);
            }
        }

        [Test]
        public void BinaryTreeScapegoatDelete5()
        {
            for (var i = 0; i < 10; i++)
            {
                //Randomize the input order
                var pool = Enumerable.Range(1, 25).ToList();
                var input = new List<int>();
                while (pool.Count > 0)
                {
                    var r = _rnd.Next(pool.Count);
                    input.Add(pool[r]);
                    pool.RemoveAt(r);
                }

                var tree = new ScapegoatTree<int, int>();
                TestOrderPreservationOnDeleteStructs(input, tree);
            }
        }

        [Test]
        public void BinaryTreeScapegoatDelete6()
        {
            for (var i = 0; i < 10; i++)
            {
                //Randomize the input order
                var pool = Enumerable.Range(1, 1000).ToList();
                var input = new List<int>();
                while (pool.Count > 0)
                {
                    var r = _rnd.Next(pool.Count);
                    input.Add(pool[r]);
                    pool.RemoveAt(r);
                }

                var tree = new ScapegoatTree<int, int>();
                TestOrderPreservationOnDeleteStructs(input, tree);
            }
        }

        [Test]
        public void BinaryTreeScapegoatDelete7()
        {
            var input = new List<int>() { 19, 10, 20, 14, 16, 5, 2, 23, 9, 1, 8, 4, 15, 11, 24, 7, 21, 13, 6, 3, 22, 18, 12, 17, 25 };
            var tree = new ScapegoatTree<int, int>();
            foreach (var i in input)
            {
                tree.Add(i, i);
            }
            var deletes = new List<int>() { 3, 11, 25, 2, 15, 19 };
            var count = input.Count;
            foreach (var i in deletes)
            {
                Console.WriteLine("Removing Key " + i);
                Assert.IsTrue(tree.Remove(i), "Failed to remove Key " + i);
                input.Remove(i);
                count--;
                TestOrderStructs(input.OrderBy(k => k, Comparer<int>.Default).ToList(), tree.Keys.ToList());
                Assert.AreEqual(count, tree.Nodes.Count(), "Removal of Key " + i + " did not reduce node count as expected");
            }
        }

        [Test]
        public void BinaryTreeScapegoatDelete8()
        {
            var input = new List<int>() { 25,14,5,8,22,17,9,12,4,1,3,23,2,7,19,20,10,24,16,6,21,13,18,11,15 };
            var tree = new ScapegoatTree<int,int>();
            foreach (var i in input)
            {
                tree.Add(i, i);
            }
            var deletes = new List<int>() { 3, 14, 25 };
            var count = input.Count;
            foreach (var i in deletes)
            {
                Console.WriteLine("Removing Key " + i);
                Assert.IsTrue(tree.Remove(i), "Failed to remove Key " + i);
                input.Remove(i);
                count--;
                TestOrderStructs(input.OrderBy(k => k, Comparer<int>.Default).ToList(), tree.Keys.ToList());
                Assert.AreEqual(count, tree.Nodes.Count(), "Removal of Key " + i + " did not reduce node count as expected");
            }
        }

        [Test]
        public void BinaryTreeScapegoatDelete9()
        {
            var input = new List<int>() { 790, 73, 443, 954, 1, 551, 592, 738, 621, 395, 775, 730, 787, 744, 887, 614, 325, 26, 607, 423, 97, 57, 521, 261, 943, 598, 259, 682, 686, 233, 18, 10, 632, 492, 583, 893, 969, 700, 823, 93, 314, 516, 230, 870, 65, 797, 609, 405, 17, 279, 468, 283, 600, 78, 953, 735, 547, 182, 13, 400, 292, 911, 55, 386, 378, 768, 106, 295, 162, 996, 653, 631, 206, 519, 116, 575, 829, 659, 201, 151, 989, 855, 669, 409, 767, 737, 410, 556, 539, 854, 341, 791, 690, 419, 805, 403, 465, 848, 486, 297, 697, 317, 246, 24, 629, 845, 83, 86, 965, 373, 913, 898, 982, 866, 412, 437, 240, 645, 620, 644, 352, 536, 752, 303, 656, 868, 941, 784, 718, 703, 112, 980, 165, 903, 448, 667, 594, 641, 780, 925, 651, 320, 581, 163, 942, 438, 806, 824, 8, 354, 161, 822, 217, 844, 940, 152, 558, 944, 358, 759, 900, 285, 351, 252, 262, 929, 723, 81, 404, 739, 833, 582, 864, 691, 988, 33, 948, 567, 355, 202, 61, 120, 771, 348, 526, 276, 751, 312, 961, 289, 102, 652, 270, 20, 28, 360, 967, 330, 894, 14, 709, 715, 408, 256, 515, 554, 958, 510, 856, 908, 199, 985, 564, 154, 695, 777, 502, 4, 712, 983, 172, 399, 124, 117, 628, 174, 311, 842, 666, 612, 66, 599, 678, 346, 481, 331, 701, 498, 827, 973, 321, 224, 456, 50, 95, 121, 87, 128, 308, 236, 841, 962, 602, 912, 834, 301, 588, 290, 613, 353, 242, 111, 847, 225, 843, 577, 293, 218, 660, 369, 77, 7, 175, 746, 648, 935, 945, 74, 30, 513, 96, 195, 527, 435, 779, 107, 2, 766, 506, 190, 731, 414, 886, 472, 720, 216, 385, 231, 203, 493, 815, 861, 457, 946, 167, 835, 788, 867, 260, 896, 364, 523, 272, 103, 265, 56, 565, 420, 452, 743, 584, 520, 705, 473, 626, 664, 126, 692, 32, 434, 546, 714, 876, 512, 706, 227, 736, 576, 268, 939, 309, 138, 499, 610, 825, 198, 137, 794, 209, 665, 637, 974, 781, 447, 907, 287, 149, 986, 316, 484, 719, 94, 839, 819, 340, 635, 649, 750, 384, 39, 318, 990, 144, 495, 432, 451, 22, 91, 640, 426, 528, 328, 477, 357, 796, 573, 932, 415, 606, 411, 79, 758, 269, 776, 214, 210, 288, 683, 267, 179, 540, 381, 396, 173, 350, 662, 832, 264, 21, 453, 271, 60, 336, 890, 964, 393, 605, 483, 38, 762, 713, 235, 135, 976, 956, 555, 710, 371, 281, 215, 158, 335, 817, 469, 885, 146, 924, 307, 6, 133, 763, 668, 407, 168, 142, 27, 433, 397, 816, 837, 601, 812, 654, 518, 476, 304, 244, 439, 219, 800, 952, 464, 992, 799, 282, 937, 239, 874, 3, 811, 471, 302, 487, 904, 1000, 49, 849, 968, 830, 930, 134, 323, 550, 460, 615, 359, 676, 291, 995, 570, 899, 497, 84, 200, 177, 299, 677, 603, 881, 189, 769, 552, 389, 5, 366, 300, 337, 753, 525, 430, 467, 273, 593, 274, 655, 636, 906, 531, 164, 927, 249, 949, 875, 425, 910, 971, 196, 248, 721, 696, 372, 212, 76, 625, 150, 572, 920, 548, 90, 322, 34, 919, 773, 562, 110, 617, 895, 801, 557, 537, 392, 139, 440, 947, 928, 814, 684, 972, 85, 387, 80, 62, 41, 813, 54, 223, 188, 966, 119, 70, 708, 362, 442, 859, 902, 344, 170, 313, 315, 294, 67, 428, 702, 171, 319, 213, 793, 132, 694, 363, 884, 957, 478, 707, 222, 955, 880, 950, 511, 871, 427, 922, 674, 494, 820, 45, 717, 566, 42, 587, 642, 382, 657, 785, 332, 670, 370, 852, 514, 826, 444, 59, 253, 761, 166, 745, 879, 724, 619, 959, 450, 204, 782, 424, 869, 125, 726, 192, 342, 243, 501, 207, 99, 100, 681, 509, 284, 658, 388, 549, 585, 36, 446, 673, 455, 361, 661, 901, 306, 72, 147, 997, 931, 75, 141, 441, 579, 226, 915, 183, 933, 145, 529, 251, 343, 194, 975, 159, 71, 663, 475, 220, 367, 68, 380, 436, 783, 115, 987, 535, 488, 818, 883, 254, 101, 530, 280, 19, 938, 258, 917, 52, 747, 185, 622, 463, 534, 176, 368, 234, 417, 517, 809, 650, 630, 921, 647, 857, 728, 740, 638, 480, 241, 406, 733, 109, 247, 255, 148, 821, 916, 936, 205, 804, 732, 413, 491, 136, 482, 905, 878, 604, 770, 748, 118, 981, 9, 238, 624, 155, 850, 431, 310, 197, 611, 489, 798, 374, 485, 522, 169, 257, 16, 542, 329, 466, 278, 11, 909, 221, 496, 533, 422, 795, 836, 37, 693, 538, 266, 755, 104, 15, 633, 999, 977, 541, 671, 12, 184, 286, 591, 114, 704, 963, 742, 643, 595, 275, 461, 699, 722, 23, 347, 421, 725, 379, 35, 532, 398, 596, 792, 333, 88, 741, 402, 160, 646, 40, 569, 810, 808, 277, 831, 679, 639, 356, 838, 470, 58, 180, 462, 449, 53, 589, 51, 122, 716, 211, 327, 623, 543, 140, 675, 627, 508, 298, 46, 991, 586, 578, 29, 608, 765, 689, 891, 458, 187, 734, 803, 383, 571, 580, 459, 178, 131, 377, 250, 559, 749, 305, 544, 445, 786, 970, 334, 727, 711, 181, 143, 391, 338, 872, 892, 545, 156, 882, 772, 429, 757, 680, 926, 25, 490, 951, 48, 923, 764, 454, 500, 108, 44, 561, 326, 296, 69, 672, 877, 846, 365, 228, 853, 998, 553, 401, 82, 802, 760, 960, 31, 324, 698, 865, 862, 64, 376, 524, 984, 851, 897, 390, 978, 186, 774, 63, 345, 807, 507, 597, 474, 590, 994, 918, 98, 418, 858, 889, 503, 89, 860, 237, 191, 756, 394, 129, 504, 130, 688, 229, 914, 574, 618, 92, 863, 979, 232, 563, 193, 479, 47, 245, 157, 123, 568, 505, 339, 789, 828, 375, 153, 616, 43, 934, 888, 687, 127, 208, 778, 685, 634, 113, 105, 416, 840, 993, 873, 754, 263, 349, 560, 729 };

            var tree = new ScapegoatTree<int, int>();
            foreach (var i in input)
            {
                Assert.IsTrue(tree.Add(i, i), "Failed to insert " + i);
            }

            var current = 0;
            foreach (var i in tree.Keys)
            {
                if (Math.Abs(i - current) > 1) Assert.Fail("Missing value between " + current + " and " + i);
                current = i;
            }

            TestOrderStructs(input.OrderBy(k => k, Comparer<int>.Default).ToList(), tree.Keys.ToList());
            var deletes = new List<int>() { 10 };
            var count = input.Count;
            foreach (var i in deletes)
            {
                Console.WriteLine("Removing Key " + i);
                Assert.IsTrue(tree.Remove(i), "Failed to remove Key " + i);
                input.Remove(i);
                count--;
                TestOrderStructs(input.OrderBy(k => k, Comparer<int>.Default).ToList(), tree.Keys.ToList());
                Assert.AreEqual(count, tree.Nodes.Count(), "Removal of Key " + i + " did not reduce node count as expected");
            }
        }

        [Test]
        public void BinaryTreeScapegoatIndexAccess1()
        {
            var tree = new ScapegoatTree<int, int>();
            var inputs = Enumerable.Range(1, 10).ToList();
            TestOrderPreservationOnInsertStructs(inputs, tree);

            for (var i = 0; i < 10; i++)
            {
                Assert.AreEqual(inputs[i], tree.GetValueAt(i));
            }

            // Swap the values using index access
            for (int i = 0, j = 9; i < j; i++, j--)
            {
                var temp = tree.GetValueAt(i);
                tree.SetValueAt(i, tree.GetValueAt(j));
                tree.SetValueAt(j, temp);
            }

            TestTools.PrintEnumerableStruct(tree.Values, ",");
            for (var i = 0; i < 10; i++)
            {
                Assert.AreEqual(10 - i, tree.GetValueAt(i));
            }
        }

        [Test]
        public void BinaryTreeScapegoatIndexAccess2()
        {
            var tree = new ScapegoatTree<int, int>();
            var inputs = Enumerable.Range(1, 10).ToList();
            TestOrderPreservationOnInsertStructs(inputs, tree);

            for (var i = 0; i < 10; i++)
            {
                Assert.AreEqual(inputs[i], tree.GetValueAt(i));
            }

            // Remove values in random order
            while (tree.Root != null)
            {
                var index = _rnd.Next(tree.Root.Size);
                tree.RemoveAt(index);
                inputs.RemoveAt(index);
                TestOrderStructs(inputs, tree.Keys.ToList());
            }
        }

        #endregion

        #region AVL Tree

        [Test]
        public void BinaryTreeAVLInsert1()
        {
            var tree = new AVLTree<int, int>();
            TestOrderPreservationOnInsertStructs(Enumerable.Range(1, 10), tree);
        }

        [Test]
        public void BinaryTreeAVLInsert2()
        {
            var tree = new AVLTree<int, int>();
            TestOrderPreservationOnInsertStructs(Enumerable.Range(1, 100), tree);
        }

        [Test]
        public void BinaryTreeAVLInsert3()
        {
            var tree = new AVLTree<int, int>();

            //Randomize the input order
            var pool = Enumerable.Range(1, 100).ToList();
            var input = new List<int>();
            while (pool.Count > 0)
            {
                var r = _rnd.Next(pool.Count);
                input.Add(pool[r]);
                pool.RemoveAt(r);
            }

            TestOrderPreservationOnInsertStructs(input, tree);
        }

        [Test]
        public void BinaryTreeAVLInsert4()
        {
            var tree = new AVLTree<int, int>();

            //Randomize the input order
            var pool = Enumerable.Range(1, 1000).ToList();
            var input = new List<int>();
            while (pool.Count > 0)
            {
                var r = _rnd.Next(pool.Count);
                input.Add(pool[r]);
                pool.RemoveAt(r);
            }

            TestOrderPreservationOnInsertStructs(input, tree);
        }

        [Test]
        public void BinaryTreeAVLInsert5()
        {
            for (var i = 1; i < 10; i++)
            {
                var tree = new AVLTree<int, int>();
                TestOrderPreservationOnInsertStructs(Enumerable.Range(1, i), tree);
            }
        }

        [Test]
        public void BinaryTreeAVLDelete1()
        {
            var tree = new AVLTree<int, int>();
            var inputs = Enumerable.Range(1, 100).ToList();
            TestOrderPreservationOnInsertStructs(inputs, tree);
            Assert.IsTrue(tree.Remove(inputs[0]));
            inputs.RemoveAt(0);
            TestOrderStructs(inputs, tree.Keys.ToList());
        }

        [Test]
        public void BinaryTreeAVLDelete2()
        {
            var tree = new AVLTree<int, int>();
            var inputs = Enumerable.Range(1, 100).ToList();
            TestOrderPreservationOnInsertStructs(inputs, tree);
            Assert.IsTrue(tree.Remove(inputs[50]));
            inputs.RemoveAt(50);
            TestOrderStructs(inputs, tree.Keys.ToList());
        }

        [Test]
        public void BinaryTreeAVLDelete3()
        {
            for (var i = 0; i < 10; i++)
            {
                var tree = new AVLTree<int, int>();
                TestOrderPreservationOnDeleteStructs(Enumerable.Range(1, 10), tree);
            }
        }

        [Test]
        public void BinaryTreeAVLDelete4()
        {
            for (var i = 0; i < 10; i++)
            {
                var tree = new AVLTree<int, int>();
                TestOrderPreservationOnDeleteStructs(Enumerable.Range(1, 100), tree);
            }
        }

        [Test]
        public void BinaryTreeAVLDelete5()
        {
            for (var i = 0; i < 10; i++)
            {
                //Randomize the input order
                var pool = Enumerable.Range(1, 25).ToList();
                var input = new List<int>();
                while (pool.Count > 0)
                {
                    var r = _rnd.Next(pool.Count);
                    input.Add(pool[r]);
                    pool.RemoveAt(r);
                }

                var tree = new AVLTree<int, int>();
                TestOrderPreservationOnDeleteStructs(input, tree);
            }
        }

        [Test]
        public void BinaryTreeAVLDelete6()
        {
            for (var i = 0; i < 10; i++)
            {
                //Randomize the input order
                var pool = Enumerable.Range(1, 1000).ToList();
                var input = new List<int>();
                while (pool.Count > 0)
                {
                    var r = _rnd.Next(pool.Count);
                    input.Add(pool[r]);
                    pool.RemoveAt(r);
                }

                var tree = new AVLTree<int, int>();
                TestOrderPreservationOnDeleteStructs(input, tree);
            }
        }

        [Test]
        public void BinaryTreeAVLDelete7()
        {
            var input = new List<int>() { 19, 10, 20, 14, 16, 5, 2, 23, 9, 1, 8, 4, 15, 11, 24, 7, 21, 13, 6, 3, 22, 18, 12, 17, 25 };
            var tree = new AVLTree<int, int>();
            foreach (var i in input)
            {
                tree.Add(i, i);
            }
            var deletes = new List<int>() { 3, 11, 25, 2, 15, 19 };
            var count = input.Count;
            foreach (var i in deletes)
            {
                Console.WriteLine("Removing Key " + i);
                Assert.IsTrue(tree.Remove(i), "Failed to remove Key " + i);
                input.Remove(i);
                count--;
                TestOrderStructs(input.OrderBy(k => k, Comparer<int>.Default).ToList(), tree.Keys.ToList());
                Assert.AreEqual(count, tree.Nodes.Count(), "Removal of Key " + i + " did not reduce node count as expected");
            }
        }

        [Test]
        public void BinaryTreeAVLDelete8()
        {
            var input = new List<int>() { 25, 14, 5, 8, 22, 17, 9, 12, 4, 1, 3, 23, 2, 7, 19, 20, 10, 24, 16, 6, 21, 13, 18, 11, 15 };
            var tree = new AVLTree<int, int>();
            foreach (var i in input)
            {
                tree.Add(i, i);
            }
            var deletes = new List<int>() { 3, 14, 25 };
            var count = input.Count;
            foreach (var i in deletes)
            {
                Console.WriteLine("Removing Key " + i);
                Assert.IsTrue(tree.Remove(i), "Failed to remove Key " + i);
                input.Remove(i);
                count--;
                TestOrderStructs(input.OrderBy(k => k, Comparer<int>.Default).ToList(), tree.Keys.ToList());
                Assert.AreEqual(count, tree.Nodes.Count(), "Removal of Key " + i + " did not reduce node count as expected");
            }
        }

        [Test]
        public void BinaryTreeAVLIndexAccess1()
        {
            var tree = new AVLTree<int, int>();
            var inputs = Enumerable.Range(1, 10).ToList();
            TestOrderPreservationOnInsertStructs(inputs, tree);

            for (var i = 0; i < 10; i++)
            {
                Assert.AreEqual(inputs[i], tree.GetValueAt(i));
            }

            // Swap the values using index access
            for (int i = 0, j = 9; i < j; i++, j--)
            {
                var temp = tree.GetValueAt(i);
                tree.SetValueAt(i, tree.GetValueAt(j));
                tree.SetValueAt(j, temp);
            }

            TestTools.PrintEnumerableStruct(tree.Values, ",");
            for (var i = 0; i < 10; i++)
            {
                Assert.AreEqual(10 - i, tree.GetValueAt(i));
            }
        }

        [Test]
        public void BinaryTreeAVLIndexAccess2()
        {
            var tree = new AVLTree<int, int>();
            var inputs = Enumerable.Range(1, 10).ToList();
            TestOrderPreservationOnInsertStructs(inputs, tree);

            for (var i = 0; i < 10; i++)
            {
                Assert.AreEqual(inputs[i], tree.GetValueAt(i));
            }

            // Remove values in random order
            while (tree.Root != null)
            {
                var index = _rnd.Next(tree.Root.Size);
                tree.RemoveAt(index);
                inputs.RemoveAt(index);
                TestOrderStructs(inputs, tree.Keys.ToList());
            }
        }

        #endregion
    }
}
