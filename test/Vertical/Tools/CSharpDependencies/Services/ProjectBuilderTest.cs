using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Serilog;
using Shouldly;
using Vertical.Tools.CSharpDependencies.Models;
using Xunit;

namespace Vertical.Tools.CSharpDependencies.Services
{
    public class ProjectBuilderTest
    {
        [Fact]
        public async Task DeserializeFromXmlAsync_Returns_Expected_Values()
        {
            var serializer = ProjectBuilder.CreateSerializer();
            var subject = new ProjectBuilder(serializer, new FileSystem(), new Mock<ILogger>().Object);
            var project = await subject.DeserializeFromXmlAsync("Resources/Project.xml");
            
            project.ProjectName.ShouldBe("Project");
            project.Path.ShouldBe("Resources/Project.xml");
            project.FileName.ShouldBe("Project.xml");
            project.Directory.ShouldBe("Resources");
            
            project.GetAssemblyName().ShouldBe("Vertical.Assembly");
            project.GetPackageId().ShouldBe("Vertical.Package");
            project.GetIsPackable().ShouldBeTrue();
            
            project.Metadata.Sdk.ShouldBe("Microsoft.NET.Sdk");
            project.Metadata.ItemGroups[0].Condition.ShouldBe("'$(TargetFramework)'=='.NetCoreApp3.0'"); 

            var packageReferences = project
                .GetPackageReferences()
                .Select(reference => (reference.Include, reference.Version))
                .ToArray();
            
            packageReferences.ShouldBe(new[]
            {
                ("Dotnet.Glob", "3.0.5"),
                ("microsoft.extensions.dependencyinjection", "3.0.1"),
                ("serilog.sinks.console", "3.1.1"),
                ("system.io.abstractions", "7.0.7"),
                ("vertical-commandline", "1.0.2"),
                ("microsoft.extensions.logging", "3.0.1"),
                ("microsoft.extensions.objectpool", "3.0.1")
            });

            var propertyGroup = project.Metadata.PropertyGroups.Single();
            propertyGroup.TargetFramework.ShouldBe("netcoreapp3.0");
            propertyGroup.IsPackable.ShouldBeTrue();
            propertyGroup.AssemblyName.ShouldBe("Vertical.Assembly");
            propertyGroup.PackageVersion.ShouldBe("1.0.0-DEV");
            propertyGroup.Version.ShouldBe("2.0.0");
            propertyGroup.VersionPrefix.ShouldBe("3.0.0");
            propertyGroup.VersionSuffix.ShouldBe("DEV");
            propertyGroup.TargetFrameworks.ShouldBe("net451;net461");
        }
    }
}