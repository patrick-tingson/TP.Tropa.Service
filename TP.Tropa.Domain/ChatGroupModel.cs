namespace TP.Tropa.Domain;

public record ChatGroupModel
{
    public required string GroupId { get; set; }
    public required DateTimeOffset DateTimeExpiration { get; set; } 
}