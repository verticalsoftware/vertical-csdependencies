using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Vertical.Tools.CSharpDependencies
{
    public class ProgramTest
    {
        [Fact]
        public async Task Main_Executes_End_To_End()
        {
            await Program.Main(new[]{Directory.GetCurrentDirectory()});
        }
    }
}