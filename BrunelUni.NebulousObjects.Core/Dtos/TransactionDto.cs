using BrunelUni.NebulousObjects.Core.Enums;

namespace BrunelUni.NebulousObjects.Core.Dtos;

public class TransactionDto
{
    public LockEnum Type { get; set; }
    public LockGranularityEnum Granularity { get; set; }
    public Type Model { get; set; }
    public int Index { get; set; }
    public Guid ID { get; set; }
    public StatusEnum Status { get; set; }
}