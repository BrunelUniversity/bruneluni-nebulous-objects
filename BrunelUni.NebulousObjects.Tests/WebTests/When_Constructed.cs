using System;
using NSubstitute;
using NUnit.Framework;

namespace BrunelUni.NebulousObjects.Tests.WebTests;

public class When_Constructed : Given_A_NebulousClient
{
    [ Test ]
    public void Then_Message_Available_Event_Is_Subscribed_To_Once( )
    {
        MockMessageService.Received( 1 ).MessageAvailable += Arg.Any<Action<byte [ ]>>( );
    }

    [ Test ]
    public void Then_All_Models_Are_Listed_For_Discovery( )
    {
        var models = SUT.Models;
        Assert.True( models[ "Person" ] == typeof( Person ) );
        Assert.True( models[ "OtherModel" ] == typeof( OtherModel ) );
    }
}