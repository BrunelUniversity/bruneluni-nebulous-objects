using System;

namespace BrunelUni.NebulousObjects.Tests;

public static class TestHelpers
{
    public const string GuidString = "22a1471e-c2d5-4e50-9497-a4ab25321dea";
    public const string GuidString2 = "a1f5abef-7818-43b6-b4c6-be374a109ade";

    public static readonly byte [ ] ObjectBytes =
    {
        0x50, 0x65, 0x72, 0x73, 0x6f, 0x6e, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
    };

    public static readonly byte [ ] GuidBytes =
    {
        0x1e, 0x47, 0xa1, 0x22, 0xd5, 0xc2, 0x50, 0x4e, 0x94, 0x97, 0xa4, 0xab, 0x25, 0x32, 0x1d, 0xea
    };

    public static readonly byte [ ] GuidBytes2 = new Guid( GuidString2 ).ToByteArray( );
}