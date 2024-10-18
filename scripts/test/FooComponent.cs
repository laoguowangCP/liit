using Godot;
using LGWCP.Godot.Extensions;

namespace LGWCP.Godot.Liit;

public partial class FooComponent : NodeComponent<Node>
{
    public override void _Ready()
    {
        IsSubmit = true;
        base._Ready();
    }
}