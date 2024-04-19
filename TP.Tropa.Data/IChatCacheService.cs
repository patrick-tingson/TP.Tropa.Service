using TP.Tropa.Domain;

public interface IChatCacheService
{
    Task<ChatGroupModel> GetGroupAsync(string groupId);
    Task CreateGroupAsync(ChatGroupModel data);
    Task<IEnumerable<ChatMessageModel>> GetMessageToGroupAsync(string groupId);
    Task AddMessageToGroupAsync(ChatMessageModel data);
}
