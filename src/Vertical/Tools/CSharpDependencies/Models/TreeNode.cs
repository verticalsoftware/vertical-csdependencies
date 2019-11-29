// Copyright(c) 2019 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Vertical.Tools.CSharpDependencies.Models
{
    /// <summary>
    /// Represents a generic type used to build tree structures.
    /// </summary>
    /// <typeparam name="T">Tree node value type.</typeparam>
    public class TreeNode<T>
    {
        private readonly Lazy<List<TreeNode<T>>> _lazyChildList =
            new Lazy<List<TreeNode<T>>>(() => new List<TreeNode<T>>());

        /// <summary>
        /// Creates a new tree node.
        /// </summary>
        /// <param name="value">The node value.</param>
        public TreeNode(T value) : this(value, null)
        {
        }

        private TreeNode(T value, TreeNode<T> parent)
        {
            Value = value;
            Parent = parent;
        }

        /// <summary>
        /// Gets the node value.
        /// </summary>
        public T Value { get; }

        /// <summary>
        /// Gets the parent node.
        /// </summary>
        public TreeNode<T> Parent { get; }

        /// <summary>
        /// Gets the child at the given index.
        /// </summary>
        /// <param name="index">Index</param>
        /// <returns>Child node</returns>
        public TreeNode<T> this[int index] => Children[index];

        /// <summary>
        /// Gets the child nodes.
        /// </summary>
        public IReadOnlyList<TreeNode<T>> Children => _lazyChildList.IsValueCreated
            ? (IReadOnlyList<TreeNode<T>>) _lazyChildList.Value
            : Array.Empty<TreeNode<T>>();

        /// <summary>
        /// Gets the number of child nodes.
        /// </summary>
        public int Count => Children.Count;

        /// <summary>
        /// Adds a child node.
        /// </summary>
        /// <param name="value">The value to add as a child.</param>
        public TreeNode<T> AddChild(T value)
        {
            var node = new TreeNode<T>(value, this);
            _lazyChildList.Value.Add(node);
            return node;
        }

        /// <summary>
        /// Removes a child node.
        /// </summary>
        /// <param name="node">The node to remove.</param>
        /// <returns>The node to remove.</returns>
        public bool RemoveChild(TreeNode<T> node) => _lazyChildList.IsValueCreated && _lazyChildList.Value.Remove(node);

        /// <inheritdoc />
        [ExcludeFromCodeCoverage]
        public override string ToString() => $"{Value}";
    }
}