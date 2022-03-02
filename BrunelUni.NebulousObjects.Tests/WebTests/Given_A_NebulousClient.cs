using Aidan.Common.Utils.Test;
using BrunelUni.NebulousObjects.Core.Interfaces.Contract;
using BrunelUni.NebulousObjects.Web;
using NSubstitute;

namespace BrunelUni.NebulousObjects.Tests.WebTests;

public class Given_A_NebulousClient : GivenWhenThen<INebulousClient>
{
    protected IMessageService MessageService;

    protected override void Given( )
    {
        MessageService = Substitute.For<IMessageService>( );
        SUT = new NebulousClient( MessageService );
    }
}