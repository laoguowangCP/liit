using Godot;

namespace LGWCP.Godot.Liit;


/// <summary>
/// Hold move vel and rotY, set to parent CB3.
/// </summary>
public partial class CB3RotMove : NodeComponent<CharacterBody3D>
{
    public Vector3 Vel { get; set; }
    public float RotYAddup { get; set; }

    public override void _Ready()
    {
        base._Ready();
        Vel = Vector3.Zero;
    }

    public override void _PhysicsProcess(double delta)
    {
        Entity.RotateY(RotYAddup);
        Entity.Velocity = Vel;
        Entity.MoveAndSlide();

        // Prepare for next frame
        Vel = Entity.Velocity;
    }
}
