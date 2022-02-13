﻿using System.Linq;
using NSubstitute;
using NUnit.Framework;

namespace BrunelUni.NebulousObjects.Tests.NebulousCollectionTests;

public class When_Person_Is_Searched_And_First_Occurance_Is_Replaced : Given_A_NebulousList
{
    protected override Person [ ] StartingItems { get; } =
    {
        new()
        {
            Name = "Dave"
        }
    };

    private Person StartingItem => StartingItems[0];

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
        Received.InOrder( ( ) =>
        {
            MockNebulousManager.EnterItemExclusiveLock<Person>( 0 );
            MockNebulousManager.ExitItemExclusiveLock<Person>( 0 );
        } );
        MockNebulousManager.Received( 1 ).EnterItemExclusiveLock<object>( Arg.Any<int>( ) );
        MockNebulousManager.Received( 1 ).ExitItemExclusiveLock<object>( Arg.Any<int>( ) );
    }
}