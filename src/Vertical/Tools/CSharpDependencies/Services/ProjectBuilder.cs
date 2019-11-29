// Copyright(c) 2019 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using System.IO;
using System.IO.Abstractions;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Serilog;
using Vertical.Tools.CSharpDependencies.Abstractions;
using Vertical.Tools.CSharpDependencies.Models;

namespace Vertical.Tools.CSharpDependencies.Services
{
    /// <summary>
    /// Project builder implementation.
    /// </summary>
    public class ProjectBuilder : IProjectBuilder
    {
        private readonly XmlSerializer _xmlSerializer;
        private readonly IFileSystem _fileSystem;
        private readonly ILogger _logger;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="xmlSerializer">Xml serializer instance</param>
        /// <param name="fileSystem">File system</param>
        /// <param name="logger"></param>
        public ProjectBuilder(XmlSerializer xmlSerializer
            , IFileSystem fileSystem
            , ILogger logger)
        {
            _xmlSerializer = xmlSerializer;
            _fileSystem = fileSystem;
            _logger = logger;
        }
        
        /// <summary>
        /// Creates a serializer.
        /// </summary>
        /// <returns><see cref="XmlSerializer"/></returns>
        public static XmlSerializer CreateSerializer() => new XmlSerializer(typeof(ProjectMetadata)
            , new XmlRootAttribute("Project"));
        
        /// <summary>
        /// Deserializes a project from xml content.
        /// </summary>
        /// <param name="path">Path to xml file.</param>
        /// <returns>Project</returns>
        public async Task<Project> DeserializeFromXmlAsync(string path)
        {
            _logger.Debug("Deserialize project path {path} to model");
            
            await using var stream = new MemoryStream(await _fileSystem.File.ReadAllBytesAsync(path));

            var metadata = (ProjectMetadata) _xmlSerializer.Deserialize(stream);
            
            return new Project
            {
                Directory = _fileSystem.Path.GetDirectoryName(path),
                Metadata = metadata,
                Path = path,
                FileName = _fileSystem.Path.GetFileName(path),
                ProjectName = _fileSystem.Path.GetFileNameWithoutExtension(path)
            };
        }
    }
}