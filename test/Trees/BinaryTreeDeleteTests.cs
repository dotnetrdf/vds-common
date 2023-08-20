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

using System.Linq;
using NUnit.Framework;

namespace VDS.Common.Trees
{
    [TestFixture,Category("Trees")]
    public class BinaryTreeDeleteTests
    {
        #region Tree Preparation

        private void PrepareTree1(ITree<IBinaryTreeNode<int, int>, int, int> tree)
        {
            //Tree with just a root
            //
            //     (1)
            var root = new BinaryTreeNode<int, int>(null, 1, 1);

            tree.Root = root;

            BinaryTreeTools.PrintBinaryTreeStructs(tree);
        }

        private void PrepareTree2(ITree<IBinaryTreeNode<int, int>, int, int> tree)
        {
            //Tree with a root and a left child
            //
            //     (2)
            //    /
            //  (1)
            var root = new BinaryTreeNode<int, int>(null, 2, 2);
            var leftChild = new BinaryTreeNode<int, int>(null, 1, 1);
            root.LeftChild = leftChild;

            tree.Root = root;

            BinaryTreeTools.PrintBinaryTreeStructs(tree);
        }

        private void PrepareTree3(ITree<IBinaryTreeNode<int, int>, int, int> tree)
        {
            //Tree with a root and a left child
            //
            //     (2)
            //    /
            //  (1)
            var root = new BinaryTreeNode<int, int>(null, 1, 1);
            var rightChild = new BinaryTreeNode<int, int>(null, 2, 2);
            root.RightChild = rightChild;

            tree.Root = root;

            BinaryTreeTools.PrintBinaryTreeStructs(tree);
        }

        private void PrepareTree4(ITree<IBinaryTreeNode<int, int>, int, int> tree)
        {
            //Tree with a root and two children
            //
            //     (2)
            //    /   \
            //  (1)    (3)
            var root = new BinaryTreeNode<int, int>(null, 2, 2);
            var leftChild = new BinaryTreeNode<int, int>(null, 1, 1);
            var rightChild = new BinaryTreeNode<int, int>(null, 3, 3);
            root.LeftChild = leftChild;
            root.RightChild = rightChild;

            tree.Root = root;

            BinaryTreeTools.PrintBinaryTreeStructs(tree);
        }

        private void PrepareTree5(ITree<IBinaryTreeNode<int, int>, int, int> tree)
        {
            //Tree with a root and two left children
            //
            //     (3)
            //    /   
            //  (2)
            //  /
            //(1)
            var root = new BinaryTreeNode<int, int>(null, 3, 3);
            var leftInnerChild = new BinaryTreeNode<int, int>(null, 2, 2);
            var leftLeafChild = new BinaryTreeNode<int, int>(null, 1, 1);
            leftInnerChild.LeftChild = leftLeafChild;
            root.LeftChild = leftInnerChild;

            tree.Root = root;

            BinaryTreeTools.PrintBinaryTreeStructs(tree);
        }

        private void PrepareTree6(ITree<IBinaryTreeNode<int, int>, int, int> tree)
        {
            //Tree with a root and a left child which has a right child
            //
            //     (3)
            //    /   
            //  (1)
            //     \
            //     (2)
            var root = new BinaryTreeNode<int, int>(null, 3, 3);
            var leftChild = new BinaryTreeNode<int, int>(null, 1, 1);
            var rightChild = new BinaryTreeNode<int, int>(null, 2, 2);
            leftChild.RightChild = rightChild;
            root.LeftChild = leftChild;

            tree.Root = root;

            BinaryTreeTools.PrintBinaryTreeStructs(tree);
        }

        private void PrepareTree7(ITree<IBinaryTreeNode<int, int>, int, int> tree)
        {
            //Tree with a root and a right child which has a right child
            //
            //     (1)
            //       \   
            //       (2)
            //         \
            //         (3)
            var root = new BinaryTreeNode<int, int>(null, 1, 1);
            var rightInnerChild = new BinaryTreeNode<int, int>(null, 2, 2);
            var rightLeafChild = new BinaryTreeNode<int, int>(null, 3, 3);
            rightInnerChild.RightChild = rightLeafChild;
            root.RightChild = rightInnerChild;

            tree.Root = root;

            BinaryTreeTools.PrintBinaryTreeStructs(tree);
        }

