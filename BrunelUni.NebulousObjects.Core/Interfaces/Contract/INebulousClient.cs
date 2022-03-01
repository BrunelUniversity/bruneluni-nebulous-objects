using BrunelUni.NebulousObjects.Core.Dtos;

namespace BrunelUni.NebulousObjects.Core.Interfaces.Contract;

public interface INebulousClient
{
    void UpdateData<T>( OperationDto operationDto );
    void Send( OperationDto operationDto );
}