var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRequiredServices();

var app = builder.Build();
app.AddRequiredBuilder(
    builder.Environment.IsProduction(),
    builder.Configuration.GetSection("AllowedOrigin").Get<string[]>() ?? [],
    builder.Configuration["TropaChatHubAPIKey"],
    builder.Configuration["TropaChatHubEndpoint"]
);

app.MapGroup("api").WithTags("Chat Groups").AddChatGroupEndpoints();

app.MapHub<ChatHub>($"/{builder.Configuration["TropaChatHubEndpoint"]}");
app.Run();
