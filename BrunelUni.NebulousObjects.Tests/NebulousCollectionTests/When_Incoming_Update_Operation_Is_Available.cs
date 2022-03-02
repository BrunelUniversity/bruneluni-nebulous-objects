using System;
using BrunelUni.NebulousObjects.Core.Dtos;
using BrunelUni.NebulousObjects.Core.Enums;
using NSubstitute;
using NUnit.Framework;

namespace BrunelUni.NebulousObjects.Tests.NebulousCollectionTests;

public class When_Incoming_Update_Operation_Is_Available : Given_A_NebulousList
{
    private OperationDto _operationDto;
    private Person _person;

    protected override Person [ ] StartingItems => new [ ]
    {
        new Person
        {
            Name = "John"
        }
    };

    protected override void When( )
    {
        _person = new Person
        {
            Name = "steve"
        };
        _operationDto = new OperationDto
        {
            Operation = OperationEnum.Update,
            Data = _person
        };
        MockNebulousClient.MessageAvailable += Raise.Event<Action<OperationDto>>( _operationDto );
    }

    [ Test ]
    public void Then_Replication_Is_Acknowledged( ) { MockNebulousClient.Received( 1 ).Ack( ); }

    [ Test ]
    public void Then_Data_Is_Updated( )
    {
        Assert.AreEqual( _person.Name, SUT[ 0 ].Name );
        Assert.AreEqual( _person.Id, SUT[ 0 ].Id );
    }
}