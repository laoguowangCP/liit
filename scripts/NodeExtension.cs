using System.Linq;
using Godot;

namespace LGWCP.Godot.Extensions;

public static class NodeExtension
{
    /// <summary>
    /// Return "mono", first child of this node with given type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="node"></param>
    /// <returns></returns>
    public static T GetMonoOrNull<T>(this Node node)
        where T : Node
    {
        if (node is null)
        {
            #if DEBUG
            GD.PushWarning("Node is null.");
            #endif
            return null;
        }

        T mono = null;
        var children = node.GetChildren().ToArray();

        #if DEBUG
        foreach (var child in children)
        {
            if (child is T t)
            {
                if (mono is not null)
                {
                    GD.PushWarning(node.GetPath(), " has multiple child with type ", typeof(T));
                    break;
                }
                mono = t;
            }
        }

        if (mono is null)
        {
            GD.PushWarning(node.GetPath(), " no child with type ", typeof(T));
        }
        return mono;

        #else

        foreach (var child in children)
        {
            if (child is T t)
            {
                mono = t;
                break;
            }
        }

        return mono;

        #endif
    }

    public static bool HasMono<T>(this Node node)
        where T : Node
    {
        return false;
    }

    /// <summary>
    /// Return sibling "mono", first sibling of this node with given type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="node"></param>
    /// <returns></returns>
    public static T GetMonoSiblingOrNull<T>(this Node node)
        where T : Node
    {
        if (node is null)
        {
            #if DEBUG
            GD.PushWarning("Node is null.");
            #endif
            return null;
        }
        
        var parent = node.GetParentOrNull<Node>();
        if (parent is null)
        {
            #if DEBUG
            GD.PushWarning(node.GetPath(), " has no parent.");
            #endif
            return null;
        }

        T mono = null;
        var children = parent.GetChildren().ToArray();

        #if DEBUG
        // In debug, also check sibling is mono
        foreach (var child in children)
        {
            if (child != node && child is T t)
            {
                if (mono is not null)
                {
                    GD.PushWarning(node.GetPath(), " has multiple siblings with type ", typeof(T));
                    break;
                }
                mono = t;
            }
        }

        if (mono is null)
        {
            GD.PushWarning(node.GetPath(), " no siblings with type ", typeof(T));
        }

        return mono;

        #else

        foreach (var child in children)
        {
            if (child != node && child is T t)
            {
                mono = t;
                break;
            }
        }

        return mono;

        #endif
    }

    public static bool HasMonoSibling<T>(this Node node)
        where T : Node
    {
        var parent = node.GetParentOrNull<Node>();
        if (parent is null)
        {
            #if DEBUG
            GD.PushWarning(node.GetPath(), " has no parent.");
            #endif
            return false;
        }

        var children = parent.GetChildren().ToArray();

        foreach (var child in children)
        {
            if (child != node && child is T _)
            {
                return true;
            }
        }

        return false;
    }
}