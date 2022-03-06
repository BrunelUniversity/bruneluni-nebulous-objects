using Aidan.Common.Core.Attributes;
using Aidan.Common.Core.Enum;
using BrunelUni.NebulousObjects.Core.Dtos;

namespace BrunelUni.NebulousObjects.Core.Interfaces.Contract;

[ Service( Scope = ServiceLifetimeEnum.Singleton ) ]
public interface INebulousObjectManager
{
    event Action<OperationDto> MessageAvailable;
    void Send( OperationDto operationDto );
}