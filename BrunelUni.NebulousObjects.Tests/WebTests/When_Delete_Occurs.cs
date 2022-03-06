using System.Linq;
using BrunelUni.NebulousObjects.Core.Dtos;
using BrunelUni.NebulousObjects.Core.Enums;
using BrunelUni.NebulousObjects.Tests.NebulousCollectionTests;
using NSubstitute;
using NUnit.Framework;

namespace BrunelUni.NebulousObjects.Tests.WebTests;

public class When_Delete_Occurs : Given_A_NebulousClient
{
    protected override void When( )
    {
        SUT.Send( new OperationDto
        {
            Operation = OperationEnum.Delete,
            Index = 1,
            DataType = typeof( Person )
        } );
    }


    [ Test ]
    public void Then_Message_Is_Sent( )
    {
        var operationBytes = new byte [ ]
        {
            0x02
        };
        var indexBytes = new byte [ ]
        {
            0x01, 0x00
        };
        var bytes = operationBytes
            .Concat( indexBytes )
            .Concat( ObjectBytes )
            .ToArray( );
        MockMessageService.Received( 1 ).AddOutgoing( Arg.Any<byte [ ]>( ) );
        MockMessageService.Received( ).AddOutgoing( Arg.Is<byte [ ]>( b => b.SequenceEqual( bytes ) ) );
    }
}