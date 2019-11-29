using Shouldly;
using Vertical.Tools.CSharpDependencies.Configuration;
using Xunit;

namespace Vertical.Tools.CSharpDependencies.Services
{
    public class ServiceContainerTests
    {
        [Fact]
        public void Container_Builds()
        {
            var services = new ServiceContainer(new RunOptions());
            services.ProjectAggregator.ShouldNotBeNull();
        }
    }
}