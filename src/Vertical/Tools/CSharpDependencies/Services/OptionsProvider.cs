using System.Collections.Generic;
using System.IO.Abstractions;
using Vertical.Tools.CSharpDependencies.Abstractions;
using Vertical.Tools.CSharpDependencies.Configuration;
using Vertical.Tools.CSharpDependencies.Models;

namespace Vertical.Tools.CSharpDependencies.Services
{
    /// <summary>
    /// Implements the options provider.
    /// </summary>
    public class OptionsProvider : IOptions
    {
        private const string DefaultIncludePattern = "**/*.csproj";
        private readonly RunOptions _options;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="options">Options.</param>
        /// <param name="directory">File system directory.</param>
        public OptionsProvider(RunOptions options, IDirectory directory)
        {
            _options = options;
            
            IncludeGlobs = options.IncludeGlobs.Count > 0
                ? (IEnumerable<string>) options.IncludeGlobs
                : new[] {DefaultIncludePattern};
            
            SourceDirectories = options.SourceDirectories.Count > 0
                ? (IEnumerable<string>) _options.SourceDirectories
                : new[] {directory.GetCurrentDirectory()};
        }

        /// <inheritdoc />
        public OutputFormat OutputFormat => _options.OutputFormat;

        /// <inheritdoc />
        public IEnumerable<string> IncludeGlobs { get; }

        /// <inheritdoc />
        public IEnumerable<string> ExcludeGlobs => _options.ExcludeGlobs;

        /// <inheritdoc />
        public IEnumerable<string> SourceDirectories { get; }
    }
}