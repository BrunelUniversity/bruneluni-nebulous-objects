using BrunelUni.NebulousObjects.Core.Enums;

namespace BrunelUni.NebulousObjects.Core.Dtos;

public class OperationDto
{
    public Type DataType { get; set; }
    public object? Data { get; set; }
    public OperationEnum Operation { get; set; }
    public int Index { get; set; }
}