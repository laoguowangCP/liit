using System;
using System.Collections.Generic;
using Godot;

namespace LGWCP.Godot.Liit;


[Tool]
public partial class NodeC<T> : Node, IComponent
    where T : Node
{
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

        #if DEBUG
        CheckEntity();
        #endif

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
    }

    #if DEBUG
    public void CheckEntity()
    {
        if (Entity is null)
        {
            GD.PushWarning(GetPath(), ": need parent with type ", typeof(T));
        }
    }
    #endif
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
