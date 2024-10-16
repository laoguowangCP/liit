using System;
using Godot;
using LGWCP.Godot.Extensions;
using LGWCP.Godot.StatechartSharp;

namespace LGWCP.Godot.Liit;


/// <summary>
/// Hold movement commands, calculate move vel and rotY, send to RotMoveCB3. Connect many thing to statechart.
/// </summary>
public partial class ProcessMotCB3 : NodeComponent<CharacterBody3D>
{
    [Export]
    protected ProcessMotImpl ProcessMot;

    protected float JumpVelocity;

    public Vector2 Dir { get; set; }
    protected Vector3 Vel;
    protected float RotY;
    protected RotMoveCB3 CB3RotMove;
    protected float Gravity;


    public override void _Ready()
    {
        base._Ready();
        CB3RotMove = this.GetMonoSiblingOrNull<RotMoveCB3>();

        JumpVelocity = ProcessMot.JumpVelocity;
        Gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle() * ProcessMot.GravityRatio;
    }

    public void RI_Jump(StatechartDuct duct)
    {
		// Handle Jump.
		if (Input.IsActionJustPressed("Space") && Entity.IsOnFloor())
		{
			Vel.Y = JumpVelocity;
		}
    }

    public void RI_StandWalk(StatechartDuct duct)
	{
        var delta = (float)(duct.PhysicsDelta);

        // Add the gravity.
		Vel.Y = Entity.IsOnFloor() ? 0.0f : Vel.Y - Gravity * delta;
    }

    public void RI_CommitRotMove(StatechartDuct _)
    {
        
        CB3RotMove.Vel = Vel;
    }
}