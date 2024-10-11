using Godot;

namespace LGWCP.Godot.Liit;


public partial class CB3Veler : CNode<CharacterBody3D>
{
    protected CharacterBody3D CB3;
    public Vector3 Vel { get; set; }


    public override void _Ready()
    {
        CB3 = ENode as CharacterBody3D;
    }
}
