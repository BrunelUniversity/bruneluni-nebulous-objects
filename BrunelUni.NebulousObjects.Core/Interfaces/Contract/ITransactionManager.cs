using Aidan.Common.Core.Attributes;
using Aidan.Common.Core.Enum;
using BrunelUni.NebulousObjects.Core.Dtos;

namespace BrunelUni.NebulousObjects.Core.Interfaces.Contract;

[ Service( Scope = ServiceLifetimeEnum.Singleton ) ]
public interface ITransactionManager
{
    public TransactionDto GetTransactionBeingProccessed { get; }
    event Action<TransactionDto> NewTransactionToBeProcessed;
}