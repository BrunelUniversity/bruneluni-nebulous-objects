using BrunelUni.NebulousObjects.Core.Dtos;
using BrunelUni.NebulousObjects.Core.Enums;
using BrunelUni.NebulousObjects.Core.Interfaces.Contract;

namespace BrunelUni.NebulousObjects.Web;

public class NebulousManager : INebulousManager
{
    private readonly INebulousClient _nebulousClient;

    public NebulousManager( INebulousClient nebulousClient ) { _nebulousClient = nebulousClient; }

    public void EnterListSharedLock<TItem>( )
    {
        _nebulousClient.Send( new OperationDto
        {
            Operation = OperationEnum.EnterSharedListLock,
            DataType = typeof( TItem )
        } );
    }

    public void ExitListSharedLock<TItem>( )
    {
        _nebulousClient.Send( new OperationDto
        {
            Operation = OperationEnum.ExitSharedListLock,
            DataType = typeof( TItem )
        } );
    }

    public void EnterListExclusiveLock<TItem>( )
    {
        _nebulousClient.Send( new OperationDto
        {
            Operation = OperationEnum.EnterExclusiveListLock,
            DataType = typeof( TItem )
        } );
    }

    public void ExitListExclusiveLock<TItem>( )
    {
        _nebulousClient.Send( new OperationDto
        {
            Operation = OperationEnum.ExitExclusiveListLock,
            DataType = typeof( TItem )
        } );
    }

    public void EnterItemExclusiveLock<TItem>( int index )
    {
        _nebulousClient.Send( new OperationDto
        {
            Operation = OperationEnum.EnterExclusiveLock,
            DataType = typeof( TItem ),
            Index = index
        } );
    }

    public void ExitItemExclusiveLock<TItem>( int index )
    {
        _nebulousClient.Send( new OperationDto
        {
            Operation = OperationEnum.ExitExclusiveLock,
            DataType = typeof( TItem ),
            Index = index
        } );
    }

    public void EnterItemSharedLock<TItem>( int index )
    {
        _nebulousClient.Send( new OperationDto
        {
            Operation = OperationEnum.EnterSharedLock,
            DataType = typeof( TItem ),
            Index = index
        } );
    }

    public void ExitItemSharedLock<TItem>( int index )
    {
        _nebulousClient.Send( new OperationDto
        {
            Operation = OperationEnum.ExitSharedLock,
            DataType = typeof( TItem ),
            Index = index
        } );
    }

    public event Action<OperationDto>? OperationAvailable;
    public void Delete<T>( int index ) { throw new NotImplementedException( ); }

    public void Update<T>( int index, T @object ) { throw new NotImplementedException( ); }

    public void Create<T>( T @object ) { throw new NotImplementedException( ); }

    public void ReplicateDelete<T>( int index, INebulousList<T> list ) { throw new NotImplementedException( ); }

    public void ReplicateUpdate<T>( int index, T @object, INebulousList<T> list )
    {
        throw new NotImplementedException( );
    }

    public void ReplicateCreate<T>( T @object, INebulousList<T> list ) { throw new NotImplementedException( ); }
}