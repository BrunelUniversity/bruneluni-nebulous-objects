using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;
using BrunelUni.NebulousObjects.Core.Dtos;
using BrunelUni.NebulousObjects.Core.Enums;
using BrunelUni.NebulousObjects.Core.Interfaces.Contract;

namespace BrunelUni.NebulousObjects.Web;

public class NebulousObjectManager : INebulousObjectManager
{
    private readonly IMessageService _messageService;

    public NebulousObjectManager( IMessageService messageService ) { _messageService = messageService; }

    public event Action<OperationDto> MessageAvailable;

    public void Send( OperationDto operationDto )
    {
        var allBytes = new List<byte>( );
        switch( operationDto.Operation )
        {
            case OperationEnum.EnterExclusiveLock:
            case OperationEnum.EnterExclusiveListLock:
            case OperationEnum.EnterSharedLock:
                _messageService.CurrentTransactionID = Guid.NewGuid( );
                break;
            case OperationEnum.ExitExclusiveLock:
            case OperationEnum.ExitSharedLock:
            case OperationEnum.ExitExclusiveListLock:
                allBytes.Add( ( byte )operationDto.Operation );
                allBytes.AddRange( _messageService.CurrentTransactionID.Value.ToByteArray( ) );
                _messageService.CurrentTransactionID = null;
                break;
            case OperationEnum.Delete:
                var typeUpdate = operationDto.DataType;
                allBytes.Add( ( byte )operationDto.Operation );
                var indexBytesUpdate = BitConverter.GetBytes( operationDto.Index );
                allBytes.AddRange( indexBytesUpdate.Take( 2 ) );
                var bytesUpdate = new byte[ 16 ];
                Encoding.UTF8.GetBytes( typeUpdate.Name, bytesUpdate );
                allBytes.AddRange( bytesUpdate );
                break;
            case OperationEnum.Create:
            case OperationEnum.Update:
                var type = operationDto.Data.GetType( );
                var props = type.GetProperties( );
                allBytes.Add( ( byte )operationDto.Operation );
                var indexBytes = BitConverter.GetBytes( operationDto.Index );
                allBytes.AddRange( indexBytes.Take( 2 ) );
                var bytes = new byte[ 16 ];
                Encoding.UTF8.GetBytes( type.Name, bytes );
                allBytes.AddRange( bytes );
                foreach( var prop in props )
                {
                    var propertyType = prop.PropertyType;
                    if( propertyType == typeof( Guid ) )
                    {
                        allBytes.AddRange( ( ( Guid )prop.GetValue( operationDto.Data ) ).ToByteArray( ) );
                    }
                    else if( propertyType == typeof( string ) )
                    {
                        var stringLength = prop.GetCustomAttribute<MaxLengthAttribute>( ).Length;
                        var bytesString = new byte[ stringLength ];
                        Encoding.UTF8.GetBytes( ( string )prop.GetValue( operationDto.Data ), bytesString );
                        allBytes.AddRange( bytesString );
                    }
                }

                break;
            default:
                throw new ArgumentException( "enum value invalid" );
        }

        _messageService.AddOutgoing( allBytes.ToArray( ) );
    }

    public void AckReplication( ) { throw new NotImplementedException( ); }
}