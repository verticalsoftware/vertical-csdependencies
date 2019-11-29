// Copyright(c) 2019 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using System;
using System.Collections.Generic;
using System.Linq;

namespace Vertical.Tools.CSharpDependencies.Models
{
    /// <summary>
    /// Extension methods on the project model.
    /// </summary>
    public static class ProjectExtensions
    {
        /// <summary>
        /// Gets package references.
        /// </summary>
        /// <param name="project">Project</param>
        /// <returns>PackageReference sequence.</returns>
        public static IEnumerable<PackageReference> GetPackageReferences(this Project project) => project
            .Metadata
            .ItemGroups?
            .SelectMany(group => group?.PackageReferences ?? Array.Empty<PackageReference>())
                ?? Enumerable.Empty<PackageReference>();
        
        /// <summary>
        /// Gets the dependency key for the project.
        /// </summary>
        /// <param name="project">Project</param>
        /// <returns>String</returns>
        public static string GetDependencyKey(this Project project) => project.GetPackageId()
                                                                         ?? project.GetAssemblyName()
                                                                         ?? project.ProjectName;
        /// <summary>
        /// Gets the assembly name.
        /// </summary>
        /// <param name="project">Project</param>
        /// <returns>String</returns>
        public static string GetAssemblyName(this Project project) => 
            project.GetPropertyGroupValue(group => group.AssemblyName, value => !string.IsNullOrWhiteSpace(value));

        /// <summary>
        /// Gets whether the package is packable.
        /// </summary>
        /// <param name="project">Project</param>
        /// <returns>True if package is packable.</returns>
        public static bool GetIsPackable(this Project project) =>
            project.GetPropertyGroupValue(group => group.IsPackable, value => value);
            
        
        /// <summary>
        /// Gets the package id.
        /// </summary>
        /// <param name="project">Project</param>
        /// <returns>String</returns>
        public static string GetPackageId(this Project project) =>
            project.GetPropertyGroupValue(group => group.PackageId, value => !string.IsNullOrWhiteSpace(value));
        
        private static T GetPropertyGroupValue<T>(this Project project
            , Func<PropertyGroup, T> property
            , Func<T, bool> selectPredicate)
        {
            var propertyGroups = project
                .Metadata?
                .PropertyGroups;

            if (propertyGroups == null) return default(T);
            
            return propertyGroups
                .Select(property)
                .FirstOrDefault(selectPredicate);
        }
    }
}