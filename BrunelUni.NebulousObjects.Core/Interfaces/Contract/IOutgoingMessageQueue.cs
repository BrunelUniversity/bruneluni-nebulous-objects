using Aidan.Common.Core.Attributes;
using Aidan.Common.Core.Enum;

namespace BrunelUni.NebulousObjects.Core.Interfaces.Contract;

[ ThreadSafe ]
[ Service( Scope = ServiceLifetimeEnum.Singleton ) ]
public interface IOutgoingMessageQueue
{
    void Push( string message );
    void Pop( );
}