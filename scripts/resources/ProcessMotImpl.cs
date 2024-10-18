using Godot;


namespace LGWCP.Godot.Liit;

[GlobalClass]
public partial class ProcessMotImpl : Resource
{
    [ExportGroup("Motion Parameter")]
    [Export] public float JumpVelocity = 5.0f;
    [Export] public float GravityRatio = 1.0f;

    [ExportGroup("Input Action Name")]
    [Export] public StringName ActionJump = "Space";
}