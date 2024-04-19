using Microsoft.Extensions.Caching.Memory;
using TP.Tropa.Domain;

public class ChatCacheLogic: IChatCacheLogic
{
    private readonly IChatCacheService _service;

    public ChatCacheLogic(IChatCacheService service)
    {
        _service = service ??
            throw new ArgumentNullException(nameof(service));
    }
    
    public async Task<ChatGroupModel> GetGroupAsync(string groupId)
    {
        return await _service.GetGroupAsync(groupId);
    }

    public Task<ChatGroupModel> CreateGroupAsync()
    {
        ChatGroupModel newChatGroup = new() {
            DateTimeExpiration = DateTimeOffset.UtcNow,
            GroupId = Guid.NewGuid().ToString()
        };

        _ = _service.CreateGroupAsync(newChatGroup);
        
        return Task.FromResult(newChatGroup);
    }

    public async Task<IEnumerable<ChatMessageModel>> GetMessageToGroupAsync(string groupId)
    {
        return await _service.GetMessageToGroupAsync(groupId);
    }

    public Task AddMessageToGroupAsync(ChatMessageModel data)
    {
        _ = _service.AddMessageToGroupAsync(data);
        return Task.CompletedTask;
    }
}
