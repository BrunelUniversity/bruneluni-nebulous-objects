using Aidan.Common.Core.Attributes;
using Aidan.Common.Core.Enum;
using BrunelUni.NebulousObjects.Core.Dtos;

namespace BrunelUni.NebulousObjects.Core.Interfaces.Contract;

[ Service( Scope = ServiceLifetimeEnum.Singleton ) ]
public interface INebulousObjectManager
{
    public Dictionary<string, Type> Models { get; }
    event Action<OperationDto> OperationAvailable;
    void Send( OperationDto operationDto );
}