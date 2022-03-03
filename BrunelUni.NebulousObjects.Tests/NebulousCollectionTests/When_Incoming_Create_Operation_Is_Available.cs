using System;
using BrunelUni.NebulousObjects.Collections;
using BrunelUni.NebulousObjects.Core.Dtos;
using BrunelUni.NebulousObjects.Core.Enums;
using NSubstitute;
using NUnit.Framework;

namespace BrunelUni.NebulousObjects.Tests.NebulousCollectionTests;

public class When_Incoming_Create_Operation_Is_Available : Given_A_NebulousList
{
    private OperationDto _operationDto;
    private Person _person;
    protected override Person [ ] StartingItems => Array.Empty<Person>( );

    protected override void When( )
    {
        _person = new Person
        {
            Name = "James"
        };
        _operationDto = new OperationDto
        {
            Operation = OperationEnum.Create,
            Data = _person
        };
        MockNebulousClient.MessageAvailable += Raise.Event<Action<OperationDto>>( _operationDto );
    }

    [ Test ]
    public void Then_Data_Is_Added( ) { Assert.True( SUT[ 0 ].NebulousEquals( _person ) ); }

    [ Test ]
    public void Then_Replication_Is_Acknowledged( ) { MockNebulousClient.Received( 1 ).AckReplication( ); }
}