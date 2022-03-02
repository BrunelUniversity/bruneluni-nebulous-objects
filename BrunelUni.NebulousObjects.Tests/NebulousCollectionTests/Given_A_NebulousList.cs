using Aidan.Common.Utils.Test;
using BrunelUni.NebulousObjects.Collections;
using BrunelUni.NebulousObjects.Core.Interfaces.Contract;
using NSubstitute;

namespace BrunelUni.NebulousObjects.Tests.NebulousCollectionTests;

public abstract class Given_A_NebulousList : GivenWhenThen<NebulousList<Person>>
{
    protected INebulousClient MockNebulousClient;

    protected abstract Person [ ] StartingItems { get; }

    protected override void Given( )
    {
        MockNebulousClient = Substitute.For<INebulousClient>( );
        SUT = new NebulousList<Person>( MockNebulousClient, StartingItems );
    }
}