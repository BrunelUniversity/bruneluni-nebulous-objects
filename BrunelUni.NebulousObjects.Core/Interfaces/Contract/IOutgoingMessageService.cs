namespace BrunelUni.NebulousObjects.Core.Interfaces.Contract;

public interface IOutgoingMessageService
{
    void Add( string message );
    string Recieve( );
}