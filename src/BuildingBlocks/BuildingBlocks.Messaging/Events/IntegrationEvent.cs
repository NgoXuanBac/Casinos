﻿namespace BuildingBlocks.Messaging.Events;
public abstract record IntegrationEvent
{
    public Guid Id => Guid.NewGuid();
    public DateTime OccurredOn => DateTime.UtcNow;
    public string EventType => GetType().AssemblyQualifiedName ?? string.Empty;
}
