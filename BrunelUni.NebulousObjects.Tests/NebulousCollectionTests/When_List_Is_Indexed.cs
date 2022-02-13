using NSubstitute;
using NUnit.Framework;

namespace BrunelUni.NebulousObjects.Tests.NebulousCollectionTests;

public class When_List_Is_Indexed : Given_A_NebulousList
{
    private Person _person;

    protected override Person [ ] StartingItems { get; } =
    {
        new()
        {
            Name = "James"
        }
    };

    protected override void When( )
    {
        _person = SUT[ 0 ];
    }
    
    [ Test ]
    public void Then_Correct_Person_Was_Recieved( )
    {
        Assert.AreEqual( StartingItems[ 0 ].Name, _person.Name );
    }

    [ Test ]
    public void Then_Recieved_Person_Is_A_Clone_Of_The_Original_Object( )
    {
        Assert.AreNotSame( StartingItems[ 0 ], _person );
    }

    [ Test ]
    public void Then_Read_Lock_Is_Entered_Then_Exitted( )
    {
        Received.InOrder( ( ) =>
        {
            MockNebulousManager.EnterItemSharedLock<Person>( 0 );
            MockNebulousManager.ExitItemSharedLock<Person>( 0 );
        } );
        MockNebulousManager.Received( 1 ).EnterItemSharedLock<object>( Arg.Any<int>( ) );
        MockNebulousManager.Received( 1 ).ExitItemSharedLock<object>( Arg.Any<int>( ) );
    }
}