namespace BrunelUni.NebulousObjects.Core.Enums;

public enum OperationEnum
{
    Create,
    Update,
    Delete,
    EnterExclusiveListLock,
    EnterSharedListLock,
    EnterExclusiveLock,
    EnterSharedLock,
    ExitExclusiveListLock,
    ExitSharedListLock,
    ExitExclusiveLock,
    ExitSharedLock,
    Ack
}