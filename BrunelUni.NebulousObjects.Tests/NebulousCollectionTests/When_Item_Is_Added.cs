using System.Linq;
using BrunelUni.NebulousObjects.Collections;
using BrunelUni.NebulousObjects.Core.Dtos;
using BrunelUni.NebulousObjects.Core.Enums;
using NSubstitute;
using NUnit.Framework;

namespace BrunelUni.NebulousObjects.Tests.NebulousCollectionTests;

public class When_Item_Is_Added : Given_A_NebulousList
{
    private string _addedName;
    private Person _person;

    protected override Person [ ] StartingItems => new [ ]
    {
        new Person
        {
            Name = "Aidan"
        },
        new Person
        {
            Name = "John"
        }
    };

    protected override void When( )
    {
        _addedName = "David";
        _person = new Person
        {
            Name = _addedName
        };
        SUT.Add( _person );
    }

    [ Test ]
    public void Then_Exclusive_List_Lock_Is_Aquired( )
    {
        Received.InOrder( ( ) =>
        {
            MockNebulousObjectManager.Send( Arg.Is<OperationDto>( o =>
                o.Operation == OperationEnum.EnterExclusiveListLock ) );
            MockNebulousObjectManager.Send( Arg.Is<OperationDto>( o =>
                o.Operation == OperationEnum.Create && o.Data.NebulousEquals( _person ) ) );
            MockNebulousObjectManager.Send( Arg.Is<OperationDto>( o =>
                o.Operation == OperationEnum.ExitExclusiveListLock ) );
        } );
        MockNebulousObjectManager.Received( 3 ).Send( Arg.Any<OperationDto>( ) );
    }

    [ Test ]
    public void Then_Item_Was_Added( ) { Assert.DoesNotThrow( ( ) => _ = SUT.First( x => x.Name == _addedName ) ); }
}