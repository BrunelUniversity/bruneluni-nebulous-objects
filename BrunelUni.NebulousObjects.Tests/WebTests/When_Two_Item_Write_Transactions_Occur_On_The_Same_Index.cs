using System;
using System.Linq;
using BrunelUni.NebulousObjects.Core.Enums;
using NSubstitute;
using NUnit.Framework;

namespace BrunelUni.NebulousObjects.Tests.WebTests;

[ TestFixture( ( byte )0x05, LockEnum.Shared, ( byte )0x04 ) ]
[ TestFixture( ( byte )0x04, LockEnum.Exclsuive, ( byte )0x04 ) ]
[ TestFixture( ( byte )0x04, LockEnum.Exclsuive, ( byte )0x05 ) ]
public class When_Two_Item_Conflicting_Transactions_Occur_On_The_Same_Index : Given_A_TransactionManager
{
    private readonly byte _firstTransactionOp;
    private readonly LockEnum _transactionToBeProcessed;
    private readonly byte _secondTransactionOp;

    public When_Two_Item_Conflicting_Transactions_Occur_On_The_Same_Index( byte firstTransactionOp,
        LockEnum transactionToBeProcessed, byte secondTransactionOp )
    {
        _firstTransactionOp = firstTransactionOp;
        _transactionToBeProcessed = transactionToBeProcessed;
        _secondTransactionOp = secondTransactionOp;
    }

    protected override void When( )
    {
        MockMessageService.MessageAvailable += Raise.Event<Action<byte [ ]>>(
            new byte [ ] { _firstTransactionOp, 0x01, 0x00 }
                .Concat( TestHelpers.ObjectBytes )
                .Concat( TestHelpers.GuidBytes )
                .ToArray( ) );
        MockMessageService.MessageAvailable += Raise.Event<Action<byte [ ]>>(
            new byte [ ] { _secondTransactionOp, 0x01, 0x00 }
                .Concat( TestHelpers.ObjectBytes )
                .Concat( TestHelpers.GuidBytes2 )
                .ToArray( ) );
    }

    [ Test ]
    public void Then_Only_1_Transaction_Is_Processed( ) { Assert.AreEqual( 1, _transactions.Count ); }

    [ Test ]
    public void Then_Only_First_Transaction_Is_Processed( )
    {
        var transactionDto = _transactions.First( );
        Assert.AreEqual( LockGranularityEnum.Item, transactionDto.Granularity );
        Assert.AreEqual( _transactionToBeProcessed, transactionDto.Type );
    }
}