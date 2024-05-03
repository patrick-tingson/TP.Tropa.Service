using TP.Tropa.Domain;

public static class ChatGroupEndpointRouteBuilderExtensions
{
    public static void AddChatGroupEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/v1/group", async (
            string id, 
            HttpContext context,
            IChatCacheLogic logic
        ) => {
            var groupData = await logic.GetGroupAsync(id);

            if(groupData is null) 
            {
                context.Response.StatusCode = 404;
                return;
            }

            await context.Response.WriteAsJsonAsync(groupData);
        }).Produces<ChatGroupModel>(200)
            .Produces(400)
            .Produces(401)
            .Produces(404)
            .Produces(500);

        app.MapPost("/v1/group", async (IChatCacheLogic logic) => await logic.CreateGroupAsync())
            .Produces<ChatGroupModel>(200)
            .Produces(401)
            .Produces(500);
    }
}
