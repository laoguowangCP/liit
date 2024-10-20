/*
    An Intuitive Entity-Component framework for godot
       ^         ^      ^
    Non-intrusive: no need to extend every node, no need to migrate every thing into ECS
*/

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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

    protected Dictionary<Node, Dictionary<Type, IComponent>> DictETC = new();
    protected Dictionary<Type, LinkedList<IComponent>> DictTC = new();

    /// <summary>
    /// For component to use, submit itself as component, while submitting its parent node as entity.
    /// </summary>
    /// <typeparam name="TComponent"></typeparam>
    /// <param name="entity"></param>
    /// <returns></returns>
    public bool SubmitComponent<TEntity, TComponent>(TEntity entity)
        where TEntity : Node
        where TComponent : NodeComponent<TEntity, TComponent>
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

        Dictionary<Type, IComponent> dictTC;
        bool isSubmitted = DictETC.TryGetValue(entity, out dictTC);
        
        if (isSubmitted)
        {
            bool isMono = dictTC.TryAdd(typeof(TComponent), component);
            if (isMono)
            {
                // Submit to reversed map
                SubmitComponentReversed<TEntity, TComponent>(component);
            }
            return isMono;
        }
        
        // New submit
        dictTC = new()
        {
            { typeof(TComponent), component }
        };
        DictETC.TryAdd(entity, dictTC);
        SubmitComponentReversed<TEntity, TComponent>(component);
        return true;
    }

    public bool SubmitComponent<TEntity, TComponent>(TComponent component)
        where TEntity : Node
        where TComponent : NodeComponent<TEntity, TComponent>
    {
        if (component is null)
        {
            return false;
        }
        TEntity entity = component.Entity;

        Dictionary<Type, IComponent> dictTC;
        bool isSubmitted = DictETC.TryGetValue(entity, out dictTC);
        
        if (isSubmitted)
        {
            bool isMono = dictTC.TryAdd(typeof(TComponent), component);
            if (isMono)
            {
                // Submit to reversed map
                SubmitComponentReversed<TEntity, TComponent>(component);
            }
            return isMono;
        }
        
        // New submit
        dictTC = new()
        {
            { typeof(TComponent), component }
        };
        DictETC.TryAdd(entity, dictTC);
        SubmitComponentReversed<TEntity, TComponent>(component);
        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected void SubmitComponentReversed<TEntity, TComponent>(TComponent component)
        where TEntity : Node
        where TComponent : NodeComponent<TEntity, TComponent>
    {
        // Submit to reversed map
        LinkedList<IComponent> lstC;
        bool isSubmittedReversed = DictTC.TryGetValue(typeof(TComponent), out lstC);
        if (isSubmittedReversed)
        {
            lstC.AddLast(component);
        }
        else
        {
            lstC = new();
            lstC.AddLast(component);
            DictTC.TryAdd(typeof(TComponent), lstC);
        }

        // Reversed dependence
        component.ComponentLLN = lstC.Last;
    }

    /// <summary>
    /// For any node submitted as entity, get its component with given type, if once submitted
    /// </summary>
    /// <typeparam name="TComponent"></typeparam>
    /// <param name="entity"></param>
    /// <returns></returns>
    public TComponent GetComponent<TEntity, TComponent>(TEntity entity)
        where TEntity : Node
        where TComponent : NodeComponent<TEntity, TComponent>
    {
        if (entity is null)
        {
            return null;
        }

        Dictionary<Type, IComponent> dict;
        DictETC.TryGetValue(entity, out dict);

        if (dict is null)
        {
            return null;
        }

        dict.TryGetValue(typeof(TComponent), out IComponent component);
        if (component is TComponent t)
        {
            return t;
        }
        else
        {
            #if DEBUG
            GD.PushWarning("Type not matched, expect ", typeof(TComponent), "get ", component.GetType());
            #endif
            return null;
        }
    }

    public bool GetComponentAll<TComponent>(out LinkedList<IComponent> components)
    {
        return DictTC.TryGetValue(typeof(TComponent), out components);
    }

    /// <summary>
    /// Erase component from linked list.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TComponent"></typeparam>
    /// <param name="component"></param>
    /// <returns></returns>
    public bool TryEraseComponent<TEntity, TComponent>(TComponent component)
        where TEntity : Node
        where TComponent : NodeComponent<TEntity, TComponent>
    {
        if (component is null)
        {
            return false;
        }

        LinkedListNode<IComponent> llnC = component.ComponentLLN;
        llnC.List.Remove(llnC);
        return true;
    }

    /// <summary>
    /// Erase entity from dictionary. Return false if already erased.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public bool TryEraseEntity<TEntity>(TEntity entity)
        where TEntity : Node
    {
        if (entity is null)
        {
            return false;
        }

        return DictETC.Remove(entity);
    }
}

public interface IComponent
{
    
}

public interface IEntity
{
    
}
