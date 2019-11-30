using System.Collections.Generic;
using Vertical.Tools.CSharpDependencies.Models;

namespace Vertical.Tools.CSharpDependencies.Abstractions
{
    /// <summary>
    /// Provides access to the options.
    /// </summary>
    public interface IOptions
    {
        OutputFormat OutputFormat { get; }
        
        IEnumerable<string> IncludeGlobs { get; }
        
        IEnumerable<string> ExcludeGlobs { get; }
        
        IEnumerable<string> SourceDirectories { get; }
    }
}