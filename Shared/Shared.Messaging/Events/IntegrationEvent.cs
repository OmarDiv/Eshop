namespace Shared.Messaging.Events
{
    public record IntegrationEvent
    {
        public Guid EventId => Guid.NewGuid();
        public DateTime CreatedAt => DateTime.UtcNow;
        public string EventType => GetType().AssemblyQualifiedName!;
    }
}
