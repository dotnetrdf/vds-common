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

using System.Linq;
using NUnit.Framework;

namespace VDS.Common.Trees;

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

    private void TestTree2A(ITree<IBinaryTreeNode<int, int>, int, int> tree)
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

    private void TestTree2B(ITree<IBinaryTreeNode<int, int>, int, int> tree)
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

    private void TestTree3A(ITree<IBinaryTreeNode<int, int>, int, int> tree)
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

    private void TestTree3B(ITree<IBinaryTreeNode<int, int>, int, int> tree)
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

    private void TestTree4A(ITree<IBinaryTreeNode<int, int>, int, int> tree)
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

    private void TestTree4B(ITree<IBinaryTreeNode<int, int>, int, int> tree)
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

    private void TestTree4C(ITree<IBinaryTreeNode<int, int>, int, int> tree)
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

    private void TestTree5A(ITree<IBinaryTreeNode<int, int>, int, int> tree)
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

    private void TestTree5B(ITree<IBinaryTreeNode<int, int>, int, int> tree)
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

    private void TestTree5C(ITree<IBinaryTreeNode<int, int>, int, int> tree)
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

    private void TestTree6A(ITree<IBinaryTreeNode<int, int>, int, int> tree)
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

    private void TestTree6B(ITree<IBinaryTreeNode<int, int>, int, int> tree)
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

    private void TestTree6C(ITree<IBinaryTreeNode<int, int>, int, int> tree)
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

    private void TestTree7A(ITree<IBinaryTreeNode<int, int>, int, int> tree)
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

    private void TestTree7B(ITree<IBinaryTreeNode<int, int>, int, int> tree)
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

    private void TestTree7C(ITree<IBinaryTreeNode<int, int>, int, int> tree)
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

    private void TestTree8A(ITree<IBinaryTreeNode<int, int>, int, int> tree)
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

    private void TestTree8B(ITree<IBinaryTreeNode<int, int>, int, int> tree)
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

    private void TestTree8C(ITree<IBinaryTreeNode<int, int>, int, int> tree)
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

    private void TestTree9A(ITree<IBinaryTreeNode<int, int>, int, int> tree)
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

    private void TestTree9B(ITree<IBinaryTreeNode<int, int>, int, int> tree)
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

    private void TestTree9C(ITree<IBinaryTreeNode<int, int>, int, int> tree)
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

    private void TestTree10A(ITree<IBinaryTreeNode<int, int>, int, int> tree)
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

    private void TestTree10B(ITree<IBinaryTreeNode<int, int>, int, int> tree)
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

    private void TestTree10C(ITree<IBinaryTreeNode<int, int>, int, int> tree)
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
    public void BinaryTreeAvlDeleteValidation1()
    {
        var tree = new AvlTree<int, int>();
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
    public void BinaryTreeAvlDeleteValidation2A()
    {
        var tree = new AvlTree<int, int>();
        TestTree2A(tree);
    }

    [Test]
    public void BinaryTreeAvlDeleteValidation2B()
    {
        var tree = new AvlTree<int, int>();
        TestTree2B(tree);
    }

    [Test]
    public void BinaryTreeUnbalancedDeleteValidation2A()
    {
        var tree = new UnbalancedBinaryTree<int, int>();
        TestTree2A(tree);
    }

    [Test]
    public void BinaryTreeUnbalancedDeleteValidation2B()
    {
        var tree = new UnbalancedBinaryTree<int, int>();
        TestTree2B(tree);
    }

    [Test]
    public void BinaryTreeScapegoatDeleteValidation2A()
    {
        var tree = new ScapegoatTree<int, int>();
        TestTree2A(tree);
    }

    [Test]
    public void BinaryTreeScapegoatDeleteValidation2B()
    {
        var tree = new ScapegoatTree<int, int>();
        TestTree2B(tree);
    }

    [Test]
    public void BinaryTreeAvlDeleteValidation3A()
    {
        var tree = new AvlTree<int, int>();
        TestTree3A(tree);
    }

    [Test]
    public void BinaryTreeAvlDeleteValidation3B()
    {
        var tree = new AvlTree<int, int>();
        TestTree3B(tree);
    }

    [Test]
    public void BinaryTreeUnbalancedDeleteValidation3A()
    {
        var tree = new UnbalancedBinaryTree<int, int>();
        TestTree3A(tree);
    }

    [Test]
    public void BinaryTreeUnbalancedDeleteValidation3B()
    {
        var tree = new UnbalancedBinaryTree<int, int>();
        TestTree3B(tree);
    }

    [Test]
    public void BinaryTreeScapegoatDeleteValidation3A()
    {
        var tree = new ScapegoatTree<int, int>();
        TestTree3A(tree);
    }

    [Test]
    public void BinaryTreeScapegoatDeleteValidation3B()
    {
        var tree = new ScapegoatTree<int, int>();
        TestTree3B(tree);
    }

    [Test]
    public void BinaryTreeAvlDeleteValidation4A()
    {
        var tree = new AvlTree<int, int>();
        TestTree4A(tree);
    }

    [Test]
    public void BinaryTreeAvlDeleteValidation4B()
    {
        var tree = new AvlTree<int, int>();
        TestTree4B(tree);
    }

    [Test]
    public void BinaryTreeAvlDeleteValidation4C()
    {
        var tree = new AvlTree<int, int>();
        TestTree4C(tree);
    }

    [Test]
    public void BinaryTreeUnbalancedDeleteValidation4A()
    {
        var tree = new UnbalancedBinaryTree<int, int>();
        TestTree4A(tree);
    }

    [Test]
    public void BinaryTreeUnbalancedDeleteValidation4B()
    {
        var tree = new UnbalancedBinaryTree<int, int>();
        TestTree4B(tree);
    }

    [Test]
    public void BinaryTreeUnbalancedDeleteValidation4C()
    {
        var tree = new UnbalancedBinaryTree<int, int>();
        TestTree4C(tree);
    }

    //Tree 7 tests are not run for Scapegoat Tree because they trigger a rebalance which
    //means the tree will not be as we expect
    //NB - Tree 4c, 5 and 6 tests also trigger the rebalance but they always leave the tree in the state
    //we expect so they can be safely run

    [Test]
    public void BinaryTreeScapegoatDeleteValidation4C()
    {
        var tree = new ScapegoatTree<int, int>();
        TestTree4C(tree);
    }

    [Test]
    public void BinaryTreeAvlDeleteValidation5A()
    {
        var tree = new AvlTree<int, int>();
        TestTree5A(tree);
    }

    [Test]
    public void BinaryTreeAvlDeleteValidation5B()
    {
        var tree = new AvlTree<int, int>();
        TestTree5B(tree);
    }

    [Test]
    public void BinaryTreeAvlDeleteValidation5C()
    {
        var tree = new AvlTree<int, int>();
        TestTree5C(tree);
    }

    [Test]
    public void BinaryTreeUnbalancedDeleteValidation5A()
    {
        var tree = new UnbalancedBinaryTree<int, int>();
        TestTree5A(tree);
    }

    [Test]
    public void BinaryTreeUnbalancedDeleteValidation5B()
    {
        var tree = new UnbalancedBinaryTree<int, int>();
        TestTree5B(tree);
    }

    [Test]
    public void BinaryTreeUnbalancedDeleteValidation5C()
    {
        var tree = new UnbalancedBinaryTree<int, int>();
        TestTree5C(tree);
    }

    [Test]
    public void BinaryTreeScapegoatDeleteValidation5A()
    {
        var tree = new ScapegoatTree<int, int>();
        TestTree5A(tree);
    }

    [Test]
    public void BinaryTreeScapegoatDeleteValidation5B()
    {
        var tree = new ScapegoatTree<int, int>();
        TestTree5B(tree);
    }

    [Test]
    public void BinaryTreeScapegoatDeleteValidation5C()
    {
        var tree = new ScapegoatTree<int, int>();
        TestTree5C(tree);
    }

    [Test]
    public void BinaryTreeAvlDeleteValidation6A()
    {
        var tree = new AvlTree<int, int>();
        TestTree6A(tree);
    }

    [Test]
    public void BinaryTreeAvlDeleteValidation6B()
    {
        var tree = new AvlTree<int, int>();
        TestTree6B(tree);
    }

    [Test]
    public void BinaryTreeAvlDeleteValidation6C()
    {
        var tree = new AvlTree<int, int>();
        TestTree6C(tree);
    }

    [Test]
    public void BinaryTreeUnbalancedDeleteValidation6A()
    {
        var tree = new UnbalancedBinaryTree<int, int>();
        TestTree6A(tree);
    }

    [Test]
    public void BinaryTreeUnbalancedDeleteValidation6B()
    {
        var tree = new UnbalancedBinaryTree<int, int>();
        TestTree6B(tree);
    }

    [Test]
    public void BinaryTreeUnbalancedDeleteValidation6C()
    {
        var tree = new UnbalancedBinaryTree<int, int>();
        TestTree6C(tree);
    }

    [Test]
    public void BinaryTreeScapegoatDeleteValidation6A()
    {
        var tree = new ScapegoatTree<int, int>();
        TestTree6A(tree);
    }

    [Test]
    public void BinaryTreeScapegoatDeleteValidation6B()
    {
        var tree = new ScapegoatTree<int, int>();
        TestTree6B(tree);
    }

    [Test]
    public void BinaryTreeScapegoatDeleteValidation6C()
    {
        var tree = new ScapegoatTree<int, int>();
        TestTree6C(tree);
    }

    [Test]
    public void BinaryTreeAvlDeleteValidation7A()
    {
        var tree = new AvlTree<int, int>();
        TestTree7A(tree);
    }

    [Test]
    public void BinaryTreeAvlDeleteValidation7B()
    {
        var tree = new AvlTree<int, int>();
        TestTree7B(tree);
    }

    [Test]
    public void BinaryTreeAvlDeleteValidation7C()
    {
        var tree = new AvlTree<int, int>();
        TestTree7C(tree);
    }

    [Test]
    public void BinaryTreeUnbalancedDeleteValidation7A()
    {
        var tree = new UnbalancedBinaryTree<int, int>();
        TestTree7A(tree);
    }

    [Test]
    public void BinaryTreeUnbalancedDeleteValidation7B()
    {
        var tree = new UnbalancedBinaryTree<int, int>();
        TestTree7B(tree);
    }

    [Test]
    public void BinaryTreeUnbalancedDeleteValidation7C()
    {
        var tree = new UnbalancedBinaryTree<int, int>();
        TestTree7C(tree);
    }

    //Tree 7 tests are not run for Scapegoat Tree because they trigger a rebalance which
    //means the tree will not be as we expect
    //NB - Tree 4c, 5 and 6 tests also trigger the rebalance but they always leave the tree in the state
    //we expect so they can be safely run

    [Test]
    public void BinaryTreeAvlDeleteValidation8A()
    {
        var tree = new AvlTree<int, int>();
        TestTree8A(tree);
    }

    [Test]
    public void BinaryTreeAvlDeleteValidation8B()
    {
        var tree = new AvlTree<int, int>();
        TestTree8B(tree);
    }

    [Test]
    public void BinaryTreeAvlDeleteValidation8C()
    {
        var tree = new AvlTree<int, int>();
        TestTree8C(tree);
    }

    [Test]
    public void BinaryTreeUnbalancedDeleteValidation8A()
    {
        var tree = new UnbalancedBinaryTree<int, int>();
        TestTree8A(tree);
    }

    [Test]
    public void BinaryTreeUnbalancedDeleteValidation8B()
    {
        var tree = new UnbalancedBinaryTree<int, int>();
        TestTree8B(tree);
    }

    [Test]
    public void BinaryTreeUnbalancedDeleteValidation8C()
    {
        var tree = new UnbalancedBinaryTree<int, int>();
        TestTree8C(tree);
    }

    //Tree 8 tests are not run for Scapegoat Tree because they trigger a rebalance which
    //means the tree will not be as we expect
    //NB - Tree 4c, 5 and 6 tests also trigger the rebalance but they always leave the tree in the state
    //we expect so they can be safely run

    [Test]
    public void BinaryTreeUnbalancedDeleteValidation9A()
    {
        var tree = new UnbalancedBinaryTree<int, int>();
        TestTree9A(tree);
    }

    [Test]
    public void BinaryTreeUnbalancedDeleteValidation9B()
    {
        var tree = new UnbalancedBinaryTree<int, int>();
        TestTree9B(tree);
    }

    [Test]
    public void BinaryTreeUnbalancedDeleteValidation9C()
    {
        var tree = new UnbalancedBinaryTree<int, int>();
        TestTree9C(tree);
    }

    [Test]
    public void BinaryTreeUnbalancedDeleteValidation9d()
    {
        var tree = new UnbalancedBinaryTree<int, int>();
        TestTree9d(tree);
    }

    [Test]
    public void BinaryTreeUnbalancedDeleteValidation10A()
    {
        var tree = new UnbalancedBinaryTree<int, int>();
        TestTree10A(tree);
    }

    [Test]
    public void BinaryTreeUnbalancedDeleteValidation10B()
    {
        var tree = new UnbalancedBinaryTree<int, int>();
        TestTree10B(tree);
    }

    [Test]
    public void BinaryTreeUnbalancedDeleteValidation10C()
    {
        var tree = new UnbalancedBinaryTree<int, int>();
        TestTree10C(tree);
    }

    [Test]
    public void BinaryTreeUnbalancedDeleteValidation10d()
    {
        var tree = new UnbalancedBinaryTree<int, int>();
        TestTree10d(tree);
    }
}