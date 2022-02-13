using System;
using NSubstitute;
using NUnit.Framework;

namespace BrunelUni.NebulousObjects.Tests.NebulousCollectionTests;

public class When_Person_Is_Updated : Given_A_NebulousList_Is_Injected
{
    private Person _person;

    protected override void When( )
    {
        _person = new Person
        {
            Name = "Dave"
        };
        SUT.Add( _person );
        PersonService.UpdatePerson( new Person
        {
            Id = _person.Id,
            Name = "Peter"
        } );
    }

    [ Test ]
    public void Then_Person_Was_Updated( )
    {
        Assert.AreEqual( "Peter", PersonService.GetPersonById( _person.Id ).Name );
    }
    
    [ Test ]
    public void Then_Exclusive_Lock_Was_Aquired( )
    {
        Received.InOrder( ( ) =>
        {
            MockNebulousManager.EnterExclusiveLock<Person>( _person.Id );
            MockNebulousManager.ExitExclusiveLock<Person>( _person.Id );
        } );
        MockNebulousManager.Received( 1 ).EnterExclusiveLock<object>( Arg.Any<Guid>( ) );
        MockNebulousManager.Received( 1 ).ExitExclusiveLock<object>( Arg.Any<Guid>( ) );
    }
}