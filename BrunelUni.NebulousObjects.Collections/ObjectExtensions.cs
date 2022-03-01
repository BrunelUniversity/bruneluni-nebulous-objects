using Newtonsoft.Json;

namespace BrunelUni.NebulousObjects.Collections;

public static class ObjectExtensions
{
    public static T Clone<T>( this T obj ) =>
        JsonConvert.DeserializeObject<T>( JsonConvert.SerializeObject( obj ) )!;

    public static string GetJsonString<T>( this T obj ) =>
        JsonConvert.SerializeObject( obj );
    
    public static bool NebulousEquals<T>( this T @this, T obj ) =>
        GetJsonString( @this ) == GetJsonString( obj );
}