        private void PrepareTree8(ITree<IBinaryTreeNode<int, int>, int, int> tree)
        {
            //Tree with a root and a right child which has a left child
            //
            //     (1)
            //       \   
            //       (3)
            //       /
            //     (2)
            var root = new BinaryTreeNode<int, int>(null, 1, 1);
            var rightChild = new BinaryTreeNode<int, int>(null, 3, 3);
            var leftChild = new BinaryTreeNode<int, int>(null, 2, 2);
            rightChild.LeftChild = leftChild;
            root.RightChild = rightChild;

            tree.Root = root;

            BinaryTreeTools.PrintBinaryTreeStructs(tree);
        }

        private void PrepareTree9(ITree<IBinaryTreeNode<int, int>, int, int> tree)
        {
            //Tree with a root and two children, left child has a left child
            //
            //     (3)
            //    /   \
            //  (2)    (4)
            //  /
            //(1)
            var root = new BinaryTreeNode<int, int>(null, 3, 3);
            var leftChild = new BinaryTreeNode<int, int>(null, 2, 2);
            var leftLeafChild = new BinaryTreeNode<int, int>(null, 1, 1);
            var rightChild = new BinaryTreeNode<int, int>(null, 4, 4);
            leftChild.LeftChild = leftLeafChild;
            root.LeftChild = leftChild;
            root.RightChild = rightChild;

            tree.Root = root;

            BinaryTreeTools.PrintBinaryTreeStructs(tree);
        }

        private void PrepareTree10(ITree<IBinaryTreeNode<int, int>, int, int> tree)
        {
            //Tree with a root and two children, left child has a right child
            //
            //     (3)
            //    /   \
            //  (1)    (4)
            //    \
            //    (2)
            var root = new BinaryTreeNode<int, int>(null, 3, 3);
            var leftChild = new BinaryTreeNode<int, int>(null, 1, 1);
            var rightLeafChild = new BinaryTreeNode<int, int>(null, 2, 2);
            var rightChild = new BinaryTreeNode<int, int>(null, 4, 4);
            leftChild.RightChild = rightLeafChild;
            root.LeftChild = leftChild;
            root.RightChild = rightChild;

            tree.Root = root;

            BinaryTreeTools.PrintBinaryTreeStructs(tree);
        }

        #endregion

        #region Tree Test Methods

        private void TestTree1(ITree<IBinaryTreeNode<int, int>, int, int> tree)
        {
            PrepareTree1(tree);

            //Remove Root
            tree.Remove(1);
            BinaryTreeTools.PrintBinaryTreeStructs(tree);

            //Should lead to empty tree
            Assert.AreEqual(0, tree.Nodes.Count());
            Assert.IsNull(tree.Root);
        }

        private void TestTree2a(ITree<IBinaryTreeNode<int, int>, int, int> tree)
        {
            PrepareTree2(tree);

            //Remove Root
            tree.Remove(2);
            BinaryTreeTools.PrintBinaryTreeStructs(tree);

            //Should lead to Left Child as Root
            Assert.AreEqual(1, tree.Nodes.Count());
            Assert.AreEqual(1, tree.Root.Value);
            Assert.IsNull(tree.Root.LeftChild);
        }

        private void TestTree2b(ITree<IBinaryTreeNode<int, int>, int, int> tree)
        {
            PrepareTree2(tree);

            //Remove Left Child
            tree.Remove(1);
            BinaryTreeTools.PrintBinaryTreeStructs(tree);

            //Should just leave Root
            Assert.AreEqual(1, tree.Nodes.Count());
            Assert.AreEqual(2, tree.Root.Value);
            Assert.IsNull(tree.Root.LeftChild);
        }

        private void TestTree3a(ITree<IBinaryTreeNode<int, int>, int, int> tree)
        {
            PrepareTree3(tree);

            //Remove Root
            tree.Remove(1);
            BinaryTreeTools.PrintBinaryTreeStructs(tree);

            //Should lead to Right Child as Root
            Assert.AreEqual(1, tree.Nodes.Count());
            Assert.AreEqual(2, tree.Root.Value);
            Assert.IsNull(tree.Root.RightChild);
        }

        private void TestTree3b(ITree<IBinaryTreeNode<int, int>, int, int> tree)
        {
            PrepareTree3(tree);

            //Remove Right Child
            tree.Remove(2);
            BinaryTreeTools.PrintBinaryTreeStructs(tree);

            //Should just leave Root
            Assert.AreEqual(1, tree.Nodes.Count());
            Assert.AreEqual(1, tree.Root.Value);
            Assert.IsNull(tree.Root.RightChild);
        }

