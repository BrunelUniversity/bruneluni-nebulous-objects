using BrunelUni.NebulousObjects.Core.Dtos;
using BrunelUni.NebulousObjects.Core.Interfaces.Contract;

namespace BrunelUni.NebulousObjects.Web;

public class NebulousClient : INebulousClient
{
    public event Action<OperationDto> MessageAvailable;

    public void Send( OperationDto operationDto ) { throw new NotImplementedException( ); }
}