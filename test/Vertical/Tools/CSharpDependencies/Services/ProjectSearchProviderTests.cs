using System.IO;
using System.IO.Abstractions;
using Moq;
using Serilog;
using Shouldly;
using Vertical.Tools.CSharpDependencies.Configuration;
using Xunit;

namespace Vertical.Tools.CSharpDependencies.Services
{
    public class ProjectSearchProviderTests
    {
        [Fact]
        public void GetProjectPaths_Returns_Paths_Included_Glob_Patterns()
        {
            var subject = new ProjectSearchProvider(CreateFileSystem()
                , new Mock<ILogger>().Object
                , new RunOptions
                {
                    SourceDirectories = {"/src"}, 
                    IncludeGlobs = {"/src/core/*.csproj"},
                    ExcludeGlobs = { }
                });
            
            subject.GetProjectPaths().ShouldBe(new[]
            {
                "/src/core/diagnostics.csproj",
                "/src/core/logging.csproj",
                "/src/core/security.csproj"
            });
        }
        
        [Fact]
        public void GetProjectPaths_Returns_Paths_Filtered_By_Excluded_Glob_Patterns()
        {
            var subject = new ProjectSearchProvider(CreateFileSystem()
                , new Mock<ILogger>().Object
                , new RunOptions
                {
                    SourceDirectories = {"/src"}, 
                    IncludeGlobs = {"/src/**/*.csproj"},
                    ExcludeGlobs = { "/**/logging*" }
                });
            
            subject.GetProjectPaths().ShouldBe(new[]
            {
                "/src/core/diagnostics.csproj",
                "/src/core/security.csproj",
                "/src/services/dataAccess.csproj",
                "/src/services/workQueue.csproj"
            });
        }

        private static IFileSystem CreateFileSystem()
        {
            var fileSystemMock = new Mock<IFileSystem>();
            var pathMock = new Mock<IPath>();
            var directoryMock = new Mock<IDirectory>();
            
            pathMock.Setup(m => m.GetFullPath("/src")).Returns("/src");
            directoryMock.Setup(m => m.EnumerateFiles("/src", "*", SearchOption.AllDirectories))
                .Returns(new[]
                {
                    "/src/core/diagnostics.csproj",
                    "/src/core/logging.csproj",
                    "/src/core/security.csproj",
                    "/src/services/dataAccess.csproj",
                    "/src/services/workQueue.csproj"
                });

            fileSystemMock.SetupGet(m => m.Path).Returns(pathMock.Object);
            fileSystemMock.SetupGet(m => m.Directory).Returns(directoryMock.Object);
            
            return fileSystemMock.Object;
        }
    }
}