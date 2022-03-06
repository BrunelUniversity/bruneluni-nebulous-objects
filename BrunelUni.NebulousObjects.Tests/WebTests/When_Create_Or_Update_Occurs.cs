using System;
using System.Linq;
using BrunelUni.NebulousObjects.Core.Dtos;
using BrunelUni.NebulousObjects.Core.Enums;
using NSubstitute;
using NUnit.Framework;

namespace BrunelUni.NebulousObjects.Tests.WebTests;

[ TestFixture( OperationEnum.Create, ( byte )0x00 ) ]
[ TestFixture( OperationEnum.Update, ( byte )0x01 ) ]
public class When_Create_Or_Update_Occurs : Given_A_NebulousClient
{
    private readonly OperationEnum _operationEnum;
    private readonly byte _operationByte;
    private Person _person;

    public When_Create_Or_Update_Occurs( OperationEnum operationEnum, byte operationByte )
    {
        _operationEnum = operationEnum;
        _operationByte = operationByte;
    }

    protected override void When( )
    {
        _person = new Person
        {
            Id = new Guid( GuidString ),
            Name = "James"
        };
        SUT.Send( new OperationDto
        {
            Operation = _operationEnum,
            Data = _person,
            Index = 1
        } );
    }


    [ Test ]
    public void Then_Message_Is_Sent( )
    {
        var operationBytes = new [ ]
        {
            _operationByte
        };
        var indexBytes = new byte [ ]
        {
            0x01, 0x00
        };
        var nameBytes = new byte [ ]
        {
            0x4A, 0x61, 0x6D, 0x65, 0x73, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        };
        var bytes = operationBytes
            .Concat( indexBytes )
            .Concat( ObjectBytes )
            .Concat( GuidBytes )
            .Concat( nameBytes )
            .ToArray( );
        MockMessageService.Received( 1 ).AddOutgoing( Arg.Any<byte [ ]>( ) );
        MockMessageService.Received( ).AddOutgoing( Arg.Is<byte [ ]>( b => b.SequenceEqual( bytes ) ) );
    }
}