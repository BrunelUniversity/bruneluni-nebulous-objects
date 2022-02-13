namespace BrunelUni.NebulousObjects.Core.Interfaces.Contract;

public interface INebulousList<T> : IList<T>
{
    void ReplaceFirstOccurance( Func<T, bool> predicate, T replacement );
}