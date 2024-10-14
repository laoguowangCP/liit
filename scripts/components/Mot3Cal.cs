using Godot;
using LGWCP.Godot.Extensions;
using LGWCP.Godot.StatechartSharp;

namespace LGWCP.Godot.Liit;


/// <summary>
/// Hold movement commands, calculate move vel and rotY, send to CB3RotMove
/// </summary>
public partial class Mot3Cal : NodeC<CharacterBody3D>
{
    protected CB3RotMove CB3RotMove;
    protected Vector3 Vel;
    protected float RotY;


    public override void _Ready()
    {
        base._Ready();
        CB3RotMove = this.GetMonoSiblingOrNull<CB3RotMove>();
    }

    public void RI_StandWalk(StatechartDuct duct)
	{
        var delta = (float)(duct.PhysicsDelta);
    }

    public void RI_CommitRotMove(StatechartDuct _)
    {
        CB3RotMove.RotYAddup += RotY;
        CB3RotMove.Vel = Vel;
    }
}