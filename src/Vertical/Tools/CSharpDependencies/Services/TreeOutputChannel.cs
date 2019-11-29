// Copyright(c) 2019 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using Vertical.Tools.CSharpDependencies.Abstractions;
using Vertical.Tools.CSharpDependencies.Models;

namespace Vertical.Tools.CSharpDependencies.Services
{
    /// <summary>
    /// Output formatter for the tree option
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class TreeOutputChannel : IOutputChannel
    {
        private readonly IOutputFormatter _outputFormatter;

        /// <summary>
        /// Creates a new instance of this type.
        /// </summary>
        /// <param name="outputFormatter">Output formatter</param>
        public TreeOutputChannel(IOutputFormatter outputFormatter)
        {
            _outputFormatter = outputFormatter;
        }

        /// <inheritdoc />
        public void DisplayOutput(IEnumerable<Project> orderedProjects
            , IReadOnlyDictionary<string, Project> projectDictionary)
        {
            Console.OutputEncoding = Encoding.UTF8;
            
            var rootNodes = new List<TreeNode<string>>(32);

            void AddProject(Project project, TreeNode<string> parent = null)
            {
                TreeNode<string> node = null;
                var value = _outputFormatter.FormatProject(project);

                if (parent == null)
                    rootNodes.Add(node = new TreeNode<string>(value));
                else
                    node = parent.AddChild(value);

                var references = project
                    .GetPackageReferences()
                    .Where(pkg => projectDictionary.ContainsKey(pkg.Include))
                    .Select(pkg => projectDictionary[pkg.Include])
                    .OrderBy(proj => proj.ProjectName);

                foreach (var reference in references)
                {
                    AddProject(reference, node);
                }
            }

            foreach (var project in orderedProjects)
            {
                AddProject(project);
            }

            foreach (var node in rootNodes.OrderBy(n => n.Value))
            {
                node.Print(Console.WriteLine);
            }
        }
    }
}