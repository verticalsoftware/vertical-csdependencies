// Copyright(c) 2019 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using System.Collections.Generic;
using Vertical.Tools.CSharpDependencies.Models;

namespace Vertical.Tools.CSharpDependencies.Configuration
{
    /// <summary>
    /// Represents the command line options.
    /// </summary>
    public class RunOptions
    {
        /// <summary>
        /// Gets the source directories.
        /// </summary>
        public List<string> SourceDirectories { get; } = new List<string>();

        /// <summary>
        /// Gets the glob patterns used to match files.
        /// </summary>
        public List<string> IncludeGlobs { get; } = new List<string>();

        /// <summary>
        /// Gets the glob patterns used to exclude files.
        /// </summary>
        public List<string> ExcludeGlobs { get; } = new List<string>();
        
        /// <summary>
        /// Gets/sets whether to display debug output.
        /// </summary>
        public bool DebugOutput { get; set; }
        
        /// <summary>
        /// Gets/sets whether to display the dependency tree.
        /// </summary>
        public bool Tree { get; set; }

        /// <summary>
        /// Gets/sets the output format.
        /// </summary>
        public OutputFormat OutputFormat { get; set; } = OutputFormat.Path;
    }
}