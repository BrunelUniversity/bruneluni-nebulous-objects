using System;
using System.ComponentModel.DataAnnotations;

namespace BrunelUni.NebulousObjects.Tests;

[ Serializable ]
public class OtherModel
{
    [ MaxLength( 16 ) ] public string Foo { get; set; }

    public Guid Id { get; set; }
}