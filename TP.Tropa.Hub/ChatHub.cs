using Microsoft.AspNetCore.SignalR;
using TP.Tropa.Domain;

public class ChatHub : Hub
{
    private static List<ChatGroupSubscriberModel> _subs = new ();
    private readonly IChatCacheLogic _logic;

    public ChatHub(IChatCacheLogic logic)
    {
        _logic = logic ?? 
            throw new ArgumentNullException(nameof(logic));
    }
    
    public override async Task OnConnectedAsync()
    {
        var nickname = Context.User.GetClaimStringValue(KeyClaimsString.Nickname);
        var groupId = Context.User.GetClaimStringValue(KeyClaimsString.GroupId);
        var subscriberData = _subs.FirstOrDefault(r => r.Nickname == nickname);

        if (subscriberData != null && subscriberData.GroupId == groupId)
        {
            subscriberData?.ConnectionId?.Add(Context.ConnectionId);
        }
        else
        {
            _subs.Add(new ChatGroupSubscriberModel
            {
                ConnectionId = new List<string>() { Context.ConnectionId },
                Nickname = nickname,
                GroupId = groupId,
                DateTimeJoined = DateTimeOffset.UtcNow,
            });
        }

        await Groups.AddToGroupAsync(Context.ConnectionId, nickname);
        await PublishedPreviousMessagesInGroup(groupId);
        _ = PublishedGroupSubscribers(groupId);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        var subscriberData = _subs.FirstOrDefault(r => 
            r.Nickname == Context.User.GetClaimStringValue(KeyClaimsString.Nickname));
        
        if (subscriberData is not null)
        {
            subscriberData.ConnectionId.ForEach(f => {
                Groups.RemoveFromGroupAsync(f, subscriberData.Nickname);
            });
            _subs.Remove(subscriberData);
            await PublishedGroupSubscribers(subscriberData.GroupId);
        }

        await base.OnDisconnectedAsync(exception);
    }

    [HubMethodName(ChatMethodString.GetGroupSubscribers)]
    public Task GetGroupSubscribers(string groupId)
    {
        PublishedGroupSubscribers(groupId);

        return Task.CompletedTask;
    }

    public Task PublishedGroupSubscribers(string groupId)
    {
        var usernames = _subs
            .Where(w => w.GroupId == groupId)
            .Select(s => s.Nickname);
            
        Clients.Groups(usernames).SendAsync(
            ChatMethodString.PublishedGroupSubscribers, 
            usernames);
            
        return Task.CompletedTask;
    }

    public async Task PublishedPreviousMessagesInGroup(string groupId)
    {
        var usernames = _subs
            .Where(w => w.GroupId == groupId)
            .Select(s => s.Nickname);

        var message = await _logic.GetMessageToGroupAsync(groupId);
            
        await Clients.Groups(usernames).SendAsync(
            ChatMethodString.PublishedPreviousMessagesInGroup, 
            message);
    }

    [HubMethodName(ChatMethodString.SendChatMessageToGroupAsSubscriber)]
    public Task SendChatMessageToGroupAsSubscriber(ChatMessageModel cm)
    {
        _ = _logic.AddMessageToGroupAsync(cm);
        
        PublishChatMessageFromSubscriber(cm);

        return Task.CompletedTask;
    }

    public Task PublishChatMessageFromSubscriber(ChatMessageModel cm)
    {
        var usernames = _subs
            .Where(w => w.GroupId == cm.GroupId)
            .Select(s => s.Nickname);
                
        Clients.Groups(usernames).SendAsync(
            ChatMethodString.PublishedChatMessageFromSubscriber, 
            cm);

        return Task.CompletedTask;
    }
}