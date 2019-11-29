using System.Linq;
using Shouldly;
using Xunit;

namespace Vertical.Tools.CSharpDependencies.Models
{
    public class ProjectExtensionsTest
    {
        [Fact]
        public void GetPackageReferences_Aggregates_All_Item_Groups()
        {
            var project = new Project
            {
                Metadata = new ProjectMetadata
                {
                    ItemGroups = new[]
                    {
                        new ItemGroup
                        {
                            PackageReferences = new[]
                            {
                                new PackageReference {Include = "dependency1"}
                                , new PackageReference {Include = "dependency2"},
                            }
                        }
                        , new ItemGroup
                        {
                            PackageReferences = new[]
                            {
                                new PackageReference {Include = "dependency3"}
                                , new PackageReference {Include = "dependency4"},
                            }
                        }
                    }
                }
            };

            var references = project.GetPackageReferences().ToArray();
            references[0].Include.ShouldBe("dependency1");
            references[1].Include.ShouldBe("dependency2");
            references[2].Include.ShouldBe("dependency3");
            references[3].Include.ShouldBe("dependency4");
        }

        [Fact]
        public void GetDependencyKey_Returns_ProjectName_As_Default()
        {
            new Project { ProjectName = "project" }.GetDependencyKey().ShouldBe("project");
        }

        [Fact]
        public void GetDependencyKey_Returns_AssemblyName_Before_Project_Name()
        {
            var project = new Project
            {
                ProjectName = "project", Metadata = new ProjectMetadata
                {
                    PropertyGroups = new[]
                    {
                        new PropertyGroup {AssemblyName = "assembly"}
                    }
                }
            };
            
            project.GetDependencyKey().ShouldBe("assembly");
        }

        [Fact]
        public void GetDependencyKey_Returns_PackageId_Before_Project_Or_Assembly_Name()
        {
            var project = new Project
            {
                ProjectName = "project", Metadata = new ProjectMetadata
                {
                    PropertyGroups = new[]
                    {
                        new PropertyGroup {AssemblyName = "assembly"},
                        new PropertyGroup {PackageId = "package"}
                    }
                }
            };
            
            project.GetDependencyKey().ShouldBe("package");
        }
    }
}