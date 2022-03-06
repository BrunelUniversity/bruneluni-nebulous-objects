using Aidan.Common.Core.Attributes;
using Aidan.Common.Core.Enum;

namespace BrunelUni.NebulousObjects.Core.Interfaces.Contract;

[ Service( Scope = ServiceLifetimeEnum.Singleton ) ]
public interface IMessageService
{
    public Guid? CurrentTransactionID { get; set; }
    void AddOutgoing( byte [ ] messageParts );
    event Action<byte [ ]> MessageAvailable;
}