using System;
using System.Linq;
using BrunelUni.NebulousObjects.Core.Dtos;
using BrunelUni.NebulousObjects.Core.Enums;
using NSubstitute;
using NUnit.Framework;

namespace BrunelUni.NebulousObjects.Tests.WebTests;

[ TestFixture( OperationEnum.ExitExclusiveLock, ( byte )0x07 ) ]
[ TestFixture( OperationEnum.ExitSharedLock, ( byte )0x08 ) ]
[ TestFixture( OperationEnum.ExitExclusiveListLock, ( byte )0x06 ) ]
public class When_Exit_Lock_Occurs : Given_A_NebulousClient
{
    private readonly OperationEnum _operationEnum;
    private readonly byte _operationByte;

    public When_Exit_Lock_Occurs( OperationEnum operationEnum, byte operationByte )
    {
        _operationEnum = operationEnum;
        _operationByte = operationByte;
    }

    protected override void When( )
    {
        MockMessageService.GetOutgoingResponse( ).Returns( AckBytes );
        MockMessageService.CurrentTransactionID.Returns( new Guid( GuidString ) );
        SUT.Send( new OperationDto
        {
            Operation = OperationEnum.EnterExclusiveListLock
        } );
    }

    [ Test ]
    public void Then_New_Transaction_Id_Is_Cleared_Once( )
    {
        MockMessageService.Received( 1 ).CurrentTransactionID = Arg.Any<Guid?>( );
        MockMessageService.Received( ).CurrentTransactionID = null;
    }

    [ Test ]
    public void Then_Message_Is_Sent( )
    {
        var operationBytes = new [ ]
        {
            _operationByte
        };
        var bytes = operationBytes
            .Concat( GuidBytes )
            .ToArray( );
        MockMessageService.Received( 1 ).AddOutgoing( Arg.Any<byte [ ]>( ) );
        MockMessageService.Received( ).AddOutgoing( Arg.Is<byte [ ]>( b => b.SequenceEqual( bytes ) ) );
    }
}