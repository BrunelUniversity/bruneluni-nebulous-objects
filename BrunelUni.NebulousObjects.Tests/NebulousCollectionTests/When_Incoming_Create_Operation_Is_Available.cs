using System;
using BrunelUni.NebulousObjects.Core.Dtos;
using BrunelUni.NebulousObjects.Core.Enums;
using BrunelUni.NebulousObjects.Core.Interfaces.Contract;
using NSubstitute;
using NUnit.Framework;

namespace BrunelUni.NebulousObjects.Tests.NebulousCollectionTests;

public class When_Incoming_Create_Operation_Is_Available : Given_A_NebulousList
{
    private OperationDto _operationDto;
    private bool _raised;
    protected override Person [ ] StartingItems => Array.Empty<Person>( );

    protected override void When( )
    {
        _operationDto = new OperationDto
        {
            Operation = OperationEnum.Create
        };
        MockNebulousManager.OperationAvailable += dto => _raised = true;
        MockNebulousManager.OperationAvailable += Raise.Event<Action<OperationDto>>( _operationDto );
    }

    [ Test ]
    public void Then_Event_Was_Raised( )
    {
        Assert.AreEqual( true, _raised );
    }
    
    [ Test ]
    public void Then_Changes_Are_Replicated( )
    {
        MockNebulousManager.DidNotReceive(  ).ReplicateDelete( Arg.Any<int>( ), Arg.Any<INebulousList<Person>>( ) );
        MockNebulousManager.DidNotReceive(  ).ReplicateUpdate( Arg.Any<int>( ), Arg.Any<Person>( ), Arg.Any<INebulousList<Person>>( ) );
        MockNebulousManager.Received( 1 )
            .ReplicateCreate( Arg.Any<Person>( ), Arg.Any<INebulousList<Person>>( ) );
        MockNebulousManager.Received( ).ReplicateCreate( _operationDto.Data as Person, SUT );
    }
}