using Aidan.Common.Utils.Test;
using BrunelUni.NebulousObjects.Core.Interfaces.Contract;
using BrunelUni.NebulousObjects.Web;
using NSubstitute;

namespace BrunelUni.NebulousObjects.Tests.WebTests;

public class Given_A_NebulousManager : GivenWhenThen<INebulousManager>
{
    protected INebulousClient MockNebulousClient;

    protected override void Given( )
    {
        MockNebulousClient = Substitute.For<INebulousClient>( );
        SUT = new NebulousManager( MockNebulousClient );
    }
}