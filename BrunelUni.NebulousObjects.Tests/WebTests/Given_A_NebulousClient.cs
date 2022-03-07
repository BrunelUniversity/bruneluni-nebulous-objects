using Aidan.Common.Core.Interfaces.Contract;
using Aidan.Common.Utils.Test;
using BrunelUni.NebulousObjects.Core.Dtos;
using BrunelUni.NebulousObjects.Core.Interfaces.Contract;
using BrunelUni.NebulousObjects.Web;
using NSubstitute;

namespace BrunelUni.NebulousObjects.Tests.WebTests;

public class Given_A_NebulousClient : GivenWhenThen<INebulousObjectManager>
{
    protected readonly byte [ ] AckBytes = { 0x09 };

    protected IConfigurationAdapter MockConfigurationAdapter;

    protected IMessageService MockMessageService;

    protected override void Given( )
    {
        MockMessageService = Substitute.For<IMessageService>( );
        MockConfigurationAdapter = Substitute.For<IConfigurationAdapter>( );
        MockConfigurationAdapter.Get<AppOptions>( ).Returns( new AppOptions
        {
            ModelNamespace = "BrunelUni.NebulousObjects.Tests"
        } );
        SUT = new NebulousObjectManager( MockMessageService, MockConfigurationAdapter );
    }
}