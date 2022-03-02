using System;
using System.Linq;
using BrunelUni.NebulousObjects.Core.Dtos;
using BrunelUni.NebulousObjects.Core.Enums;
using BrunelUni.NebulousObjects.Tests.NebulousCollectionTests;
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
        MessageService.GetOutgoingResponse( ).Returns( OperationEnum.Ack.ToString( ) );
        _person = new Person
        {
            Id = new Guid( "22a1471e-c2d5-4e50-9497-a4ab25321dea" ),
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
        var objectBytes = new byte [ ]
        {
            0x50, 0x65, 0x72, 0x73, 0x6f, 0x6e, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        };
        var guidBytes = new byte [ ]
        {
            0x1e, 0x47, 0xa1, 0x22, 0xd5, 0xc2, 0x50, 0x4e, 0x94, 0x97, 0xa4, 0xab, 0x25, 0x32, 0x1d, 0xea
        };
        var nameBytes = new byte [ ]
        {
            0x4A, 0x61, 0x6D, 0x65, 0x73, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        };
        var bytes = operationBytes
            .Concat( indexBytes )
            .Concat( objectBytes )
            .Concat( guidBytes )
            .Concat( nameBytes )
            .ToArray( );
        MessageService.Received( 1 ).AddOutgoing( Arg.Any<byte [ ]>( ) );
        MessageService.Received( ).AddOutgoing( Arg.Is<byte [ ]>( b => b.SequenceEqual( bytes ) ) );
    }
}