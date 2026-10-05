namespace Shared.Messaging.Extentions.Settings
{
    public record MessageBroker
    {
        public const string SectionName = "MessageBroker";
        public string Host { get; init; } = string.Empty;
        public string Username { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
    }
}
