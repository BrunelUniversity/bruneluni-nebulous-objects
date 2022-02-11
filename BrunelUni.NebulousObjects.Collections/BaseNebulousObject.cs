using System.Runtime.Serialization.Formatters.Binary;

namespace BrunelUni.NebulousObjects.Collections;

[ Serializable ]
public abstract class BaseNebulousObject
{
    public Guid Id { get; set; } = Guid.NewGuid( );

    public T CloneThis<T>( ) where T : BaseNebulousObject => Clone<T>( this );

    public T CloneFrom<T>( T obj ) where T : BaseNebulousObject => Clone<T>( obj );

    private static T Clone<T>( object obj ) where T : BaseNebulousObject
    {
        using var ms = new MemoryStream( );
        var formatter = new BinaryFormatter( );
        formatter.Serialize( ms, obj );
        ms.Position = 0;

        return( T )formatter.Deserialize( ms );
    }
}