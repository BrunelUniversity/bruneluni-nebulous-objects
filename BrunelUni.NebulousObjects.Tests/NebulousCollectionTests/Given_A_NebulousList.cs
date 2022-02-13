using Aidan.Common.Utils.Test;
using BrunelUni.NebulousObjects.Collections;
using BrunelUni.NebulousObjects.Core.Interfaces.Contract;
using NSubstitute;

namespace BrunelUni.NebulousObjects.Tests.NebulousCollectionTests;

public abstract class Given_A_NebulousList : GivenWhenThen<NebulousList<Person>>
{
    protected INebulousManager MockNebulousManager;

    protected override void Given( )
    {
        MockNebulousManager = Substitute.For<INebulousManager>( );
        SUT = new NebulousList<Person>( MockNebulousManager, StartingItems );
    }

    protected abstract Person[] StartingItems { get; }
}