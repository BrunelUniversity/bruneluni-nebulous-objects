using System.Linq;
using NSubstitute;
using NUnit.Framework;

namespace BrunelUni.NebulousObjects.Tests.NebulousCollectionTests;

public class When_Item_Is_Added : Given_A_NebulousList
{
    private string _addedName;

    protected override Person [ ] StartingItems => new []
    {
        new Person
        {
            Name = "Aidan"
        },
        new Person
        {
            Name = "John"
        }
    };

    protected override void When( )
    {
        _addedName = "David";
        SUT.Add( new Person
        {
            Name = _addedName
        } );
    }

    [ Test ]
    public void Then_Exclusive_List_Lock_Is_Aquired( )
    {
        Received.InOrder( ( ) =>
        {
            MockNebulousManager.EnterListExclusiveLock<Person>( );
            MockNebulousManager.ExitListExclusiveLock<Person>( );
        } );
        MockNebulousManager.Received( 1 ).EnterListExclusiveLock<object>( );
        MockNebulousManager.Received( 1 ).ExitListExclusiveLock<object>( );
    }
    
    [ Test ]
    public void Then_Item_Was_Added( )
    {
        Assert.DoesNotThrow( ( ) => _ = SUT.First( x => x.Name == _addedName ) );
    }
}