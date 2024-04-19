using TP.Tropa.Domain;

public interface IChatCacheLogic
{
    Task<ChatGroupModel> GetGroupAsync(string groupId);
    Task<ChatGroupModel> CreateGroupAsync();
    Task<IEnumerable<ChatMessageModel>> GetMessageToGroupAsync(string groupId);
    Task AddMessageToGroupAsync(ChatMessageModel data);
}
