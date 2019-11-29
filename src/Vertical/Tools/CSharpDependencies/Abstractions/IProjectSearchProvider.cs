// Copyright(c) 2019 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using System.Collections.Generic;

namespace Vertical.Tools.CSharpDependencies.Abstractions
{
    /// <summary>
    /// Represents a project search provider.
    /// </summary>
    public interface IProjectSearchProvider
    {
        /// <summary>
        /// Gets the project paths.
        /// </summary>
        /// <returns>String</returns>
        IEnumerable<string> GetProjectPaths();
    }
}