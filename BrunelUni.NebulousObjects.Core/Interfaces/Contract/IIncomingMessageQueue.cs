﻿using Aidan.Common.Core.Attributes;
using Aidan.Common.Core.Enum;
using BrunelUni.NebulousObjects.Core.Dtos;

namespace BrunelUni.NebulousObjects.Core.Interfaces.Contract;

[ ThreadSafe ]
[ Service( Scope = ServiceLifetimeEnum.Singleton ) ]
public interface IIncomingMessageQueue
{
    void Push( string message );
    event Action<OperationDto> MessageAvailable;
}