using System;
using System.Collections.Generic;
using Aidan.Common.Utils.Test;
using BrunelUni.NebulousObjects.Core.Dtos;
using BrunelUni.NebulousObjects.Core.Interfaces.Contract;
using BrunelUni.NebulousObjects.Web;
using NSubstitute;

namespace BrunelUni.NebulousObjects.Tests.WebTests;

public class Given_A_TransactionManager : GivenWhenThen<ITransactionManager>
{
    protected List<TransactionDto> _transactions = new( );
    protected IMessageService MockMessageService;
    protected INebulousObjectManager MockNebulousObjectManager;

    protected override void Given( )
    {
        MockNebulousObjectManager = Substitute.For<INebulousObjectManager>( );
        MockNebulousObjectManager.Models.Returns( new Dictionary<string, Type>
        {
            { "Person", typeof( Person ) },
            { "OtherModel", typeof( OtherModel ) }
        } );
        MockMessageService = Substitute.For<IMessageService>( );
        SUT = new TransactionManager( MockMessageService, MockNebulousObjectManager );
        SUT.NewTransactionToBeProcessed += dto => _transactions.Add( dto );
    }
}