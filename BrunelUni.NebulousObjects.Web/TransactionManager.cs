using System.Collections.Concurrent;
using System.Text;
using BrunelUni.NebulousObjects.Core.Dtos;
using BrunelUni.NebulousObjects.Core.Enums;
using BrunelUni.NebulousObjects.Core.Interfaces.Contract;

namespace BrunelUni.NebulousObjects.Web;

public class TransactionManager : ITransactionManager
{
    private readonly IMessageService _messageService;
    private readonly INebulousObjectManager _nebulousObjectManager;
    private readonly ConcurrentDictionary<Guid, TransactionDto> _transactionDtos = new( );

    public TransactionManager( IMessageService messageService, INebulousObjectManager nebulousObjectManager )
    {
        _messageService = messageService;
        _nebulousObjectManager = nebulousObjectManager;
        _messageService.MessageAvailable += OnMessageAvailable;
        NewTransactionToBeProcessed += OnNewTransactionToBeProcessed;
    }

    public event Action<TransactionDto> NewTransactionToBeProcessed;

    public TransactionDto GetTransactionBeingProccessed => _transactionDtos
        .ToArray( )
        .First( x => x.Value.Status == StatusEnum.Processing )
        .Value;

    private void OnNewTransactionToBeProcessed( TransactionDto transactionDto )
    {
        _transactionDtos[ transactionDto.ID ].Status = StatusEnum.Processing;
    }

    private void OnMessageAvailable( byte [ ] obj )
    {
        var transaction = new TransactionDto( );
        var _16byteHolder = new byte[ 16 ];
        transaction.Index = BitConverter.ToInt16( obj.Skip( 1 ).Take( 2 ).ToArray( ) );
        transaction.Model =
            _nebulousObjectManager.Models[
                Encoding.UTF8.GetString( obj.Skip( 3 ).Take( 16 ).ToArray( ) ).Replace( "\u0000", string.Empty ) ];
        transaction.Status = StatusEnum.Waiting;
        transaction.ID = new Guid( obj.Skip( 19 ).Take( 16 ).ToArray( ) );
        if( obj[ 0 ] == 0x04 || obj[ 0 ] == 0x05 ) transaction.Granularity = LockGranularityEnum.Item;
        switch( obj[ 0 ] )
        {
            case 0x04:
                transaction.Type = LockEnum.Exclsuive;
                break;
            case 0x05:
                transaction.Type = LockEnum.Shared;
                break;
            default:
                throw new ArgumentException( "invalid operation" );
        }

        var noTransactions = _transactionDtos.IsEmpty;
        _transactionDtos.TryAdd( transaction.ID, transaction );
        if( noTransactions )
            NewTransactionToBeProcessed.Invoke( transaction );

        // as many shared locks can be processed as possible
        else if( GetTransactionBeingProccessed.Type == LockEnum.Shared && transaction.Type == LockEnum.Shared )
            NewTransactionToBeProcessed.Invoke( transaction );
    }
}