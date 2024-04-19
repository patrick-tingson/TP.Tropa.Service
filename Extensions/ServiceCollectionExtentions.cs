using Microsoft.OpenApi.Models;

public static class ServiceCollectionExtentions
{
    public static void AddRequiredServices(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Tropa Chat Service", Version = "v1.0.0" });
            c.AddSecurityDefinition("TPAPIKey", new OpenApiSecurityScheme
            {
                Description = "Enter your API key in the 'TPAPIKey' header",
                Type = SecuritySchemeType.ApiKey,
                In = ParameterLocation.Header,
                Name = "TPAPIKey"
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "TPAPIKey"
                        },
                        Type = SecuritySchemeType.ApiKey,
                        In = ParameterLocation.Header,
                        Name = "TPAPIKey"
                    },
                    Array.Empty<string>()
                }
            });
        });
        services.AddHttpClient();
        services.AddMemoryCache();
        services.AddCors();
        services.AddSingleton<IChatCacheLogic, ChatCacheLogic>();
        services.AddSingleton<IChatCacheService, ChatCacheService>();
        services.AddSingleton<ChatHub>();
        services.AddSignalR();
    }
}