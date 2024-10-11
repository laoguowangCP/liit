using System;
using System.Collections.Generic;
using Godot;

namespace LGWCP.Godot.Liit;


[Tool]
public partial class CNode<T> : Node, IComponent
    where T : Node
{
    protected Node ENode;

    public Type Require()
    {
        return typeof(T);
    }

    public override void _Ready()
    {
        ENode = this;
        do
        {
            ENode = ENode.GetParentOrNull<Node>();
        } while (ENode is not null && ENode is IComponent);

        #if DEBUG
        if (Engine.IsEditorHint())
        {
            UpdateConfigurationWarnings();
        }
        #endif
    }


    #if DEBUG
    public override string[] _GetConfigurationWarnings()
    {
        var warnings = new List<string>();

        if (ENode is null)
        {
            warnings.Add("CNode needs ancestor non-cnode.");
        }
        else if (ENode is not T)
        {
            warnings.Add("CNode needs ancestor with type: " + typeof(T).ToString());
        }

        return warnings.ToArray();
    }
    #endif
}
