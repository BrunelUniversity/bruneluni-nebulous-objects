using System;
using BrunelUni.NebulousObjects.Core.Dtos;
using BrunelUni.NebulousObjects.Core.Enums;
using NSubstitute;
using NUnit.Framework;

namespace BrunelUni.NebulousObjects.Tests.WebTests;

public class When_Enter_Lock_Occurs : Given_A_NebulousClient
{
    protected override void When( )
    {
        SUT.Send( new OperationDto
        {
            Operation = OperationEnum.EnterExclusiveListLock
        } );
    }

    [ Test ]
    public void Then_New_Transaction_Id_Is_Set_Once( )
    {
        MockMessageService.Received( 1 ).CurrentTransactionID = Arg.Any<Guid?>( );
        MockMessageService.Received( ).CurrentTransactionID = Arg.Is<Guid?>( x => x != null );
    }
}