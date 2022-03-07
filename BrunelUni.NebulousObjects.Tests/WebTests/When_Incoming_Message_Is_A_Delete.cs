using System;
using System.Linq;
using BrunelUni.NebulousObjects.Collections;
using BrunelUni.NebulousObjects.Core.Dtos;
using BrunelUni.NebulousObjects.Core.Enums;
using NSubstitute;
using NUnit.Framework;

namespace BrunelUni.NebulousObjects.Tests.WebTests;

public class When_Incoming_Message_Is_A_Delete : Given_A_NebulousClient
{
    private OperationDto _proccessedOperation;

    protected override void When( )
    {
        SUT.OperationAvailable += dto => _proccessedOperation = dto;
        MockMessageService.MessageAvailable += Raise.Event<Action<byte [ ]>>( new byte [ ]
        {
            0x02, 0x01, 0x00
        }.Concat( TestHelpers.ObjectBytes ).ToArray( ) );
    }

    [ Test ]
    public void Then_Delete_Operation_Event_Is_Raised( )
    {
        Assert.True( _proccessedOperation.NebulousEquals( new OperationDto
        {
            DataType = typeof( Person ),
            Index = 1,
            Operation = OperationEnum.Delete
        } ) );
    }
}