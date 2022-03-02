using System.Linq;
using NSubstitute;
using NUnit.Framework;

namespace BrunelUni.NebulousObjects.Tests.NebulousCollectionTests;

public class When_Item_Is_Added : Given_A_NebulousList
{
    private string _addedName;
    private Person _person;

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
        _person = new Person
        {
            Name = _addedName
        };
        SUT.Add( _person );
    }

    [ Test ]
    public void Then_Exclusive_List_Lock_Is_Aquired( )
    {
        Received.InOrder( ( ) =>
        {
            MockNebulousManager.EnterListExclusiveLock<Person>( );
            MockNebulousManager.Create( _person );
            MockNebulousManager.ExitListExclusiveLock<Person>( );
        } );
        MockNebulousManager.Received( 1 ).Create( Arg.Any<Person>( ) );
        MockNebulousManager.Received( 1 ).EnterListExclusiveLock<object>( );
        MockNebulousManager.Received( 1 ).ExitListExclusiveLock<object>( );
    }
    
    [ Test ]
    public void Then_Item_Was_Added( )
    {
        Assert.DoesNotThrow( ( ) => _ = SUT.First( x => x.Name == _addedName ) );
    }
}