using Aidan.Common.Core;

namespace BrunelUni.NebulousObjects.Core.Interfaces.Contract;

public interface INebulousManager
{
    /// <returns>success if item exists, failure if the item cannot be found</returns>
    Result EnterExclusiveLock<TItem>( Guid id );

    /// <returns>success if item exists, failure if the item cannot be found</returns>
    Result ExitExclusiveLock<TItem>( Guid id );

    /// <returns>success if item exists, failure if the item cannot be found</returns>
    Result EnterSharedLock<TItem>( Guid id );

    /// <returns>success if item exists, failure if the item cannot be found</returns>
    Result ExitSharedLock<TItem>( Guid id );
}