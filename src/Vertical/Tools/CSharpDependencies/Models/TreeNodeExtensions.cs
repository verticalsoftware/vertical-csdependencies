// Copyright(c) 2019 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vertical.Tools.CSharpDependencies.Models
{
    /// <summary>
    /// Extensions to tree node.
    /// </summary>
    public static class TreeNodeExtensions
    {
        private class PrintNode<T>
        {
            internal PrintNode(TreeNode<T> source, int index, int of, PrintNode<T> parent)
            {
                Source = source;
                Index = index;
                Of = of;
                Parent = parent;
            }

            internal TreeNode<T> Source { get; }
            internal int Index { get; }
            internal int Of { get; }
            internal PrintNode<T> Parent { get; }
            
        }

        public static void Print<T>(this TreeNode<T> node, Action<string> callback, Func<T, string> formatter = null)
        {
            const string vline = "\U00002502";
            const string vtee  = "\U0000251c";
            const string elbow = "\U00002514";
            const string hline = "\U00002500";
            const string space = " ";

            var output = new List<string>(256);
            var builder = new StringBuilder(500);
            var stack = new Stack<PrintNode<T>>();
            
            formatter ??= (n => n.ToString());

            stack.Push(new PrintNode<T>(node, 1, 1, null));

            while (stack.Any())
            {
                var current = stack.Pop();
                var parent = current.Parent;

                for (; parent != null; parent = parent.Parent)
                {
                    if (parent.Parent == null) break;

                    if (parent.Of > 1 && parent.Index < parent.Of)
                        builder.Insert(0, vline);
                    else
                        builder.Insert(0, space);

                    builder.Insert(1, $"{space}");
                }

                if (current.Parent != null)
                {
                    // Not root node
                    if (current.Index == current.Of)
                        builder.Append(elbow);
                    else
                        builder.Append(vtee);

                    builder.Append(hline);
                }

                builder.Append(current.Source.Value);
                callback(builder.ToString());
                builder.Clear();

                var i = current.Source.Children.Count;
                foreach (var child in current.Source.Children.Reverse())
                {
                    stack.Push(new PrintNode<T>(child, i--, current.Source.Count, current));
                }
            }
        }
    }
}