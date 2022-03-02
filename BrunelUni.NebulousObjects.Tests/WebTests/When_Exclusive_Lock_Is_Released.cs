using BrunelUni.NebulousObjects.Core.Dtos;
using BrunelUni.NebulousObjects.Core.Enums;
using BrunelUni.NebulousObjects.Tests.NebulousCollectionTests;
using NSubstitute;
using NUnit.Framework;

namespace BrunelUni.NebulousObjects.Tests.WebTests;

public class When_Exclusive_Lock_Is_Released : Given_A_NebulousManager
{
    private int _index;

    protected override void When( )
    {
        _index = 0;
        SUT.ExitItemExclusiveLock<Person>( _index );
    }

    [ Test ]
    public void Then_Lock_Operation_Is_Sent( )
    {
        MockNebulousClient.Received( 1 ).Send( Arg.Any<OperationDto>( ) );
        MockNebulousClient.Received( ).Send( Arg.Is<OperationDto>( o =>
            o.DataType == typeof( Person ) && o.Operation == OperationEnum.ExitExclusiveLock && o.Index == _index ) );
    }
}