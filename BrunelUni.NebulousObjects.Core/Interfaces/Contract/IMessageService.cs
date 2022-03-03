using Aidan.Common.Core.Attributes;
using Aidan.Common.Core.Enum;

namespace BrunelUni.NebulousObjects.Core.Interfaces.Contract;

[ Service( Scope = ServiceLifetimeEnum.Singleton ) ]
public interface IMessageService
{
    public bool CurrentAck { get; set; }
    public Guid? CurrentTransactionID { get; set; }
    void AddOutgoing( byte [ ] messageParts );
    byte [ ] GetOutgoingResponse( );
    byte [ ] AddIncomingResponse( byte [ ] messageParts );
    void GetIncoming( byte [ ] messageParts );
}