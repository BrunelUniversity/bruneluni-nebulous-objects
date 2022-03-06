using Aidan.Common.Utils.Test;
using BrunelUni.NebulousObjects.Core.Interfaces.Contract;
using BrunelUni.NebulousObjects.Web;
using NSubstitute;

namespace BrunelUni.NebulousObjects.Tests.WebTests;

public class Given_A_NebulousClient : GivenWhenThen<INebulousObjectManager>
{
    protected const string GuidString = "22a1471e-c2d5-4e50-9497-a4ab25321dea";

    protected readonly byte [ ] AckBytes = { 0x09 };

    protected readonly byte [ ] GuidBytes =
    {
        0x1e, 0x47, 0xa1, 0x22, 0xd5, 0xc2, 0x50, 0x4e, 0x94, 0x97, 0xa4, 0xab, 0x25, 0x32, 0x1d, 0xea
    };

    protected readonly byte [ ] ObjectBytes =
    {
        0x50, 0x65, 0x72, 0x73, 0x6f, 0x6e, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
    };

    protected IMessageService MockMessageService;

    protected override void Given( )
    {
        MockMessageService = Substitute.For<IMessageService>( );
        SUT = new NebulousObjectManager( MockMessageService );
    }
}