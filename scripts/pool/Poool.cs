using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Godot;


/*
    Poool: extra objects in pool. A simple object pooler.
                 ^          ^^^^
*/

namespace LGWCP.Godot.Poool;

public class Poool<T> where T : new()
{
    private protected readonly Queue<T> _pool = new();
    private protected T _firstInPool;
    private readonly int _sizeMax;
    private int _size;
    private Pooolicy<T> _p;
    private Func<T> _pCreate;
    private Func<T, bool> _pReset;

    public Poool(int sizeInit, int sizeMax, Pooolicy<T> pooolicy)
    {
        _p = pooolicy;
        _pCreate = pooolicy.Create;
        _pReset = pooolicy.Reset;
        _size = Math.Min(sizeInit, sizeMax);
        _sizeMax = sizeMax;

        for (int i = 0; i < _size; ++i)
        {
            var o = _pCreate();
            _pReset(o);
            _pool.Enqueue(o);
        }
    }

    public virtual T Take()
    {
        _pool.TryDequeue(out var o);

        // Pool is empty
        if (o is null)
        {
            return _pCreate();
        }

        --_size;
        return o;
    }

    public virtual void GiveBack(T o)
    {
        // Pool is filled
        if (_size >= _sizeMax)
        {
            return;
        }

        // Reset and back to pool
        if (_pReset(o))
        {
            _pool.Enqueue(o);
            ++_size;
        }
    }
}


/*
    Pooolicy: policy for Poool.
*/

public class Pooolicy<T> where T : new()
{
    public virtual T Create() { return new T(); }

    /// <summary>
    ///  Reset before give back. Do nothing by default.
    /// </summary>
    public virtual bool Reset(T obj) { return true; }
}


