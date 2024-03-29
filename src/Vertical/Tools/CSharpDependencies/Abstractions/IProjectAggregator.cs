// Copyright(c) 2019 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using System.Threading.Tasks;

namespace Vertical.Tools.CSharpDependencies.Abstractions
{
    /// <summary>
    /// Represents an object that aggregates project dependencies.
    /// </summary>
    public interface IProjectAggregator
    {
        /// <summary>
        /// Aggregates the projects.
        /// </summary>
        Task AggregateProjectsAsync();
    }
}