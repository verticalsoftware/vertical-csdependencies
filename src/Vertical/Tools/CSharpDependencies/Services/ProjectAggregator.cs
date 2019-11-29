// Copyright(c) 2019 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using Vertical.Tools.CSharpDependencies.Abstractions;
using Vertical.Tools.CSharpDependencies.Models;

namespace Vertical.Tools.CSharpDependencies.Services
{
    /// <summary>
    /// Represents a project aggregator.
    /// </summary>
    public class ProjectAggregator : IProjectAggregator
    {
        /// <summary>
        /// Represents aggregation info.
        /// </summary>
        private sealed class AggregationInfo
        {
            internal IDictionary<string, Project> ProjectDictionary { get; set; }
            internal ISet<Project> BuildSet { get; set; }
            internal LinkedList<Project> OrderedList { get; set; }
        }

        private readonly IProjectSearchProvider _projectSearchProvider;
        private readonly IProjectBuilder _projectBuilder;
        private readonly IOutputChannel _outputChannel;
        private readonly ILogger _logger;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="projectSearchProvider">Search provider</param>
        /// <param name="projectBuilder">Project builder</param>
        /// <param name="outputChannel">Output channel</param>
        /// <param name="logger">Logger</param>
        public ProjectAggregator(IProjectSearchProvider projectSearchProvider
            , IProjectBuilder projectBuilder
            , IOutputChannel outputChannel
            , ILogger logger)
        {
            _projectSearchProvider = projectSearchProvider;
            _projectBuilder = projectBuilder;
            _outputChannel = outputChannel;
            _logger = logger;
        }
        
        /// <summary>
        /// Aggregates the projects.
        /// </summary>
        public async Task AggregateProjectsAsync()
        {
            var projectPaths = _projectSearchProvider.GetProjectPaths();
            var deserializationTasks = projectPaths
                .AsParallel()
                .Select(path => _projectBuilder.DeserializeFromXmlAsync(path));
            var projects = await Task.WhenAll(deserializationTasks);
            var aggregationInfo = new AggregationInfo
            {
                ProjectDictionary = projects.ToDictionary(project => project.ProjectName, StringComparer.OrdinalIgnoreCase),
                BuildSet = new HashSet<Project>(projects.Length),
                OrderedList = new LinkedList<Project>()
            };

            foreach (var project in projects.OrderBy(p => p.ProjectName))
            {
                AddProjectToAggregationInfo(aggregationInfo, project);
            }    
            
            _outputChannel.DisplayOutput(aggregationInfo.OrderedList
                , (IReadOnlyDictionary<string, Project>)aggregationInfo.ProjectDictionary);
        }

        private void AddProjectToAggregationInfo(AggregationInfo aggregationInfo, Project project)
        {
            if (!aggregationInfo.BuildSet.Add(project))
                return;
            
            _logger.Debug("Add to aggregate root {name} @{path}"
                , project.ProjectName
                , project.Directory);

            var references = project
                .GetPackageReferences()
                .Select(reference => reference.Include)
                .Distinct();

            foreach (var reference in references)
            {
                if (!aggregationInfo.ProjectDictionary.TryGetValue(reference, out var dependentProject))
                    continue;
                if (aggregationInfo.BuildSet.Contains(dependentProject))
                    continue;
                AddProjectToAggregationInfo(aggregationInfo, dependentProject);
            }

            aggregationInfo.OrderedList.AddLast(project);
        }
    }
}