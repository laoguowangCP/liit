/*
    An Intuitive Entity-Component framework for godot
       ^         ^      ^
    Non-intrusive: no need to extend every node, no need to migrate every thing into ECS
*/

using System;
using System.Collections.Generic;
using Godot;
using LGWCP.Godot.Extensions;


namespace LGWCP.Godot.Liit;

public class ICE
{
    public static ICE Manager
    {
        get
        {
            if (_manager is null)
            {
                _manager = new();
            }
            return _manager;
        }
    }
    private static ICE _manager;

    protected Dictionary<Node, Dictionary<Type, Node>> EntityComponentDict = new();

    /// <summary>
    /// For component to use, submit itself as component, while submitting its parent node as entity.
    /// </summary>
    /// <typeparam name="TComponent"></typeparam>
    /// <param name="entity"></param>
    /// <returns></returns>
    public bool SubmitComponent<TComponent>(Node entity)
        where TComponent : Node
    {
        if (entity is null)
        {
            return false;
        }
        
        var component = entity.GetMonoOrNull<TComponent>();
        if (component is null)
        {
            return false;
        }

        Dictionary<Type, Node> dict;
        bool isSubmitted = EntityComponentDict.TryGetValue(entity, out dict);
        
        if (isSubmitted)
        {
            return dict.TryAdd(typeof(TComponent), component);
        }
        
        // New submit
        dict = new()
        {
            { typeof(TComponent), component }
        };
        EntityComponentDict.TryAdd(entity, dict);
        return true;
    }

    /// <summary>
    /// For any node submitted as entity, get its component with given type, if once submitted
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="entity"></param>
    /// <returns></returns>
    public T GetComponent<T>(Node entity)
        where T : Node
    {
        if (entity is null)
        {
            return null;
        }

        Dictionary<Type, Node> dict;
        EntityComponentDict.TryGetValue(entity, out dict);

        if (dict is null)
        {
            return null;
        }

        dict.TryGetValue(typeof(T), out Node node);
        if (node is T component)
        {
            return component;
        }
        else
        {
            #if DEBUG
            GD.PushWarning("Type not matched, expect ", typeof(T), "get ", node.GetType());
            #endif
            return null;
        }
    }

    /// <summary>
    /// Erase entity from dictionary. Return false if already erased.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public bool TryEraseEntity(Node entity)
    {
        if (entity is null)
        {
            return false;
        }
        return EntityComponentDict.Remove(entity);
    }
}

public interface IComponent
{
    
}

public interface IEntity
{
    
}
