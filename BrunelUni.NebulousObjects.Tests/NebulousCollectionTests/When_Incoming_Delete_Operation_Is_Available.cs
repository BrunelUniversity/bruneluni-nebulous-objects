using System;
using BrunelUni.NebulousObjects.Core.Dtos;
using BrunelUni.NebulousObjects.Core.Enums;
using NSubstitute;
using NUnit.Framework;

namespace BrunelUni.NebulousObjects.Tests.NebulousCollectionTests;

public class When_Incoming_Delete_Operation_Is_Available : Given_A_NebulousList
{
    private OperationDto _operationDto;

    protected override Person [ ] StartingItems => new [ ]
    {
        new Person( ),
        new Person( )
    };

    protected override void When( )
    {
        _operationDto = new OperationDto
        {
            Operation = OperationEnum.Delete,
            Index = 1
        };
        MockNebulousObjectManager.MessageAvailable += Raise.Event<Action<OperationDto>>( _operationDto );
    }

    [ Test ]
    public void Then_Data_Is_Deleted( ) { Assert.Throws<ArgumentOutOfRangeException>( ( ) => _ = SUT[ 1 ] ); }
}