        private void TestTree4a(ITree<IBinaryTreeNode<int, int>, int, int> tree)
        {
            PrepareTree4(tree);

            //Remove Root
            tree.Remove(2);
            BinaryTreeTools.PrintBinaryTreeStructs(tree);

            //Should lead to Left Child to Root with Right Child as-is
            Assert.AreEqual(2, tree.Nodes.Count());
            Assert.AreEqual(1, tree.Root.Value);
            Assert.IsNotNull(tree.Root.RightChild);
            Assert.AreEqual(3, tree.Root.RightChild.Value);
        }

        private void TestTree4b(ITree<IBinaryTreeNode<int, int>, int, int> tree)
        {
            PrepareTree4(tree);

            //Remove Left Child
            tree.Remove(1);
            BinaryTreeTools.PrintBinaryTreeStructs(tree);

            //Should leave Root and Right Child as-is
            Assert.AreEqual(2, tree.Nodes.Count());
            Assert.AreEqual(2, tree.Root.Value);
            Assert.IsNotNull(tree.Root.RightChild);
            Assert.AreEqual(3, tree.Root.RightChild.Value);
        }

        private void TestTree4c(ITree<IBinaryTreeNode<int, int>, int, int> tree)
        {
            PrepareTree4(tree);

            //Remove Right Child
            tree.Remove(3);
            BinaryTreeTools.PrintBinaryTreeStructs(tree);

            //Should leave Root and Left Child as-is
            Assert.AreEqual(2, tree.Nodes.Count());
            Assert.AreEqual(2, tree.Root.Value);
            Assert.IsNotNull(tree.Root.LeftChild);
            Assert.AreEqual(1, tree.Root.LeftChild.Value);
        }

        private void TestTree5a(ITree<IBinaryTreeNode<int, int>, int, int> tree)
        {
            PrepareTree5(tree);

            //Remove Root
            tree.Remove(3);
            BinaryTreeTools.PrintBinaryTreeStructs(tree);

            //Should leave Left Child to Root
            Assert.AreEqual(2, tree.Nodes.Count());
            Assert.AreEqual(2, tree.Root.Value);
            Assert.IsNotNull(tree.Root.LeftChild);
            Assert.AreEqual(1, tree.Root.LeftChild.Value);
        }

        private void TestTree5b(ITree<IBinaryTreeNode<int, int>, int, int> tree)
        {
            PrepareTree5(tree);

            //Remove Left Inner Child
            tree.Remove(2);
            BinaryTreeTools.PrintBinaryTreeStructs(tree);

            //Should leave Leaf as Left Child of Root
            Assert.AreEqual(2, tree.Nodes.Count());
            Assert.AreEqual(3, tree.Root.Value);
            Assert.IsNotNull(tree.Root.LeftChild);
            Assert.AreEqual(1, tree.Root.LeftChild.Value);
        }

        private void TestTree5c(ITree<IBinaryTreeNode<int, int>, int, int> tree)
        {
            PrepareTree5(tree);

            //Remove Left Leaf Child
            tree.Remove(1);
            BinaryTreeTools.PrintBinaryTreeStructs(tree);

            //Should leave Root and Left Inner Child as-is
            Assert.AreEqual(2, tree.Nodes.Count());
            Assert.AreEqual(3, tree.Root.Value);
            Assert.IsNotNull(tree.Root.LeftChild);
            Assert.AreEqual(2, tree.Root.LeftChild.Value);
        }

        private void TestTree6a(ITree<IBinaryTreeNode<int, int>, int, int> tree)
        {
            PrepareTree6(tree);

            //Remove Root
            tree.Remove(3);
            BinaryTreeTools.PrintBinaryTreeStructs(tree);

            //Should move right child of Left child to Root
            Assert.AreEqual(2, tree.Nodes.Count());
            Assert.AreEqual(2, tree.Root.Value);
            Assert.IsNotNull(tree.Root.LeftChild);
            Assert.AreEqual(1, tree.Root.LeftChild.Value);
        }

