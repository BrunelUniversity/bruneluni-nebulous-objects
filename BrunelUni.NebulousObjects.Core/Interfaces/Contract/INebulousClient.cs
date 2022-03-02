using BrunelUni.NebulousObjects.Core.Dtos;

namespace BrunelUni.NebulousObjects.Core.Interfaces.Contract;

public interface INebulousClient
{
    event Action<OperationDto> MessageAvailable;
    void Send( OperationDto operationDto );
}