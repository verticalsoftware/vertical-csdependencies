// Copyright(c) 2019 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using System.Collections.Generic;
using Serilog;
using Vertical.Tools.CSharpDependencies.Abstractions;
using Vertical.Tools.CSharpDependencies.Models;

namespace Vertical.Tools.CSharpDependencies.Services
{
    /// <summary>
    /// Represents a default output channel.
    /// </summary>
    public class DefaultOutputChannel : IOutputChannel
    {
        private readonly IOutputFormatter _outputFormatter;
        private readonly ILogger _logger;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="outputFormatter">Output formatter</param>
        /// <param name="logger">Logger</param>
        public DefaultOutputChannel(IOutputFormatter outputFormatter
            , ILogger logger)
        {
            _outputFormatter = outputFormatter;
            _logger = logger;
        }
        
        /// <inheritdoc />
        public void DisplayOutput(IEnumerable<Project> orderedProjects
            , IReadOnlyDictionary<string, Project> projectDictionary)
        {
            _logger.Debug("Routed to default output channel");
            foreach (var project in orderedProjects)
            {
                _logger.Information(_outputFormatter.FormatProject(project));
            }
        }
    }
}