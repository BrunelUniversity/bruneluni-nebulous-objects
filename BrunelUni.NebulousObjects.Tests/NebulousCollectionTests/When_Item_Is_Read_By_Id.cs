using System;
using System.Linq;
using NSubstitute;
using NUnit.Framework;

namespace BrunelUni.NebulousObjects.Tests.NebulousCollectionTests;
[ TestFixture(0, "Frank") ]
[ TestFixture(1, "Ann") ]
[ TestFixture(2, "Joe") ]
public class When_Item_Is_Read_By_Id : Given_A_NebulousList
{
    private readonly int _personIndex;
    private readonly string _expectedPerson;
    protected override Person [ ] StartingItems { get; } =
    {
        new()
        {
            Name = "Frank"
        },
        new()
        {
            Name = "Ann"
        },
        new()
        {
            Name = "Joe"
        }
    };
    
    public When_Item_Is_Read_By_Id( int personIndex, string expectedPerson )
    {
        _personIndex = personIndex;
        _expectedPerson = expectedPerson;
    }

    private Person _person;
    private Guid _guid;


    protected override void When( )
    {
        _guid = StartingItems[ _personIndex ].Id;
        _person = SUT.First( x => x.Id == _guid );
    }

    [ Test ]
    public void Then_Correct_Person_Was_Recieved( )
    {
        Assert.AreEqual( _expectedPerson, _person.Name );
    }

    [ Test ]
    public void Then_Recieved_Person_Is_A_Clone_Of_The_Original_Object( )
    {
        Assert.AreNotSame( StartingItems[ _personIndex ], _person );
    }

    [ Test ]
    public void Then_Read_Lock_Is_Entered_Then_Exitted( )
    {
        Received.InOrder( ( ) =>
        {
            MockNebulousManager.EnterItemSharedLock<Person>( _personIndex );
            MockNebulousManager.ExitItemSharedLock<Person>( _personIndex );
        } );
        MockNebulousManager.Received( 3 ).EnterItemSharedLock<object>( Arg.Any<int>( ) );
        MockNebulousManager.Received( 3 ).ExitItemSharedLock<object>( Arg.Any<int>( ) );
    }
}