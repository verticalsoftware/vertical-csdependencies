using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Serilog;
using Shouldly;
using Vertical.Tools.CSharpDependencies.Abstractions;
using Vertical.Tools.CSharpDependencies.Models;
using Xunit;

namespace Vertical.Tools.CSharpDependencies.Services
{
    public class ProjectAggregatorTest
    {
        [Fact]
        public async Task AggregateProjectsAsync_Outputs_Projects_In_Dependency_Order()
        {
            var allProjects = Projects;
            var searchProviderMock = new Mock<IProjectSearchProvider>();
            var projectBuilderMock = new Mock<IProjectBuilder>();
            var outputChannelMock = new Mock<IOutputChannel>();
            var results = new List<string>();
            
            searchProviderMock.Setup(m => m.GetProjectPaths()).Returns(
                allProjects.Select(p => p.Path).ToArray());

            foreach (var project in allProjects)
            {
                projectBuilderMock
                    .Setup(m => m.DeserializeFromXmlAsync(project.Path))
                    .ReturnsAsync(project);
            }

            outputChannelMock.Setup(m => m.DisplayOutput(It.IsAny<IEnumerable<Project>>()
                    , It.IsAny<IReadOnlyDictionary<string, Project>>()))
                .Callback<IEnumerable<Project>, IReadOnlyDictionary<string, Project>>((projects, _) =>
                    results.AddRange(projects.Select(p => p.ProjectName)));
            
            var subject = new ProjectAggregator(searchProviderMock.Object
                , projectBuilderMock.Object
                , outputChannelMock.Object
                , new Mock<ILogger>().Object);

            await subject.AggregateProjectsAsync();
            
            results.ShouldBe(new[]
            {
                "core",
                "diagnostics.abstractions",
                "diagnostics",
                "logging.abstractions",
                "logging",
                "webapp"
            });
        }

        private static Project[] Projects = new[]
        {
            new {project = "core", dependencies = Array.Empty<string>()},
            new {project = "diagnostics.abstractions", dependencies = new[]{"core"}},
            new {project = "logging.abstractions", dependencies = new[]{"core"}},
            new {project = "diagnostics", dependencies = new[]{"diagnostics.abstractions"}},
            new {project = "logging", dependencies = new[]{"logging.abstractions"}},
            new {project = "webapp", dependencies = new[]{"diagnostics","logging"}}
        }.Select(e => new Project
        {
            ProjectName = e.project,
            Path = $"/src/{e.project}",
            Metadata = new ProjectMetadata
            {
                ItemGroups = new[]
                {
                    new ItemGroup
                    {
                        PackageReferences = e
                            .dependencies
                            .Select(d => new PackageReference { Include = d })
                            .ToArray()
                    }
                }
            }
        }).ToArray();

    }
}