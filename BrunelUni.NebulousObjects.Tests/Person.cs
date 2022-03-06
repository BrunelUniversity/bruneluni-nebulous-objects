using System;
using System.ComponentModel.DataAnnotations;

namespace BrunelUni.NebulousObjects.Tests;

[ Serializable ]
public class Person
{
    public Guid Id { get; set; } = Guid.NewGuid( );

    [ MaxLength( 15 ) ] public string Name { get; set; }
}