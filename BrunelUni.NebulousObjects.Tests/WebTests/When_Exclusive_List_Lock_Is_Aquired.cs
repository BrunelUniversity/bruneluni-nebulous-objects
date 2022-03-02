using BrunelUni.NebulousObjects.Core.Dtos;
using BrunelUni.NebulousObjects.Core.Enums;
using BrunelUni.NebulousObjects.Tests.NebulousCollectionTests;
using NSubstitute;
using NUnit.Framework;

namespace BrunelUni.NebulousObjects.Tests.WebTests;

public class When_Exclusive_List_Lock_Is_Aquired : Given_A_NebulousManager
{
    protected override void When( ) { SUT.EnterListExclusiveLock<Person>( ); }

    [ Test ]
    public void Then_Lock_Operation_Is_Sent( )
    {
        MockNebulousClient.Received( 1 ).Send( Arg.Any<OperationDto>( ) );
        MockNebulousClient.Received( ).Send( Arg.Is<OperationDto>( o =>
            o.DataType == typeof( Person ) && o.Operation == OperationEnum.EnterExclusiveListLock ) );
    }
}