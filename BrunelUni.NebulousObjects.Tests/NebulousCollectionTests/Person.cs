using System;

namespace BrunelUni.NebulousObjects.Tests.NebulousCollectionTests;

[ Serializable ]
public class Person
{
    public Guid Id { get; set; } = Guid.NewGuid( );
    public string Name { get; set; }
}