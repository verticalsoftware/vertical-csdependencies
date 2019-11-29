// Copyright(c) 2019 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using System;
using System.IO;
using System.Threading.Tasks;
using Vertical.CommandLine.Configuration;
using Vertical.Tools.CSharpDependencies.Models;

namespace Vertical.Tools.CSharpDependencies.Configuration
{
    /// <summary>
    /// Configuration for run options.
    /// </summary>
    public class RunOptionsConfiguration : ApplicationConfiguration<RunOptions>
    {
        private const string DefaultGlobPattern = "**/*.csproj";

        /// <summary>
        /// Creates a new instance of this type.
        /// </summary>
        public RunOptionsConfiguration()
        {
            PositionArgument(arg => arg.MapMany.ToCollection(opt => opt.SourceDirectories));
            Option("-i|--include", arg => arg.MapMany.ToCollection(opt => opt.IncludeGlobs));
            Option("-x|--exclude", arg => arg.MapMany.ToCollection(opt => opt.ExcludeGlobs));
            Option<OutputFormat>("-o|--output", arg => arg.Map.ToProperty(opt => opt.OutputFormat));
            Switch("--debug", arg => arg.Map.ToProperty(opt => opt.DebugOutput));
            Switch("--tree", arg => arg.Map.ToProperty(opt => opt.Tree));
            HelpOption("-h|--help");
        }
        
        /// <summary>
        /// Creates a new run options configuration.
        /// </summary>
        /// <param name="asyncHandler">Async handler.</param>
        public RunOptionsConfiguration(Func<RunOptions, Task> asyncHandler) : this()
        {
            OnExecuteAsync(asyncHandler ?? throw new ArgumentNullException(nameof(asyncHandler)));

            var assemblyPath = Path.GetDirectoryName(typeof(RunOptions).Assembly.Location);

            Help.UseFile(Path.Combine(assemblyPath, "Resources", "help.txt"));
        }
    }
}