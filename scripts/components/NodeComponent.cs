using System;
using Godot;

namespace LGWCP.Godot.Liit;


public partial class NodeComponent<T> : Node, IComponent
    where T : Node
{
    /// <summary>
    /// If is submit, component will submit itself as parent node's component, while parent node will be submitted as entity.
    /// </summary>
    [Export]
    protected bool IsSubmit = false;

    [ExportGroup("EventFlag")]
    [Export(PropertyHint.Flags, "Process,Physics Process,Input,Shortcut Input,UnhandledKey Input,Unhandled Input")]
	public EventFlagEnum EventFlag { get; set; } = 0;

    public T Entity { get; protected set; }

    public Type Require()
    {
        return typeof(T);
    }

    public override void _Ready()
    {
        Entity = GetParentOrNull<T>();

        SetProcess(
			EventFlag.HasFlag(EventFlagEnum.Process));
		SetPhysicsProcess(
			EventFlag.HasFlag(EventFlagEnum.PhysicsProcess));
		SetProcessInput(
			EventFlag.HasFlag(EventFlagEnum.Input));
		SetProcessShortcutInput(
			EventFlag.HasFlag(EventFlagEnum.ShortcutInput));
		SetProcessUnhandledKeyInput(
			EventFlag.HasFlag(EventFlagEnum.UnhandledKeyInput));
		SetProcessUnhandledInput(
			EventFlag.HasFlag(EventFlagEnum.UnhandledInput));

        if (Entity is null)
        {
            #if DEBUG
            GD.PushWarning(GetPath(), ": need parent with type ", typeof(T));
            #endif
            return;
        }

        if (IsSubmit)
        {
            ICE.Manager.SubmitComponent<T>(Entity);
        }
    }

    public override void _ExitTree()
    {
        ICE.Manager.TryEraseEntity(Entity);
    }
}

[Flags]
public enum EventFlagEnum
{
    Process = 1,
    PhysicsProcess = 2,
    Input = 4,
    ShortcutInput = 8,
    UnhandledKeyInput = 16,
    UnhandledInput = 32,
}
