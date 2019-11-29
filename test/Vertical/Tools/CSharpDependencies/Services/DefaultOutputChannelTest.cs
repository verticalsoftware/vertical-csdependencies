using System.Collections.Generic;
using Moq;
using Serilog;
using Vertical.Tools.CSharpDependencies.Abstractions;
using Vertical.Tools.CSharpDependencies.Models;
using Xunit;

namespace Vertical.Tools.CSharpDependencies.Services
{
    public class DefaultOutputChannelTest
    {
        [Fact]
        public void DisplayOutput_Invokes_OutputFormatter()
        {
            var formatterMock = new Mock<IOutputFormatter>();
            var project = new Project();
            formatterMock.Setup(m => m.FormatProject(project)).Verifiable();
            
            var subject = new DefaultOutputChannel(formatterMock.Object, 
                new Mock<ILogger>().Object);
            
            subject.DisplayOutput(new[]{project}, new Dictionary<string, Project>());
            
            formatterMock.Verify(m => m.FormatProject(project));
        }
    }
}