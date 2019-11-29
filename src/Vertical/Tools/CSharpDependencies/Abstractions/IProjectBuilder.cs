// Copyright(c) 2019 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using System.Threading.Tasks;
using Vertical.Tools.CSharpDependencies.Models;

namespace Vertical.Tools.CSharpDependencies.Abstractions
{
    /// <summary>
    /// Represents a project builder.
    /// </summary>
    public interface IProjectBuilder
    {
        /// <summary>
        /// Deserializes a project from xml content.
        /// </summary>
        /// <param name="path">Path to xml file.</param>
        /// <returns>Project</returns>
        Task<Project> DeserializeFromXmlAsync(string path);
    }
}