using System;
using System.Collections.Generic;
using Godot;

namespace LGWCP.Godot.Liit;


[Tool]
public partial class NodeC<T> : Node, IComponent
    where T : Node
{
    public T Entity { get; protected set; }

    public Type Require()
    {
        return typeof(T);
    }

    public override void _Ready()
    {
        Entity = GetParentOrNull<T>();

        #if TOOLS
        if (Engine.IsEditorHint())
        {
            UpdateConfigurationWarnings();
            RequestReady();
        }
        #endif
    }


    #if TOOLS
    public override string[] _GetConfigurationWarnings()
    {
        List<string> warnings = new List<string>();

        if (Entity is null)
        {
            warnings.Add("CNode needs parent non-cnode.");
        }
        else if (Entity is not T _)
        {
            warnings.Add("CNode needs ancestor with type: " + typeof(T).ToString());
        }

        if (warnings.Count > 0)
        {
            warnings.Add("To use custom class as parent entity, set it tool and globalclass");
        }

        return warnings.ToArray();
    }
    #endif
}
