using System.IO.Abstractions;
using System.Linq;
using Moq;
using Shouldly;
using Vertical.Tools.CSharpDependencies.Configuration;
using Xunit;

namespace Vertical.Tools.CSharpDependencies.Services
{
    public class OptionsProviderTest
    {
        [Fact]
        public void IncludeGlobs_Return_Default_When_None_In_Options()
        {
            var directoryMock = new Mock<IDirectory>();
            directoryMock.Setup(m => m.GetCurrentDirectory()).Returns("/usr/bin");
            var subject = new OptionsProvider(new RunOptions(), directoryMock.Object);
            subject.IncludeGlobs.Single().ShouldBe("**/*.csproj");
        }

        [Fact]
        public void SourceDirectories_Returns_Current_Directory_When_None_In_Options()
        {
            var directoryMock = new Mock<IDirectory>();
            directoryMock.Setup(m => m.GetCurrentDirectory()).Returns("/usr/bin");
            var subject = new OptionsProvider(new RunOptions(), directoryMock.Object);
            subject.SourceDirectories.Single().ShouldBe("/usr/bin");
        }
    }
}