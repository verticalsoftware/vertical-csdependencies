// Copyright(c) 2019 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using System.Collections.Generic;
using Vertical.Tools.CSharpDependencies.Models;

namespace Vertical.Tools.CSharpDependencies.Abstractions
{
    /// <summary>
    /// Represents the output channel.
    /// </summary>
    public interface IOutputChannel
    {
        /// <summary>
        /// Displays the output
        /// </summary>
        /// <param name="orderedProjects">List of projects in order</param>
        /// <param name="projectDictionary">Project dictionary</param>
        void DisplayOutput(IEnumerable<Project> orderedProjects, IReadOnlyDictionary<string, Project> projectDictionary);
    }
}