using System.Collections;
using Aidan.Common.Core.Attributes;
using BrunelUni.NebulousObjects.Core.Dtos;
using BrunelUni.NebulousObjects.Core.Enums;
using BrunelUni.NebulousObjects.Core.Interfaces.Contract;

namespace BrunelUni.NebulousObjects.Collections;

[ ThreadSafe ]
public class NebulousList<T> : INebulousList<T> where T : class
{
    private readonly SynchronizedCollection<T> _list;
    private readonly INebulousClient _nebulousClient;
    private readonly object _nessMonster;

    public NebulousList( INebulousClient nebulousClient, params T [ ] items )
    {
        _nebulousClient = nebulousClient;
        _nessMonster = new object( );
        _nebulousClient.MessageAvailable += dto =>
        {
            lock( _nessMonster )
            {
                switch( dto.Operation )
                {
                    case OperationEnum.Create:
                        _list.Add( dto.Data as T );
                        _nebulousClient.AckReplication( );
                        break;
                    case OperationEnum.Delete:
                        _list.RemoveAt( dto.Index );
                        _nebulousClient.AckReplication( );
                        break;
                    case OperationEnum.Update:
                        _list[ dto.Index ] = ( dto.Data as T ).Clone( );
                        _nebulousClient.AckReplication( );
                        break;
                    default:
                        throw new ArgumentOutOfRangeException( $"{dto.Operation} is an illegal operation here" );
                }
            }
        };
        _list = new SynchronizedCollection<T>( );
        foreach( var item in items ) { _list.Add( item ); }

        IsReadOnly = false;
    }

    public IEnumerator<T> GetEnumerator( )
    {
        var items = new List<T>( );
        for( var index = 0; index < _list.Count; index++ )
        {
            lock( _nessMonster )
            {
                var item = _list[ index ];
                _nebulousClient.Send( new OperationDto
                {
                    Index = index,
                    Operation = OperationEnum.EnterSharedLock
                } );
                items.Add( item.Clone( ) );
                _nebulousClient.Send( new OperationDto
                {
                    Index = index,
                    Operation = OperationEnum.ExitSharedLock
                } );
            }
        }

        return items.GetEnumerator( );
    }

    public void ReplaceFirstOccurance( Func<T, bool> predicate, T replacement )
    {
        var first = this.FirstOrDefault( predicate );
        var index = IndexOf( first );
        lock( _nessMonster )
        {
            try
            {
                _nebulousClient.Send( new OperationDto
                {
                    Index = index,
                    Operation = OperationEnum.EnterExclusiveLock
                } );
                _nebulousClient.Send( new OperationDto
                {
                    Index = index,
                    Operation = OperationEnum.Update,
                    Data = replacement
                } );
                _list[ index ] = replacement;
            }
            finally
            {
                _nebulousClient.Send( new OperationDto
                {
                    Index = index,
                    Operation = OperationEnum.ExitExclusiveLock
                } );
            }
        }
    }

    public void Add( T item )
    {
        lock( _nessMonster )
        {
            _nebulousClient.Send( new OperationDto
            {
                Operation = OperationEnum.EnterExclusiveListLock
            } );
            _nebulousClient.Send( new OperationDto
            {
                Operation = OperationEnum.Create,
                Data = item
            } );
            _list.Add( item );
            _nebulousClient.Send( new OperationDto
            {
                Operation = OperationEnum.ExitExclusiveListLock
            } );
        }
    }

    public void RemoveAt( int index )
    {
        lock( _nessMonster )
        {
            _nebulousClient.Send( new OperationDto
            {
                Operation = OperationEnum.EnterExclusiveListLock
            } );
            _nebulousClient.Send( new OperationDto
            {
                Operation = OperationEnum.Delete,
                Index = index,
                DataType = typeof( T )
            } );
            _list.RemoveAt( index );
            _nebulousClient.Send( new OperationDto
            {
                Operation = OperationEnum.ExitExclusiveListLock
            } );
        }
    }

    public T this[ int index ]
    {
        get
        {
            lock( _nessMonster )
            {
                _nebulousClient.Send( new OperationDto
                {
                    Index = index,
                    Operation = OperationEnum.EnterSharedLock
                } );
                var item = _list[ index ].Clone( );
                _nebulousClient.Send( new OperationDto
                {
                    Index = index,
                    Operation = OperationEnum.ExitSharedLock
                } );
                return item;
            }
        }
        set => throw new NotImplementedException( );
    }

    IEnumerator IEnumerable.GetEnumerator( ) { return GetEnumerator( ); }

    public void Clear( ) { throw new NotImplementedException( ); }

    public bool Contains( T item ) { throw new NotImplementedException( ); }

    public void CopyTo( T [ ] array, int arrayIndex ) { throw new NotImplementedException( ); }

    public bool Remove( T item )
    {
        try
        {
            var index = IndexOf( item );
            RemoveAt( index );
            return true;
        }
        catch( Exception ) { return false; }
    }

    public int Count => _list.Count;

    public bool IsReadOnly { get; }

    public int IndexOf( T item )
    {
        using( var enumerator = GetEnumerator( ) )
        {
            var index = 0;
            while( enumerator.MoveNext( ) )
            {
                if( enumerator.Current.NebulousEquals( item ) ) return index;

                index++;
            }

            throw new ArgumentException( "item does not exist" );
        }
    }

    public void Insert( int index, T item ) { throw new NotImplementedException( ); }
}