        private void TestTree6b(ITree<IBinaryTreeNode<int, int>, int, int> tree)
        {
            PrepareTree6(tree);

            //Remove Left Child
            tree.Remove(1);
            BinaryTreeTools.PrintBinaryTreeStructs(tree);

            //Should move right child of left child to left child
            Assert.AreEqual(2, tree.Nodes.Count());
            Assert.AreEqual(3, tree.Root.Value);
            Assert.IsNotNull(tree.Root.LeftChild);
            Assert.AreEqual(2, tree.Root.LeftChild.Value);
        }

        private void TestTree6c(ITree<IBinaryTreeNode<int, int>, int, int> tree)
        {
            PrepareTree6(tree);

            //Remove Right Child
            tree.Remove(2);
            BinaryTreeTools.PrintBinaryTreeStructs(tree);

            //Should leave Root and Left Child as-is
            Assert.AreEqual(2, tree.Nodes.Count());
            Assert.AreEqual(3, tree.Root.Value);
            Assert.IsNotNull(tree.Root.LeftChild);
            Assert.AreEqual(1, tree.Root.LeftChild.Value);
        }

        private void TestTree7a(ITree<IBinaryTreeNode<int, int>, int, int> tree)
        {
            PrepareTree7(tree);

            //Remove Root
            tree.Remove(1);
            BinaryTreeTools.PrintBinaryTreeStructs(tree);

            //Should move Right Child to Root
            Assert.AreEqual(2, tree.Nodes.Count());
            Assert.AreEqual(2, tree.Root.Value);
            Assert.IsNotNull(tree.Root.RightChild);
            Assert.AreEqual(3, tree.Root.RightChild.Value);
        }

        private void TestTree7b(ITree<IBinaryTreeNode<int, int>, int, int> tree)
        {
            PrepareTree7(tree);

            //Remove Right Inner Child
            tree.Remove(2);
            BinaryTreeTools.PrintBinaryTreeStructs(tree);

            //Should leave Leaf as Right Child of Root
            Assert.AreEqual(2, tree.Nodes.Count());
            Assert.AreEqual(1, tree.Root.Value);
            Assert.IsNotNull(tree.Root.RightChild);
            Assert.AreEqual(3, tree.Root.RightChild.Value);
        }

        private void TestTree7c(ITree<IBinaryTreeNode<int, int>, int, int> tree)
        {
            PrepareTree7(tree);

            //Remove Right Leaf Child
            tree.Remove(3);
            BinaryTreeTools.PrintBinaryTreeStructs(tree);

            //Should leave Root and Left Inner Child as-is
            Assert.AreEqual(2, tree.Nodes.Count());
            Assert.AreEqual(1, tree.Root.Value);
            Assert.IsNotNull(tree.Root.RightChild);
            Assert.AreEqual(2, tree.Root.RightChild.Value);
        }

        private void TestTree8a(ITree<IBinaryTreeNode<int, int>, int, int> tree)
        {
            PrepareTree8(tree);

            //Remove Root
            tree.Remove(1);
            BinaryTreeTools.PrintBinaryTreeStructs(tree);

            //Should move left child of right child to Root
            Assert.AreEqual(2, tree.Nodes.Count());
            Assert.AreEqual(2, tree.Root.Value);
            Assert.IsNotNull(tree.Root.RightChild);
            Assert.AreEqual(3, tree.Root.RightChild.Value);
        }

        private void TestTree8b(ITree<IBinaryTreeNode<int, int>, int, int> tree)
        {
            PrepareTree8(tree);

            //Remove Right Child
            tree.Remove(3);
            BinaryTreeTools.PrintBinaryTreeStructs(tree);

            //Should move left child of right child to right child
            Assert.AreEqual(2, tree.Nodes.Count());
            Assert.AreEqual(1, tree.Root.Value);
            Assert.IsNotNull(tree.Root.RightChild);
            Assert.AreEqual(2, tree.Root.RightChild.Value);
        }

        private void TestTree8c(ITree<IBinaryTreeNode<int, int>, int, int> tree)
        {
            PrepareTree8(tree);

            //Remove Left Child
            tree.Remove(2);
            BinaryTreeTools.PrintBinaryTreeStructs(tree);

            //Should leave Root and Right Child as-is
            Assert.AreEqual(2, tree.Nodes.Count());
            Assert.AreEqual(1, tree.Root.Value);
            Assert.IsNotNull(tree.Root.RightChild);
            Assert.AreEqual(3, tree.Root.RightChild.Value);
        }

