using System;
using NSubstitute;
using NUnit.Framework;

namespace BrunelUni.NebulousObjects.Tests.NebulousCollectionTests;
[ TestFixture(0, "Frank") ]
[ TestFixture(1, "Ann") ]
[ TestFixture(2, "Joe") ]
public class When_Item_Is_Read : Given_A_NebulousList_Is_Injected
{
    private readonly int _personIndex;
    private readonly string _expectedPerson;

    public When_Item_Is_Read( int personIndex, string expectedPerson )
    {
        _personIndex = personIndex;
        _expectedPerson = expectedPerson;
    }
    
    private Person[] _people;
    private Person _person;
    private Guid _guid;

    protected override void When( )
    {
        _people = new [ ]
        {
            new Person
            {
                Name = "Frank"
            },
            new Person
            {
                Name = "Ann"
            },
            new Person
            {
                Name = "Joe"
            }
        };
        SUT.Add( _people[ 0 ] );
        SUT.Add( _people[ 1 ] );
        SUT.Add( _people[ 2 ] );
        _guid = _people[ _personIndex ].Id;
        _person = PersonService.GetPersonById( _guid );
    }

    [ Test ]
    public void Then_Correct_Person_Was_Recieved( )
    {
        Assert.AreEqual( _expectedPerson, _person.Name );
    }

    [ Test ]
    public void Then_Recieved_Person_Is_A_Clone_Of_The_Original_Object( )
    {
        Assert.AreNotSame( _people[ _personIndex ], _person );
    }

    [ Test ]
    public void Then_Read_Lock_Is_Entered_Then_Exitted( )
    {
        Received.InOrder( ( ) =>
        {
            MockNebulousManager.EnterSharedLock<Person>( _guid );
            MockNebulousManager.ExitSharedLock<Person>( _guid );
        } );
        MockNebulousManager.Received( 3 ).EnterSharedLock<object>( Arg.Any<Guid>( ) );
        MockNebulousManager.Received( 3 ).ExitSharedLock<object>( Arg.Any<Guid>( ) );
    }
}