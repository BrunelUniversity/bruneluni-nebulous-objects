using Aidan.Common.Utils.Test;
using BrunelUni.NebulousObjects.Collections;
using BrunelUni.NebulousObjects.Core.Interfaces.Contract;
using NSubstitute;

namespace BrunelUni.NebulousObjects.Tests.NebulousCollectionTests;

public abstract class Given_A_NebulousList : GivenWhenThen<INebulousList<Person>>
{
    protected INebulousObjectManager MockNebulousObjectManager;

    protected abstract Person [ ] StartingItems { get; }

    protected override void Given( )
    {
        MockNebulousObjectManager = Substitute.For<INebulousObjectManager>( );
        SUT = new NebulousList<Person>( MockNebulousObjectManager, StartingItems );
    }
}