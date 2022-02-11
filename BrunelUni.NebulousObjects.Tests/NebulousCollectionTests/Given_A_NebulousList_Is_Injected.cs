using Aidan.Common.Utils.Test;
using BrunelUni.NebulousObjects.Collections;
using BrunelUni.NebulousObjects.Core.Interfaces.Contract;
using NSubstitute;

namespace BrunelUni.NebulousObjects.Tests.NebulousCollectionTests;

public class Given_A_NebulousList_Is_Injected : GivenWhenThen<INebulousList<Person>>
{
    protected PersonService PersonService;
    protected INebulousManager MockNebulousManager;

    protected override void Given( )
    {
        MockNebulousManager = Substitute.For<INebulousManager>( );
        SUT = new NebulousList<Person>( MockNebulousManager );
        PersonService = new PersonService( SUT );
    }
}