        private void TestTree9a(ITree<IBinaryTreeNode<int, int>, int, int> tree)
        {
            PrepareTree9(tree);

            //Remove Root
            tree.Remove(3);
            BinaryTreeTools.PrintBinaryTreeStructs(tree);

            //Should lead to Left Child to Root with Right Child as-is
            Assert.AreEqual(3, tree.Nodes.Count());
            Assert.AreEqual(2, tree.Root.Value);
            Assert.IsNotNull(tree.Root.LeftChild);
            Assert.AreEqual(1, tree.Root.LeftChild.Value);
            Assert.IsNotNull(tree.Root.RightChild);
            Assert.AreEqual(4, tree.Root.RightChild.Value);
        }

        private void TestTree9b(ITree<IBinaryTreeNode<int, int>, int, int> tree)
        {
            PrepareTree9(tree);

            //Remove Left Inner Child
            tree.Remove(2);
            BinaryTreeTools.PrintBinaryTreeStructs(tree);

            //Should leave Root and Right Child as-is, Left Leaf child should move up
            Assert.AreEqual(3, tree.Nodes.Count());
            Assert.AreEqual(3, tree.Root.Value);
            Assert.IsNotNull(tree.Root.LeftChild);
            Assert.AreEqual(1, tree.Root.LeftChild.Value);
            Assert.IsNotNull(tree.Root.RightChild);
            Assert.AreEqual(4, tree.Root.RightChild.Value);
        }

        private void TestTree9c(ITree<IBinaryTreeNode<int, int>, int, int> tree)
        {
            PrepareTree9(tree);

            //Remove Left Leaf Child
            tree.Remove(1);
            BinaryTreeTools.PrintBinaryTreeStructs(tree);

            //Should leave Root and immediate children as-is
            Assert.AreEqual(3, tree.Nodes.Count());
            Assert.AreEqual(3, tree.Root.Value);
            Assert.IsNotNull(tree.Root.LeftChild);
            Assert.AreEqual(2, tree.Root.LeftChild.Value);
            Assert.IsNotNull(tree.Root.RightChild);
            Assert.AreEqual(4, tree.Root.RightChild.Value);
        }

        private void TestTree9d(ITree<IBinaryTreeNode<int, int>, int, int> tree)
        {
            PrepareTree9(tree);

            //Remove Right Child
            tree.Remove(4);
            BinaryTreeTools.PrintBinaryTreeStructs(tree);

            //Should leave Root and left sub-tree as-is
            Assert.AreEqual(3, tree.Nodes.Count());
            Assert.AreEqual(3, tree.Root.Value);
            Assert.IsNotNull(tree.Root.LeftChild);
            Assert.AreEqual(2, tree.Root.LeftChild.Value);
            Assert.IsNotNull(tree.Root.LeftChild.LeftChild);
            Assert.AreEqual(1, tree.Root.LeftChild.LeftChild.Value);
        }

        private void TestTree10a(ITree<IBinaryTreeNode<int, int>, int, int> tree)
        {
            PrepareTree10(tree);

            //Remove Root
            tree.Remove(3);
            BinaryTreeTools.PrintBinaryTreeStructs(tree);

            //Should lead to Rightmost Child of Left subtree to Root with Right Child as-is
            Assert.AreEqual(3, tree.Nodes.Count());
            Assert.AreEqual(2, tree.Root.Value);
            Assert.IsNotNull(tree.Root.LeftChild);
            Assert.AreEqual(1, tree.Root.LeftChild.Value);
            Assert.IsNotNull(tree.Root.RightChild);
            Assert.AreEqual(4, tree.Root.RightChild.Value);
        }

        private void TestTree10b(ITree<IBinaryTreeNode<int, int>, int, int> tree)
        {
            PrepareTree10(tree);

            //Remove Left Inner Child
            tree.Remove(1);
            BinaryTreeTools.PrintBinaryTreeStructs(tree);

            //Should leave Root and Right Child as-is, Right Leaf of left subtree should move up
            Assert.AreEqual(3, tree.Nodes.Count());
            Assert.AreEqual(3, tree.Root.Value);
            Assert.IsNotNull(tree.Root.LeftChild);
            Assert.AreEqual(2, tree.Root.LeftChild.Value);
            Assert.IsNotNull(tree.Root.RightChild);
            Assert.AreEqual(4, tree.Root.RightChild.Value);
        }

