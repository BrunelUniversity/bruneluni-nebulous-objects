using System;
using BrunelUni.NebulousObjects.Core.Dtos;
using BrunelUni.NebulousObjects.Core.Interfaces.Contract;
using NSubstitute;
using NUnit.Framework;

namespace BrunelUni.NebulousObjects.Tests.NebulousCollectionTests;

public class When_Incoming_Operation_Is_Available : Given_A_NebulousList
{
    private OperationDto _operationDto;
    protected override Person [ ] StartingItems => Array.Empty<Person>( );

    protected override void When( )
    {
        _operationDto = new OperationDto( );
        MockNebulousManager.OperationAvailable += Raise.Event<Action<OperationDto>>( _operationDto );
    }

    [ Test ]
    public void Then_Changes_Are_Replicated( )
    {
        MockNebulousManager.Received( 1 )
            .ReplicateChanges( Arg.Any<OperationDto>( ), Arg.Any<INebulousList<object>>( ) );
        MockNebulousManager.Received( ).ReplicateChanges( _operationDto, SUT );
    }
}