using Godot;
using LGWCP.Godot.StatechartSharp;
using LGWCP.Godot.Extensions;

namespace LGWCP.Godot.Liit;

public partial class Stater : NodeComponent<Node, Stater>
{
    protected Statechart _sc;

    public override void _Ready()
    {
        _sc = this.GetMonoOrNull<Statechart>();
        #if DEBUG
        if (_sc is null)
        {
            GD.PushWarning(GetPath(), " has no child statechart.");
        }
        #endif
    }
}