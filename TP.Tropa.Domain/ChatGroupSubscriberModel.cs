namespace TP.Tropa.Domain;

public class ChatGroupSubscriberModel 
{
    public string? Nickname { get; set; }
    public string? GroupId { get; set; }
    public List<string>? ConnectionId { get; set; } = new();
    public DateTimeOffset? DateTimeJoined { get; set; } 
}
