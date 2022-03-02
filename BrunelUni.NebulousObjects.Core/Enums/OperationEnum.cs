namespace BrunelUni.NebulousObjects.Core.Enums;

public enum OperationEnum
{
    Create,
    Update,
    Delete,
    EnterExclusiveListLock,
    EnterExclusiveLock,
    EnterSharedLock,
    ExitExclusiveListLock,
    ExitExclusiveLock,
    ExitSharedLock,
    Ack,
    ReplicAck
}