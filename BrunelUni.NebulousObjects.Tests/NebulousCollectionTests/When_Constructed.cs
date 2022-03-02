using System;
using BrunelUni.NebulousObjects.Core.Dtos;
using NSubstitute;
using NUnit.Framework;

namespace BrunelUni.NebulousObjects.Tests.NebulousCollectionTests;

public class When_Constructed : Given_A_NebulousList
{
    protected override Person [ ] StartingItems => Array.Empty<Person>( );

    [ Test ]
    public void Then_Operation_Available_Event_Is_Subscribed_To( )
    {
        MockNebulousClient.Received( 1 ).MessageAvailable += Arg.Any<Action<OperationDto>>( );
    }
}