using System.Linq;
using Shouldly;
using Vertical.CommandLine;
using Vertical.Tools.CSharpDependencies.Models;
using Xunit;

namespace Vertical.Tools.CSharpDependencies.Configuration
{
    public class RunOptionsConfigurationTest
    {
        private readonly RunOptions _options = new RunOptions();

        [Fact]
        public void Options_Build()
        {
            var configuration = new RunOptionsConfiguration();
            var options = CommandLineApplication.ParseArguments<RunOptions>(configuration
                , new[] {"/src", "-i", "*", "-x", "*.Test", "--tree", "--debug", "-o", "directory"});

            options.SourceDirectories.Single().ShouldBe("/src");
            options.IncludeGlobs.Single().ShouldBe("*");
            options.ExcludeGlobs.Single().ShouldBe("*.Test");
            options.Tree.ShouldBeTrue();
            options.DebugOutput.ShouldBeTrue();
            options.OutputFormat.ShouldBe(OutputFormat.Directory);
        }
    }
}