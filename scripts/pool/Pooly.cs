using System;
using System.Collections.Generic;


/*
    Pooly: simply object pool.
                ^        ^^^^
*/

namespace LGWCP.Godot.Pooly;

public class Pooly<T> where T : new()
{
    private protected readonly Queue<T> _pool = new();
    private protected T _firstInPool;
    private readonly int _sizeMax;
    private int _size;
    private Func<T> _pCreate;
    private Func<T, bool> _pReset;

    public Pooly(int sizeInit, int sizeMax, Poolicy<T> pooolicy)
    {
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

public class Poolicy<T> where T : new()
{
    public virtual T Create() { return new T(); }

    /// <summary>
    ///  Reset before give back. Do nothing by default.
    /// </summary>
    public virtual bool Reset(T obj) { return true; }
}


