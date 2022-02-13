using Aidan.Common.Core.Interfaces.Excluded;

namespace BrunelUni.NebulousObjects.Core.Interfaces.Contract;

public interface INebulousListFactory<T> : IFactory
{
    INebulousList<T> Factory( params T [ ] items );
}