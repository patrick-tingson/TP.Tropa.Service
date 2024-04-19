namespace TP.Tropa.Domain;

public record ChatMessageModel
{
    public required Guid Id { get; set; }
    public required string Nickname { get; set; }
    public required string GroupId { get; set; }
    public required DateTimeOffset DateTimeSent { get; set; } 
    public required string Message { get; set; }
}
