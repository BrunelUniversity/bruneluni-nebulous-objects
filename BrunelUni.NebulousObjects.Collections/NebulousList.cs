using System.Collections;
using BrunelUni.NebulousObjects.Core.Interfaces.Contract;

namespace BrunelUni.NebulousObjects.Collections;

public class NebulousList<T> : INebulousList<T> where T : BaseNebulousObject
{
    private readonly INebulousManager _nebulousManager;
    private List<T> _list;

    public NebulousList( INebulousManager nebulousManager )
    {
        _nebulousManager = nebulousManager;
        _list = new List<T>();
        IsReadOnly = false;
    }

    public IEnumerator<T> GetEnumerator( )
    {
        var items = new List<T>( );
        foreach( var item in _list )
        {
            _nebulousManager.EnterSharedLock<T>( item.Id );
            items.Add( item.CloneThis<T>( ) );
            _nebulousManager.ExitSharedLock<T>( item.Id );
        }

        return items.GetEnumerator( );
    }

    IEnumerator IEnumerable.GetEnumerator( ) => GetEnumerator( );

    public void Add( T item ) => _list.Add( item );

    public void Clear( ) { throw new NotImplementedException( ); }

    public bool Contains( T item ) { throw new NotImplementedException( ); }

    public void CopyTo( T [ ] array, int arrayIndex ) { throw new NotImplementedException( ); }

    public bool Remove( T item ) { throw new NotImplementedException( ); }

    public int Count { get; }
    public bool IsReadOnly { get; }
    
    public int IndexOf( T item ) { throw new NotImplementedException( ); }

    public void Insert( int index, T item ) { throw new NotImplementedException( ); }

    public void RemoveAt( int index ) { throw new NotImplementedException( ); }

    public T this[ int index ]
    {
        get => throw new NotImplementedException( );
        set => throw new NotImplementedException( );
    }
}