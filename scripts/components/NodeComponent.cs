using System;
using System.Collections.Generic;
using Godot;

namespace LGWCP.Godot.Liit;


public partial class NodeComponent<TEntity, TComponent> : Node, IComponent
    where TEntity : Node
    where TComponent : NodeComponent<TEntity, TComponent>
{
    /// <summary>
    /// If true, component will submit itself as parent node's component, while parent node will be submitted as entity.
    /// </summary>
    [Export]
    protected bool IsSubmit = false;

    /// <summary>
    /// If true, component will desubmit parent node entity.
    /// </summary>
    [Export]
    protected bool IsEraseEntityOnLeave = false;


    // TODO: not that useful, may delete
    /*
    [ExportGroup("EventFlag")]
    [Export(PropertyHint.Flags, "Process,Physics Process,Input,Shortcut Input,UnhandledKey Input,Unhandled Input")]
    public EventFlagEnum EventFlag { get; set; } = 0;
    */
    public TEntity Entity { get; protected set; }

    /// <summary>
    /// Linked list node reference of this component in all components. Used for fast remove.
    /// </summary>
    public LinkedListNode<IComponent> ComponentLLN;

    public Type Require()
    {
        return typeof(TEntity);
    }

    public override void _Ready()
    {
        Entity = GetParentOrNull<TEntity>();

        // TODO: not that useful, may delete
        /*
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
        */

        if (Entity is null)
        {
            #if DEBUG
            GD.PushWarning(GetPath(), ": need parent entity with type ", typeof(TEntity));
            #endif
            return;
        }

        if (IsSubmit)
        {
            ICE.Manager.SubmitComponent<TEntity, TComponent>((TComponent)this);
        }
    }

    public override void _ExitTree()
    {
        if (IsSubmit)
        {
            ICE.Manager.TryEraseComponent<TEntity, TComponent>((TComponent)this);
        }
            ICE.Manager.TryEraseEntity<TEntity>(Entity);
    }
}



// TODO: not that useful, may delete
/*
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
*/
