using System.Linq;
using BrunelUni.NebulousObjects.Collections;
using BrunelUni.NebulousObjects.Core.Dtos;
using BrunelUni.NebulousObjects.Core.Enums;
using NSubstitute;
using NUnit.Framework;

namespace BrunelUni.NebulousObjects.Tests.NebulousCollectionTests;

public class When_Person_Is_Updated : Given_A_NebulousList
{
    protected override Person [ ] StartingItems { get; } =
    {
        new( )
        {
            Name = "Dave"
        }
    };

    private Person StartingItem => StartingItems[ 0 ];

    protected override void When( )
    {
        SUT.Add( StartingItem );
        SUT.ReplaceFirstOccurance( x => x.Id == StartingItem.Id, new Person
        {
            Id = StartingItem.Id,
            Name = "Peter"
        } );
    }

    [ Test ]
    public void Then_Person_Was_Updated( )
    {
        Assert.AreEqual( "Peter", SUT.FirstOrDefault( x => x.Id == StartingItem.Id )?.Name );
    }

    [ Test ]
    public void Then_Exclusive_Lock_Was_Aquired( )
    {
        var newItem = StartingItem.Clone( );
        newItem.Name = "Peter";
        Received.InOrder( ( ) =>
        {
            MockNebulousObjectManager.Send(
                Arg.Is<OperationDto>( o => o.Operation == OperationEnum.EnterExclusiveLock ) );
            MockNebulousObjectManager.Send( Arg.Is<OperationDto>( o =>
                o.Operation == OperationEnum.Update && o.Data.NebulousEquals( newItem ) && o.Index == 0 ) );
            MockNebulousObjectManager.Send(
                Arg.Is<OperationDto>( o => o.Operation == OperationEnum.ExitExclusiveLock ) );
        } );
        MockNebulousObjectManager.Received( 14 ).Send( Arg.Any<OperationDto>( ) );
    }
}