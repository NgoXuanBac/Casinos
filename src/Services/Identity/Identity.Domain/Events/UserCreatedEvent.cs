namespace Identity.Domain.Events;

public record UserCreatedEvent(User User) : IDomainEvent;