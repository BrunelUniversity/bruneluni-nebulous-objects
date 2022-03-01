using BrunelUni.NebulousObjects.Core.Enums;

namespace BrunelUni.NebulousObjects.Core.Dtos;

public class OperationDto
{
    public object? Data { get; set; }
    public OperationEnum Operation { get; set; }
    public int Index { get; set; }
}