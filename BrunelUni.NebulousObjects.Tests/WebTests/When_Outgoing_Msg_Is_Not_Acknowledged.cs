using System;
using BrunelUni.NebulousObjects.Core.Dtos;
using BrunelUni.NebulousObjects.Core.Enums;
using BrunelUni.NebulousObjects.Tests.NebulousCollectionTests;
using NSubstitute;
using NUnit.Framework;

namespace BrunelUni.NebulousObjects.Tests.WebTests;

[ TestFixture( OperationEnum.Create ) ]
[ TestFixture( OperationEnum.Delete ) ]
[ TestFixture( OperationEnum.EnterExclusiveLock ) ]
[ TestFixture( OperationEnum.ExitExclusiveLock ) ]
public class When_Outgoing_Msg_Is_Not_Acknowledged : Given_A_NebulousClient
{
    private readonly OperationEnum _operationEnum;

    public When_Outgoing_Msg_Is_Not_Acknowledged( OperationEnum operationEnum ) { _operationEnum = operationEnum; }

    protected override void When( )
    {
        MockMessageService.CurrentTransactionID.Returns( new Guid( GuidString ) );
        MockMessageService.GetOutgoingResponse( )
            .Returns( new [ ] { ( byte )OperationEnum.Nack }, new [ ] { ( byte )OperationEnum.Ack } );
        SUT.Send( new OperationDto
        {
            Operation = _operationEnum,
            Data = new Person( ),
            DataType = typeof( Person ),
            Index = 0
        } );
    }

    [ Test ]
    public void Then_Message_Is_Resent( ) { MockMessageService.Received( 2 ).AddOutgoing( Arg.Any<byte [ ]>( ) ); }
}