using System.Collections;
using BrunelUni.NebulousObjects.Core.Interfaces.Contract;

namespace BrunelUni.NebulousObjects.Collections;

public class NebulousList<T> : INebulousList<T>
{
    private readonly INebulousManager _nebulousManager;
    private readonly SynchronizedCollection<T> _list;

    public NebulousList( INebulousManager nebulousManager, params T[] items )
    {
        _nebulousManager = nebulousManager;
        _list = new SynchronizedCollection<T>( );
        foreach( var item in items )
        {
            _list.Add( item );
        }
        IsReadOnly = false;
    }

    public IEnumerator<T> GetEnumerator( )
    {
        var items = new List<T>( );
        for( var index = 0; index < _list.Count; index++ )
        {
            var item = _list[ index ];
            _nebulousManager.EnterItemSharedLock<T>( index );
            items.Add( item.Clone( ) );
            _nebulousManager.ExitItemSharedLock<T>( index );
        }

        return items.GetEnumerator( );
    }

    public void ReplaceFirstOccurance( Func<T, bool> predicate, T replacement )
    {
        var first = this.FirstOrDefault( predicate );
        var index = IndexOf( first );
        try
        {
            _nebulousManager.EnterItemExclusiveLock<T>( IndexOf( first ) );
            _list[ index ] = replacement;
        }
        finally
        {
            _nebulousManager.ExitItemExclusiveLock<T>( index );
        }
    }

    IEnumerator IEnumerable.GetEnumerator( ) => GetEnumerator( );

    public void Add( T item )
    {
        _list.Add( item );
    }
    
    public void Clear( ) { throw new NotImplementedException( ); }
    
    public bool Contains( T item ) { throw new NotImplementedException( ); }

    public void CopyTo( T [ ] array, int arrayIndex ) { throw new NotImplementedException( ); }

    public bool Remove( T item ) { throw new NotImplementedException( ); }

    public int Count => _list.Count;
    public bool IsReadOnly { get; }

    public int IndexOf( T item )
    {
        using( IEnumerator<T> enumerator = GetEnumerator( ) )
        {
            var index = 0;
            while( enumerator.MoveNext( ) )
            {
                if( enumerator.Current.NebulousEquals( item ) )
                {
                    return index;
                }
                index++;
            }

            throw new ArgumentException( "item does not exist" );
        }
    }

    public void Insert( int index, T item ) { throw new NotImplementedException( ); }

    public void RemoveAt( int index ) { throw new NotImplementedException( ); }

    public T this[ int index ]
    {
        get
        {
            _nebulousManager.EnterItemSharedLock<T>( index );
            var item =  _list[ index ].Clone( );
            _nebulousManager.ExitItemSharedLock<T>( index );
            return item;
        }
        set => throw new NotImplementedException( );
    }
}