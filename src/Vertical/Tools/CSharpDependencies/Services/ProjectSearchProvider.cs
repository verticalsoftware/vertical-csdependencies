// Copyright(c) 2019 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using DotNet.Globbing;
using Serilog;
using Vertical.Tools.CSharpDependencies.Abstractions;

namespace Vertical.Tools.CSharpDependencies.Services
{
    /// <summary>
    /// Represents a provider for project files.
    /// </summary>
    public class ProjectSearchProvider : IProjectSearchProvider
    {
        private readonly IFileSystem _fileSystem;
        private readonly ILogger _logger;
        private readonly IOptions _options;
        private readonly Glob[] _includeGlobs;
        private readonly Glob[] _excludeGlobs;

        /// <summary>
        /// Creates a new instance of this type.
        /// </summary>
        /// <param name="fileSystem">File system</param>
        /// <param name="logger">Logger instance</param>
        /// <param name="options">Runtime options</param>
        public ProjectSearchProvider(IFileSystem fileSystem
            , ILogger logger
            , IOptions options)
        {
            _fileSystem = fileSystem;
            _logger = logger;
            _options = options;
            _includeGlobs = options.IncludeGlobs.Select(Glob.Parse).ToArray();
            _excludeGlobs = options.ExcludeGlobs.Select(Glob.Parse).ToArray();
        }

        /// <summary>
        /// Gets the project paths.
        /// </summary>
        /// <returns>String</returns>
        public IEnumerable<string> GetProjectPaths()
        {
            var paths = _options
                .SourceDirectories
                .SelectMany(GetProjectsInPath);

            return paths;
        }

        private IEnumerable<string> GetProjectsInPath(string path)
        {
            var resolvedPath = _fileSystem.Path.GetFullPath(path);
            
            _logger.Debug("Recursively searching source path {path}", path);

            return _fileSystem
                .Directory
                .EnumerateFiles(resolvedPath, "*", SearchOption.AllDirectories)
                .Where(IsProjectMatch);
        }

        private bool IsProjectMatch(string file) => IsIncludedGlobMatch(file) && !IsExcludedGlobMatch(file);

        private bool IsIncludedGlobMatch(string file)
        {
            var result = _includeGlobs.Any(glob => glob.IsMatch(file));
            if (result) _logger.Debug("-> File {file} matched include pattern", file);
            return result;
        }

        private bool IsExcludedGlobMatch(string file)
        {
            var result = _excludeGlobs.Any(glob => glob.IsMatch(file));
            if (result) _logger.Debug("-> File {file} matched exclude pattern and will be skipped.", file);
            return result;
        }
    }
}