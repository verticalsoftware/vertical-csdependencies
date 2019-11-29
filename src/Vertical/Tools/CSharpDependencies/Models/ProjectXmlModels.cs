// Copyright(c) 2019 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

namespace Vertical.Tools.CSharpDependencies.Models
{
    /// <summary>
    /// Represents the root project element.
    /// </summary>
    public class Project
    {
        public string Path { get; set; }
        
        public string ProjectName { get; set; }
        
        public string Directory { get; set; }
        
        public string FileName { get; set; }
        
        public ProjectMetadata Metadata { get; set; }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage]
        public override string ToString() => ProjectName;
    }

    /// <summary>
    /// Represents project metadata.
    /// </summary>
    public class ProjectMetadata
    {
        [XmlAttribute("Sdk")]
        public string Sdk { get; set; }
        
        [XmlElement("PropertyGroup")]
        public PropertyGroup[] PropertyGroups { get; set; }
        
        [XmlElement("ItemGroup")]
        public ItemGroup[] ItemGroups { get; set; }
    }

    public class PropertyGroup
    {
        [XmlElement("TargetFramework")]
        public string TargetFramework { get; set; }
        
        [XmlElement("TargetFrameworks")]
        public string TargetFrameworks { get; set; }
        
        [XmlElement("IsPackable")]
        public bool IsPackable { get; set; }
        
        [XmlElement("AssemblyName")]
        public string AssemblyName { get; set; }
        
        [XmlElement("PackageId")]
        public string PackageId { get; set; }
        
        [XmlElement("PackageVersion")]
        public string PackageVersion { get; set; }
        
        [XmlElement("Version")]
        public string Version { get; set; }
        
        [XmlElement("VersionPrefix")]
        public string VersionPrefix { get; set; }
        
        [XmlElement("VersionSuffix")]
        public string VersionSuffix { get; set; }
    }

    /// <summary>
    /// Represents an item group.
    /// </summary>
    public class ItemGroup
    {
        [XmlAttribute("Condition")]
        public string Condition { get; set; }
        
        [XmlElement("PackageReference")]
        public PackageReference[] PackageReferences { get; set; }
    }

    /// <summary>
    /// Represents a package reference.
    /// </summary>
    public class PackageReference
    {
        [XmlAttribute("Include")]
        public string Include { get; set; }
        
        [XmlAttribute("Version")]
        public string Version { get; set; }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage]
        public override string ToString() => $"{Include}.{Version}";
    }
}