using System.Diagnostics;
using Godot;
using LGWCP.Godot.Extensions;

namespace LGWCP.Godot.Liit;

public partial class GetComponentTest : Node
{
    public override void _Ready()
    {
        for (int _ = 0; _ < 100_000; ++_)
        {
            var node = new Node();
            node.AddChild(new FooComponent(), true);
            node.AddChild(new BarComponent(), true);
            AddChild(node);
        }

        CallDeferred("StartBenchmark");
    }

    public void StartBenchmark()
    {
        var children = GetChildren();

        Stopwatch sw0 = new();
        sw0.Start();
        int cnt0 = 0;
        foreach (var child in children)
        {
            var foo = child.GetNodeOrNull<FooComponent>("Node");
            var bar = child.GetNodeOrNull<BarComponent>("Node2");
            if (foo is not null)
            {
                ++cnt0;
            }
        }
        sw0.Stop();

        Stopwatch sw1 = new();
        sw1.Start();
        int cnt1 = 0;
        foreach (var child in children)
        {
            var foo = ICE.Manager.GetComponent<Node, FooComponent>(child);
            var bar = ICE.Manager.GetComponent<Node, FooComponent>(child);
            if (foo is not null)
            {
                ++cnt1;
            }
        }
        sw1.Stop();

        GD.Print("GetNodeOrNull ", cnt0, " times: ", sw0.ElapsedMilliseconds);
        GD.Print("GetComponent ", cnt1, " times: ", sw1.ElapsedMilliseconds);
    }
}