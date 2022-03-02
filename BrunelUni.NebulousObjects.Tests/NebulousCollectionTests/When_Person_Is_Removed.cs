using System;
using System.Linq;
using NSubstitute;
using NUnit.Framework;

namespace BrunelUni.NebulousObjects.Tests.NebulousCollectionTests;

public class When_Person_Is_Removed : Given_A_NebulousList
{
    private const string RemovedName = "Aidan";
    private bool _removeResult;
    private readonly Guid _id = Guid.NewGuid(  );

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
            MockNebulousManager.EnterListExclusiveLock<Person>( );
            MockNebulousManager.Delete<Person>( 0 );
            MockNebulousManager.ExitListExclusiveLock<Person>( );
        } );
        MockNebulousManager.Received( 1 ).Delete<Person>( Arg.Any<int>( ) );
        MockNebulousManager.Received( 1 ).EnterListExclusiveLock<object>( );
        MockNebulousManager.Received( 1 ).ExitListExclusiveLock<object>( );
    }
    
    [ Test ]
    public void Then_Item_Was_Removed( )
    {
        Assert.Throws<InvalidOperationException>( ( ) => _ = SUT.First( x => x.Name == RemovedName ) );
        Assert.AreEqual( true, _removeResult );
    }
}