using Godot;
using LGWCP.Godot.Extensions;

namespace LGWCP.Godot.Liit;

public partial class BarComponent : NodeComponent<Node, BarComponent>
{
    public override void _Ready()
    {
        IsSubmit = true;
        base._Ready();
    }
}