        private void TestTree10c(ITree<IBinaryTreeNode<int, int>, int, int> tree)
        {
            PrepareTree10(tree);

            //Remove Right Left of left subtree should leave root and immediate children as=is
            tree.Remove(2);
            BinaryTreeTools.PrintBinaryTreeStructs(tree);

            //Should leave Root and immediate children as-is
            Assert.AreEqual(3, tree.Nodes.Count());
            Assert.AreEqual(3, tree.Root.Value);
            Assert.IsNotNull(tree.Root.LeftChild);
            Assert.AreEqual(1, tree.Root.LeftChild.Value);
            Assert.IsNotNull(tree.Root.RightChild);
            Assert.AreEqual(4, tree.Root.RightChild.Value);
        }

        private void TestTree10d(ITree<IBinaryTreeNode<int, int>, int, int> tree)
        {
            PrepareTree10(tree);

            //Remove Right Child
            tree.Remove(4);
            BinaryTreeTools.PrintBinaryTreeStructs(tree);

            //Should leave Root and left sub-tree as-is
            Assert.AreEqual(3, tree.Nodes.Count());
            Assert.AreEqual(3, tree.Root.Value);
            Assert.IsNotNull(tree.Root.LeftChild);
            Assert.AreEqual(1, tree.Root.LeftChild.Value);
            Assert.IsNotNull(tree.Root.LeftChild.RightChild);
            Assert.AreEqual(2, tree.Root.LeftChild.RightChild.Value);
        }

        #endregion

        [Test]
        public void BinaryTreeAVLDeleteValidation1()
        {
            var tree = new AVLTree<int, int>();
            TestTree1(tree);
        }

        [Test]
        public void BinaryTreeUnbalancedDeleteValidation1()
        {
            var tree = new UnbalancedBinaryTree<int, int>();
            TestTree1(tree);
        }

        [Test]
        public void BinaryTreeScapegoatDeleteValidation1()
        {
            var tree = new ScapegoatTree<int, int>();
            TestTree1(tree);
        }

        [Test]
        public void BinaryTreeAVLDeleteValidation2a()
        {
            var tree = new AVLTree<int, int>();
            TestTree2a(tree);
        }

        [Test]
        public void BinaryTreeAVLDeleteValidation2b()
        {
            var tree = new AVLTree<int, int>();
            TestTree2b(tree);
        }

        [Test]
        public void BinaryTreeUnbalancedDeleteValidation2a()
        {
            var tree = new UnbalancedBinaryTree<int, int>();
            TestTree2a(tree);
        }

        [Test]
        public void BinaryTreeUnbalancedDeleteValidation2b()
        {
            var tree = new UnbalancedBinaryTree<int, int>();
            TestTree2b(tree);
        }

        [Test]
        public void BinaryTreeScapegoatDeleteValidation2a()
        {
            var tree = new ScapegoatTree<int, int>();
            TestTree2a(tree);
        }

        [Test]
        public void BinaryTreeScapegoatDeleteValidation2b()
        {
            var tree = new ScapegoatTree<int, int>();
            TestTree2b(tree);
        }

        [Test]
        public void BinaryTreeAVLDeleteValidation3a()
        {
            var tree = new AVLTree<int, int>();
            TestTree3a(tree);
        }

        [Test]
        public void BinaryTreeAVLDeleteValidation3b()
        {
            var tree = new AVLTree<int, int>();
            TestTree3b(tree);
        }

        [Test]
        public void BinaryTreeUnbalancedDeleteValidation3a()
        {
            var tree = new UnbalancedBinaryTree<int, int>();
            TestTree3a(tree);
        }

        [Test]
        public void BinaryTreeUnbalancedDeleteValidation3b()
        {
            var tree = new UnbalancedBinaryTree<int, int>();
            TestTree3b(tree);
        }

        [Test]
        public void BinaryTreeScapegoatDeleteValidation3a()
        {
            var tree = new ScapegoatTree<int, int>();
            TestTree3a(tree);
        }

        [Test]
        public void BinaryTreeScapegoatDeleteValidation3b()
        {
            var tree = new ScapegoatTree<int, int>();
            TestTree3b(tree);
        }

        [Test]
        public void BinaryTreeAVLDeleteValidation4a()
        {
            var tree = new AVLTree<int, int>();
            TestTree4a(tree);
        }

