// Copyright(c) 2019 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using Vertical.Tools.CSharpDependencies.Models;

namespace Vertical.Tools.CSharpDependencies.Abstractions
{
    /// <summary>
    /// Formats output
    /// </summary>
    public interface IOutputFormatter
    {
        /// <summary>
        /// Formats the project to a configured string.
        /// </summary>
        /// <param name="project">Project</param>
        /// <returns>String</returns>
        string FormatProject(Project project);
    }
}