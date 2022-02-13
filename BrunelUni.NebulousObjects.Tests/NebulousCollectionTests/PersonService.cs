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

    public void UpdatePerson( Person person ) =>
        _nebulousPeople.ReplaceFirstOccurance( x => x.Id == person.Id, person );
}