        [Test]
        public void BinaryTreeAVLDeleteValidation4b()
        {
            var tree = new AVLTree<int, int>();
            TestTree4b(tree);
        }

        [Test]
        public void BinaryTreeAVLDeleteValidation4c()
        {
            var tree = new AVLTree<int, int>();
            TestTree4c(tree);
        }

        [Test]
        public void BinaryTreeUnbalancedDeleteValidation4a()
        {
            var tree = new UnbalancedBinaryTree<int, int>();
            TestTree4a(tree);
        }

        [Test]
        public void BinaryTreeUnbalancedDeleteValidation4b()
        {
            var tree = new UnbalancedBinaryTree<int, int>();
            TestTree4b(tree);
        }

        [Test]
        public void BinaryTreeUnbalancedDeleteValidation4c()
        {
            var tree = new UnbalancedBinaryTree<int, int>();
            TestTree4c(tree);
        }

        //Tree 7 tests are not run for Scapegoat Tree because they trigger a rebalance which
        //means the tree will not be as we expect
        //NB - Tree 4c, 5 and 6 tests also trigger the rebalance but they always leave the tree in the state
        //we expect so they can be safely run

        [Test]
        public void BinaryTreeScapegoatDeleteValidation4c()
        {
            var tree = new ScapegoatTree<int, int>();
            TestTree4c(tree);
        }

        [Test]
        public void BinaryTreeAVLDeleteValidation5a()
        {
            var tree = new AVLTree<int, int>();
            TestTree5a(tree);
        }

        [Test]
        public void BinaryTreeAVLDeleteValidation5b()
        {
            var tree = new AVLTree<int, int>();
            TestTree5b(tree);
        }

        [Test]
        public void BinaryTreeAVLDeleteValidation5c()
        {
            var tree = new AVLTree<int, int>();
            TestTree5c(tree);
        }

        [Test]
        public void BinaryTreeUnbalancedDeleteValidation5a()
        {
            var tree = new UnbalancedBinaryTree<int, int>();
            TestTree5a(tree);
        }

        [Test]
        public void BinaryTreeUnbalancedDeleteValidation5b()
        {
            var tree = new UnbalancedBinaryTree<int, int>();
            TestTree5b(tree);
        }

        [Test]
        public void BinaryTreeUnbalancedDeleteValidation5c()
        {
            var tree = new UnbalancedBinaryTree<int, int>();
            TestTree5c(tree);
        }

        [Test]
        public void BinaryTreeScapegoatDeleteValidation5a()
        {
            var tree = new ScapegoatTree<int, int>();
            TestTree5a(tree);
        }

        [Test]
        public void BinaryTreeScapegoatDeleteValidation5b()
        {
            var tree = new ScapegoatTree<int, int>();
            TestTree5b(tree);
        }

        [Test]
        public void BinaryTreeScapegoatDeleteValidation5c()
        {
            var tree = new ScapegoatTree<int, int>();
            TestTree5c(tree);
        }

        [Test]
        public void BinaryTreeAVLDeleteValidation6a()
        {
            var tree = new AVLTree<int, int>();
            TestTree6a(tree);
        }

        [Test]
        public void BinaryTreeAVLDeleteValidation6b()
        {
            var tree = new AVLTree<int, int>();
            TestTree6b(tree);
        }

        [Test]
        public void BinaryTreeAVLDeleteValidation6c()
        {
            var tree = new AVLTree<int, int>();
            TestTree6c(tree);
        }

        [Test]
        public void BinaryTreeUnbalancedDeleteValidation6a()
        {
            var tree = new UnbalancedBinaryTree<int, int>();
            TestTree6a(tree);
        }

        [Test]
        public void BinaryTreeUnbalancedDeleteValidation6b()
        {
            var tree = new UnbalancedBinaryTree<int, int>();
            TestTree6b(tree);
        }

        [Test]
        public void BinaryTreeUnbalancedDeleteValidation6c()
        {
            var tree = new UnbalancedBinaryTree<int, int>();
            TestTree6c(tree);
        }

        [Test]
        public void BinaryTreeScapegoatDeleteValidation6a()
        {
            var tree = new ScapegoatTree<int, int>();
            TestTree6a(tree);
        }

        [Test]
        public void BinaryTreeScapegoatDeleteValidation6b()
        {
            var tree = new ScapegoatTree<int, int>();
            TestTree6b(tree);
        }

