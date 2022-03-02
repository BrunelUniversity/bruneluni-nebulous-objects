using System;
using System.Linq;
using BrunelUni.NebulousObjects.Core.Dtos;
using BrunelUni.NebulousObjects.Core.Enums;
using NSubstitute;
using NUnit.Framework;

namespace BrunelUni.NebulousObjects.Tests.NebulousCollectionTests;

public class When_Person_Is_Removed : Given_A_NebulousList
{
    private const string RemovedName = "Aidan";
    private readonly Guid _id = Guid.NewGuid( );
    private bool _removeResult;

    protected override Person [ ] StartingItems => new [ ]
    {
        new Person
        {
            Id = _id,
            Name = RemovedName
        },
        new Person
        {
            Name = "John"
        }
    };

    protected override void When( )
    {
        _removeResult = SUT.Remove( new Person
        {
            Id = _id,
            Name = RemovedName
        } );
    }

    [ Test ]
    public void Then_Exclusive_List_Lock_Is_Aquired( )
    {
        Received.InOrder( ( ) =>
        {
            MockNebulousClient.Send( Arg.Is<OperationDto>( o => o.Operation == OperationEnum.EnterExclusiveListLock ) );
            MockNebulousClient.Send( Arg.Is<OperationDto>( o =>
                o.Operation == OperationEnum.Delete && o.Index == 0 ) );
            MockNebulousClient.Send( Arg.Is<OperationDto>( o => o.Operation == OperationEnum.ExitExclusiveListLock ) );
        } );
        MockNebulousClient.Received( 7 ).Send( Arg.Any<OperationDto>( ) );
    }

    [ Test ]
    public void Then_Item_Was_Removed( )
    {
        Assert.Throws<InvalidOperationException>( ( ) => _ = SUT.First( x => x.Name == RemovedName ) );
        Assert.AreEqual( true, _removeResult );
    }
}