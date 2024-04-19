using Microsoft.Extensions.Caching.Memory;
using TP.Tropa.Domain;

public class ChatCacheService: IChatCacheService
{
    private readonly IMemoryCache _memoryCache;
    private int GROUP_MESSAGE_CACHE_MINUTES = 15;
    private int GROUP_CACHE_MINUTES = 1440;

    public ChatCacheService(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache ??
            throw new ArgumentNullException(nameof(memoryCache));
    }
    
    public Task<ChatGroupModel> GetGroupAsync(string groupId)
    {
        return Task.FromResult(_memoryCache.Get<ChatGroupModel>(
            $"{ChatCacheIdString.Group}_{groupId}"));
    }

    public Task CreateGroupAsync(ChatGroupModel data)
    {
        _memoryCache.Set(
            $"{ChatCacheIdString.Group}_{data.GroupId}",
            data,
            TimeSpan.FromMinutes(GROUP_CACHE_MINUTES)
        );

        return Task.CompletedTask;
    }

    public async Task<IEnumerable<ChatMessageModel>> GetMessageToGroupAsync(string groupId)
    {
        var messages = _memoryCache.Get<IEnumerable<ChatMessageModel>>(
                $"{ChatCacheIdString.Messages}_{groupId}");
        
        if(messages is not null)
        {
            return messages.OrderBy(o => o.DateTimeSent);
        }

        return new List<ChatMessageModel>();
    }

    public Task AddMessageToGroupAsync(ChatMessageModel data)
    {
        var groupMessages = _memoryCache.Get<List<ChatMessageModel>>(
            $"{ChatCacheIdString.Messages}_{data.GroupId}");

        if (groupMessages is null)
        {
            groupMessages = new();
        }

        groupMessages.Add(data);

        _memoryCache.Set(
            $"{ChatCacheIdString.Messages}_{data.GroupId}",
            groupMessages,
            TimeSpan.FromMinutes(GROUP_MESSAGE_CACHE_MINUTES)
        );

        return Task.CompletedTask;
    }
}
