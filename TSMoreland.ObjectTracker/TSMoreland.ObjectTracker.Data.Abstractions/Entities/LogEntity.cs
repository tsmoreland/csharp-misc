namespace TSMoreland.ObjectTracker.Data.Abstractions.Entities;

public sealed class LogEntity
{
    public LogEntity(int id, int objectEntityId, int severity, string message)
    {
        Id = id;
        ObjectEntityId = objectEntityId;
        Severity = severity;
        Message = message;
    }
    private LogEntity()
    {
        
    }

    public int Id { get; private set; }
    public int ObjectEntityId { get; set; }

    public int Severity { get; set; }
    public string Message { get; set; } = string.Empty;
}