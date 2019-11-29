// Copyright(c) 2019 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

namespace Vertical.Tools.CSharpDependencies.Models
{
    /// <summary>
    /// Defines the output format.
    /// </summary>
    public enum OutputFormat
    {
        /// <summary>
        /// Output the full path to the project directory.
        /// </summary>
        Directory,
        
        /// <summary>
        /// Output the project file name with extension.
        /// </summary>
        FileName,
        
        /// <summary>
        /// Output the full path to the project file.
        /// </summary>
        Path,
        
        /// <summary>
        /// Output the project name (file name without extension).
        /// </summary>
        ProjectName,
        
        /// <summary>
        /// Output the assembly name.
        /// </summary>
        AssemblyName
    }
}