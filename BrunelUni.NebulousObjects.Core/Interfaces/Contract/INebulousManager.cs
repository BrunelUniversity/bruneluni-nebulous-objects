using BrunelUni.NebulousObjects.Core.Dtos;

namespace BrunelUni.NebulousObjects.Core.Interfaces.Contract;

public interface INebulousManager
{
    void EnterListSharedLock<TItem>( );

    void ExitListSharedLock<TItem>( );

    void EnterListExclusiveLock<TItem>( );

    void ExitListExclusiveLock<TItem>( );

    void EnterItemExclusiveLock<TItem>( int index );

    void ExitItemExclusiveLock<TItem>( int index );


    void EnterItemSharedLock<TItem>( int index );


    void ExitItemSharedLock<TItem>( int index );

    event Action<OperationDto> OperationAvailable;

    void Delete<T>( int index );
    void Update<T>( int index, T @object );
    void Create<T>( T @object );

    void ReplicateDelete<T>( int index, INebulousList<T> list );
    void ReplicateUpdate<T>( int index, T @object, INebulousList<T> list );
    void ReplicateCreate<T>( T @object, INebulousList<T> list );
}