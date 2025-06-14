namespace NotificationService.Models;

internal sealed class Notification
{
    public Guid Id { get; }
    public string NotificationKey { get; }
    public string Payload { get; }
    public bool IsProcessed { get; }
    public DateTimeOffset CreatedAt { get; }

    public Notification(Guid id, string notificationKey, string payload, bool isProcessed, DateTimeOffset createdAt)
    {
        Id = id;
        NotificationKey = notificationKey;
        Payload = payload;
        IsProcessed = isProcessed;
        CreatedAt = createdAt;
    }
}