using Godot;

namespace LGWCP.Godot.Liit;


public partial class CB3Mover : NodeC<CharacterBody3D>
{
    public Vector3 Vel { get; set; }

    public override void _Ready()
    {
        Vel = Vector3.Zero;
    }

    public override void _PhysicsProcess(double delta)
    {
        Entity.Velocity = Vel;
        Entity.MoveAndSlide();
    }
}
