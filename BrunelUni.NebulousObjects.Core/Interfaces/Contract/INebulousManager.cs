using Aidan.Common.Core;

namespace BrunelUni.NebulousObjects.Core.Interfaces.Contract;

public interface INebulousManager
{
    /// <returns>success if item exists, failure if the item cannot be found</returns>
    Result EnterListSharedLock<TItem>( );

    /// <returns>success if item exists, failure if the item cannot be found</returns>
    Result ExitListSharedLock<TItem>( );

    /// <returns>success if item exists, failure if the item cannot be found</returns>
    Result EnterListExclusiveLock<TItem>( );

    /// <returns>success if item exists, failure if the item cannot be found</returns>
    Result ExitListExclusiveLock<TItem>( );
    
    /// <returns>success if item exists, failure if the item cannot be found</returns>
    Result EnterItemExclusiveLock<TItem>( int index );

    /// <returns>success if item exists, failure if the item cannot be found</returns>
    Result ExitItemExclusiveLock<TItem>( int index );

    /// <returns>success if item exists, failure if the item cannot be found</returns>
    Result EnterItemSharedLock<TItem>( int index );

    /// <returns>success if item exists, failure if the item cannot be found</returns>
    Result ExitItemSharedLock<TItem>( int index );
}