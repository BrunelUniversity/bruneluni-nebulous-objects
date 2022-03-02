using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;
using BrunelUni.NebulousObjects.Core.Dtos;
using BrunelUni.NebulousObjects.Core.Enums;
using BrunelUni.NebulousObjects.Core.Interfaces.Contract;

namespace BrunelUni.NebulousObjects.Web;

public class NebulousClient : INebulousClient
{
    private readonly IMessageService _messageService;

    public NebulousClient( IMessageService messageService ) { _messageService = messageService; }

    public event Action<OperationDto> MessageAvailable;

    public void Send( OperationDto operationDto )
    {
        switch( operationDto.Operation )
        {
            case OperationEnum.Create:
            case OperationEnum.Update:
                var type = operationDto.Data.GetType( );
                var props = type.GetProperties( );
                var propBytes = new List<byte>( );
                propBytes.Add( ( byte )operationDto.Operation );
                var indexBytes = BitConverter.GetBytes( operationDto.Index );
                propBytes.AddRange( indexBytes.Take( 2 ) );
                var bytes = new byte[ 16 ];
                Encoding.UTF8.GetBytes( type.Name, bytes );
                propBytes.AddRange( bytes );
                foreach( var prop in props )
                {
                    var propertyType = prop.PropertyType;
                    if( propertyType == typeof( Guid ) )
                    {
                        propBytes.AddRange( ( ( Guid )prop.GetValue( operationDto.Data ) ).ToByteArray( ) );
                    }
                    else if( propertyType == typeof( string ) )
                    {
                        var stringLength = prop.GetCustomAttribute<MaxLengthAttribute>( ).Length;
                        var bytesString = new byte[ stringLength ];
                        Encoding.UTF8.GetBytes( ( string )prop.GetValue( operationDto.Data ), bytesString );
                        propBytes.AddRange( bytesString );
                    }
                }

                _messageService.AddOutgoing( propBytes.ToArray( ) );
                break;
            default:
                throw new ArgumentException( "enum value invalid" );
        }
    }

    public void Ack( ) { throw new NotImplementedException( ); }
}