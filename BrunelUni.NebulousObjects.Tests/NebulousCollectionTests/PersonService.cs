using System;
using System.Linq;
using BrunelUni.NebulousObjects.Core.Interfaces.Contract;

namespace BrunelUni.NebulousObjects.Tests.NebulousCollectionTests;

public class PersonService
{
    private readonly INebulousList<Person> _nebulousPeople;

    public PersonService( INebulousList<Person> nebulousPeople ) { _nebulousPeople = nebulousPeople; }

    public Person GetPersonById( Guid id ) => _nebulousPeople
        .FirstOrDefault( x => x.Id == id );

    public void UpdatePerson( Person person )
    {
        var personInCollection = _nebulousPeople
            .Where( x => x.Id == person.Id ).First();
        personInCollection = person;
    }
}