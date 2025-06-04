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
using System.Text;

namespace VDS.Common.Trees;

public static class BinaryTreeTools
{
    public static void PrintBinaryTreeStructs<TNode, TKey>(ITree<TNode, TKey, TKey> tree)
        where TNode : class, IBinaryTreeNode<TKey, TKey>
        where TKey : struct
    {
        Console.Write("Root: ");
        Console.WriteLine(PrintBinaryTreeNodeStructs<TNode, TKey>(tree.Root));
        Console.WriteLine();
    }

    public static string PrintBinaryTreeNodeStructs<TNode, TKey>(TNode node)
        where TNode : class, IBinaryTreeNode<TKey, TKey>
        where TKey : struct
    {
        if (node == null)
        {
            return " null";
        }
        var builder = new StringBuilder();
        builder.AppendLine("{");
        builder.AppendLine("  Key: " + node.Key);
        builder.AppendLine("  Value: " + node.Value);
        var lhs = PrintBinaryTreeNodeStructs<TNode, TKey>((TNode)node.LeftChild);
        if (lhs.Contains("\n"))
        {
            builder.Append("  Left Child: ");
            builder.AppendLine(AddIndent(lhs));
        }
        else
        {
            builder.AppendLine("  Left Child: " + lhs);
        }
        var rhs = PrintBinaryTreeNodeStructs<TNode, TKey>((TNode)node.RightChild);
        if (rhs.Contains("\n"))
        {
            builder.Append("  Right Child: ");
            builder.AppendLine(AddIndent(rhs));
        }
        else
        {
            builder.AppendLine("  Right Child: " + rhs);
        }
        builder.Append("}");
        return builder.ToString();
    }

    private static string AddIndent(string input)
    {
        var lines = input.Split('\n');
        var output = new StringBuilder();
        for (var i = 0; i < lines.Length; i++)
        {
            if (i > 0)
            {
                output.Append("  " + lines[i]);
                if (i < lines.Length - 1) output.AppendLine();
            }
            else
            {
                output.AppendLine(lines[i]);
            }
        }

        return output.ToString();
    }
}