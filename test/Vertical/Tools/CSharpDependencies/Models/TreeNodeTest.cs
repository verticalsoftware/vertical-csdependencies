using System.Collections.Generic;
using Shouldly;
using Xunit;

namespace Vertical.Tools.CSharpDependencies.Models
{
    public class TreeNodeTest
    {
        [Fact]
        public void Print_Displays_Nodes_In_Order()
        {
            var root = new TreeNode<string>("vehicles");
            var cars = root.AddChild("cars");
            var trucks = root.AddChild("trucks");
            cars.AddChild("audi");
            cars.AddChild("vw");
            trucks.AddChild("toyota");
            trucks.AddChild("nissan");

            var output = new List<string>();
            
            root.Print(output.Add);
            
            const string vline = "\U00002502";
            const string vtee  = "\U0000251c";
            const string elbow = "\U00002514";
            const string hline = "\U00002500";
            const string space = " ";

            output.ShouldBe(new[]
            {
                "vehicles",
                $"{vtee}{hline}cars",
                $"{vline}{space}{vtee}{hline}audi",
                $"{vline}{space}{elbow}{hline}vw",
                $"{elbow}{hline}trucks",
                $"{space}{space}{vtee}{hline}toyota",
                $"{space}{space}{elbow}{hline}nissan"
            });
        }

        [Fact]
        public void Parent_Returns_Parent_Value()
        {
            var root = new TreeNode<string>("parent");
            var child = root.AddChild("child");
            child.Parent.Value.ShouldBe("parent");
        }

        [Fact]
        public void Item_Returns_Value()
        {
            var root = new TreeNode<string>("parent");
            root.AddChild("child");
            root[0].Value.ShouldBe("child");
        }

        [Fact]
        public void RemoveChild_Removes_ChildNode()
        {
            var root = new TreeNode<string>("parent");
            var child = root.AddChild("child");
            root.RemoveChild(child);
            
            root.Count.ShouldBe(0);
        }
    }
}