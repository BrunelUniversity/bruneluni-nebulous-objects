using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;
using Aidan.Common.Core.Interfaces.Contract;
using BrunelUni.NebulousObjects.Core.Dtos;
using BrunelUni.NebulousObjects.Core.Enums;
using BrunelUni.NebulousObjects.Core.Interfaces.Contract;

namespace BrunelUni.NebulousObjects.Web;

public class NebulousObjectManager : INebulousObjectManager
{
    private readonly IConfigurationAdapter _configurationAdapter;
    private readonly IMessageService _messageService;

    public NebulousObjectManager( IMessageService messageService,
        IConfigurationAdapter configurationAdapter )
    {
        _messageService = messageService;
        _configurationAdapter = configurationAdapter;
        var modelNamespace = _configurationAdapter.Get<AppOptions>( ).ModelNamespace;
        var foo = "";
        var models = AppDomain.CurrentDomain.GetAssemblies( )
            .SelectMany( x => x.GetTypes( ) )
            .Where( x => x.Namespace == modelNamespace );
        Models = new Dictionary<string, Type>( models.Select( x => new KeyValuePair<string, Type>( x.Name, x ) )
            .ToArray( ) );
        _messageService.MessageAvailable += OnMessageAvailable;
    }

    public event Action<OperationDto> OperationAvailable;

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

    public Dictionary<string, Type> Models { get; }

    private void OnMessageAvailable( byte [ ] obj )
    {
        switch( obj[ 0 ] )
        {
            case 0x02:
                foreach( var model in Models )
                {
                    var bytes = new byte[ 16 ];
                    Encoding.UTF8.GetBytes( model.Value.Name, bytes );
                    var objectName = obj.Skip( 3 ).Take( 16 ).ToArray( );
                    if( bytes.SequenceEqual( objectName ) )
                    {
                        var index = ( int )BitConverter.ToInt16( obj.Skip( 1 ).Take( 2 ).ToArray( ) );
                        OperationAvailable.Invoke( new OperationDto
                        {
                            Operation = OperationEnum.Delete,
                            DataType = model.Value,
                            Index = index
                        } );
                        return;
                    }
                }

                throw new ArgumentException( "model does not exist" );
            default:
                throw new ArgumentException( "invalid operation" );
        }
    }

    public void AckReplication( ) { throw new NotImplementedException( ); }
}