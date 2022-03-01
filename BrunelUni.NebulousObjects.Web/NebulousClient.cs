using BrunelUni.NebulousObjects.Core.Dtos;
using BrunelUni.NebulousObjects.Core.Interfaces.Contract;

namespace BrunelUni.NebulousObjects.Web;

public class NebulousClient : INebulousClient
{
    public void UpdateData<T>( OperationDto operationDto ) { throw new NotImplementedException( ); }

    public void Send( OperationDto operationDto )
    {
        throw new NotImplementedException( );
    }
}