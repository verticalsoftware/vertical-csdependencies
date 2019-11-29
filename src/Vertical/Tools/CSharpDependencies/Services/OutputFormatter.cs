// Copyright(c) 2019 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using System;
using Serilog;
using Vertical.Tools.CSharpDependencies.Abstractions;
using Vertical.Tools.CSharpDependencies.Configuration;
using Vertical.Tools.CSharpDependencies.Models;

namespace Vertical.Tools.CSharpDependencies.Services
{
    /// <summary>
    /// Formats output.
    /// </summary>
    public class OutputFormatter : IOutputFormatter
    {
        private readonly ILogger _logger;
        private readonly OutputFormat _format;

        /// <summary>
        /// Creates a new instance of this type.
        /// </summary>
        /// <param name="options">Options.</param>
        /// <param name="logger">Logger</param>
        public OutputFormatter(RunOptions options, ILogger logger)
        {
            _logger = logger;
            _format = options.OutputFormat;
        }

        /// <summary>
        /// Formats the project to a configured string.
        /// </summary>
        /// <param name="project">Project</param>
        /// <returns>String</returns>
        public string FormatProject(Project project)
        {
            var result = _format switch
            {
                OutputFormat.Path => project.Path
                , OutputFormat.Directory => project.Directory
                , OutputFormat.AssemblyName => project.GetAssemblyName()
                , OutputFormat.FileName => project.FileName
                , OutputFormat.ProjectName => project.ProjectName
                , _ => throw new NotSupportedException()
            };
            
            _logger.Debug("Format project info ({format}) for {project}={result}"
                , _format
                , project.ProjectName
                , result);

            return result;
        }
    }
}