using System.Collections.Generic;
using Moq;
using Serilog;
using Shouldly;
using Vertical.Tools.CSharpDependencies.Configuration;
using Vertical.Tools.CSharpDependencies.Models;
using Xunit;

namespace Vertical.Tools.CSharpDependencies.Services
{
    public class OutputFormatterTest
    {
        private static readonly Project TestProject = new Project
        {
            Directory = "/src",
            Path = "/src/project.csproj",
            FileName = "project.csproj",
            ProjectName = "project",
            Metadata = new ProjectMetadata
            {
                PropertyGroups = new[]
                {
                    new PropertyGroup {AssemblyName = "project"}
                }
            }
        };

        [Theory, MemberData(nameof(Theories))]
        public void FormatProject_Returns_Expected_Value(OutputFormat format, string expected)
        {
            var subject = new OutputFormatter(new RunOptions { OutputFormat = format }
                , new Mock<ILogger>().Object);
            subject.FormatProject(TestProject).ShouldBe(expected);
        }

        public static IEnumerable<object[]> Theories => new[]
        {
            new object[]{ OutputFormat.Directory, "/src" },
            new object[]{ OutputFormat.Path, "/src/project.csproj" },
            new object[]{ OutputFormat.FileName, "project.csproj" },
            new object[]{ OutputFormat.ProjectName, "project" },
            new object[]{ OutputFormat.AssemblyName, "project" }
        };
    }
}