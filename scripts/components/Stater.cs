using Godot;
using LGWCP.Godot.StatechartSharp;
using LGWCP.Godot.Extensions;

namespace LGWCP.Godot.Liit;


[Tool]
public partial class Stater : NodeC<Node>
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