        [Test]
        public void BinaryTreeScapegoatDeleteValidation6c()
        {
            var tree = new ScapegoatTree<int, int>();
            TestTree6c(tree);
        }

        [Test]
        public void BinaryTreeAVLDeleteValidation7a()
        {
            var tree = new AVLTree<int, int>();
            TestTree7a(tree);
        }

        [Test]
        public void BinaryTreeAVLDeleteValidation7b()
        {
            var tree = new AVLTree<int, int>();
            TestTree7b(tree);
        }

        [Test]
        public void BinaryTreeAVLDeleteValidation7c()
        {
            var tree = new AVLTree<int, int>();
            TestTree7c(tree);
        }

        [Test]
        public void BinaryTreeUnbalancedDeleteValidation7a()
        {
            var tree = new UnbalancedBinaryTree<int, int>();
            TestTree7a(tree);
        }

        [Test]
        public void BinaryTreeUnbalancedDeleteValidation7b()
        {
            var tree = new UnbalancedBinaryTree<int, int>();
            TestTree7b(tree);
        }

        [Test]
        public void BinaryTreeUnbalancedDeleteValidation7c()
        {
            var tree = new UnbalancedBinaryTree<int, int>();
            TestTree7c(tree);
        }

        //Tree 7 tests are not run for Scapegoat Tree because they trigger a rebalance which
        //means the tree will not be as we expect
        //NB - Tree 4c, 5 and 6 tests also trigger the rebalance but they always leave the tree in the state
        //we expect so they can be safely run

        [Test]
        public void BinaryTreeAVLDeleteValidation8a()
        {
            var tree = new AVLTree<int, int>();
            TestTree8a(tree);
        }

        [Test]
        public void BinaryTreeAVLDeleteValidation8b()
        {
            var tree = new AVLTree<int, int>();
            TestTree8b(tree);
        }

        [Test]
        public void BinaryTreeAVLDeleteValidation8c()
        {
            var tree = new AVLTree<int, int>();
            TestTree8c(tree);
        }

        [Test]
        public void BinaryTreeUnbalancedDeleteValidation8a()
        {
            var tree = new UnbalancedBinaryTree<int, int>();
            TestTree8a(tree);
        }

        [Test]
        public void BinaryTreeUnbalancedDeleteValidation8b()
        {
            var tree = new UnbalancedBinaryTree<int, int>();
            TestTree8b(tree);
        }

        [Test]
        public void BinaryTreeUnbalancedDeleteValidation8c()
        {
            var tree = new UnbalancedBinaryTree<int, int>();
            TestTree8c(tree);
        }

        //Tree 8 tests are not run for Scapegoat Tree because they trigger a rebalance which
        //means the tree will not be as we expect
        //NB - Tree 4c, 5 and 6 tests also trigger the rebalance but they always leave the tree in the state
        //we expect so they can be safely run

        [Test]
        public void BinaryTreeUnbalancedDeleteValidation9a()
        {
            var tree = new UnbalancedBinaryTree<int, int>();
            TestTree9a(tree);
        }

        [Test]
        public void BinaryTreeUnbalancedDeleteValidation9b()
        {
            var tree = new UnbalancedBinaryTree<int, int>();
            TestTree9b(tree);
        }

        [Test]
        public void BinaryTreeUnbalancedDeleteValidation9c()
        {
            var tree = new UnbalancedBinaryTree<int, int>();
            TestTree9c(tree);
        }

        [Test]
        public void BinaryTreeUnbalancedDeleteValidation9d()
        {
            var tree = new UnbalancedBinaryTree<int, int>();
            TestTree9d(tree);
        }

        [Test]
        public void BinaryTreeUnbalancedDeleteValidation10a()
        {
            var tree = new UnbalancedBinaryTree<int, int>();
            TestTree10a(tree);
        }

        [Test]
        public void BinaryTreeUnbalancedDeleteValidation10b()
        {
            var tree = new UnbalancedBinaryTree<int, int>();
            TestTree10b(tree);
        }

        [Test]
        public void BinaryTreeUnbalancedDeleteValidation10c()
        {
            var tree = new UnbalancedBinaryTree<int, int>();
            TestTree10c(tree);
        }

        [Test]
        public void BinaryTreeUnbalancedDeleteValidation10d()
        {
            var tree = new UnbalancedBinaryTree<int, int>();
            TestTree10d(tree);
        }
    }
}
