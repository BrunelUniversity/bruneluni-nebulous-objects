using System;
using BrunelUni.NebulousObjects.Collections;

namespace BrunelUni.NebulousObjects.Tests.NebulousCollectionTests;

[ Serializable ]
public class Person : BaseNebulousObject
{
    public string Name { get; set; }
}