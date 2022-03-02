using System.ComponentModel;
using Aidan.Common.Core;
using BrunelUni.NebulousObjects.Core.Dtos;

namespace BrunelUni.NebulousObjects.Core.Interfaces.Contract;

public interface INebulousManager
{
    [ Description( "success if exists, failure if not found" ) ]
    Result EnterListSharedLock<TItem>( );

    [ Description( "success if exists, failure if not found" ) ]
    Result ExitListSharedLock<TItem>( );

    [ Description( "success if exists, failure if not found" ) ]
    Result EnterListExclusiveLock<TItem>( );

    [ Description( "success if exists, failure if not found" ) ]
    Result ExitListExclusiveLock<TItem>( );
    
    [ Description( "success if exists, failure if not found" ) ]
    Result EnterItemExclusiveLock<TItem>( int index );

    [ Description( "success if exists, failure if not found" ) ]
    Result ExitItemExclusiveLock<TItem>( int index );

    [ Description( "success if exists, failure if not found" ) ]
    Result EnterItemSharedLock<TItem>( int index );

    [ Description( "success if exists, failure if not found" ) ]
    Result ExitItemSharedLock<TItem>( int index );

    event Action<OperationDto> OperationAvailable;
    
    void Delete<T>( int index );
    void Update<T>( int index, T @object );
    void Create<T>( T @object );
    
    void ReplicateDelete<T>( int index, INebulousList<T> list );
    void ReplicateUpdate<T>( int index, T @object, INebulousList<T> list );
    void ReplicateCreate<T>( T @object, INebulousList<T> list );
}