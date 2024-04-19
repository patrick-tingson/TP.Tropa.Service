using System.Security.Claims;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Primitives;
using TP.Tropa.Domain;
public static class ApplicationBuilderExtensions
{
    public static void AddRequiredBuilder(
        this IApplicationBuilder app,
        bool isProduction,
        string[] allowedOrigins,
        string tpApiKey,
        string chatHubEndpoint)
    {
        if(tpApiKey is null) 
            throw new ArgumentNullException(nameof(tpApiKey));
        if(allowedOrigins is null) 
            throw new ArgumentNullException(nameof(allowedOrigins));
        if(chatHubEndpoint is null) 
            throw new ArgumentNullException(nameof(chatHubEndpoint));

        app.UseCors(builder => builder
            .WithOrigins(allowedOrigins)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseHttpsRedirection();
        app.UseExceptionHandler(c => c.Run(async context =>
        {
            var exception = context.Features
                .Get<IExceptionHandlerFeature>()?.Error;

            if (exception is not null)
            {
                var response = new ResponseModel
                {
                    Status = 500,
                    Description = exception.Message,
                    Stacktrace = !isProduction ? exception.StackTrace : null
                };

                context.Response.StatusCode = 500;

                if (exception.GetType() == typeof(ArgumentNullException))
                {
                    response.Status = 400;
                    response.Description = exception.Message;

                    context.Response.StatusCode = 400;
                }

                await context.Response.WriteAsJsonAsync(response);
            }
        }));
        app.Use(async (context, next) =>
        {
            if (context.Request.Path.StartsWithSegments($"/{chatHubEndpoint}"))
            {
                var apiKey = context.Request.Query["k"];
                var nickname = context.Request.Query["nn"];
                var groupId = context.Request.Query["gi"];

                if (!string.IsNullOrEmpty(apiKey)
                    && !string.IsNullOrEmpty(nickname)
                    && !string.IsNullOrEmpty(groupId)
                    && tpApiKey == apiKey)
                {
                    var claimsIdentity = new ClaimsIdentity();
                    claimsIdentity.AddClaim(new Claim(KeyClaimsString.Nickname, nickname!));
                    claimsIdentity.AddClaim(new Claim(KeyClaimsString.GroupId, groupId!));
                    context.User.AddIdentity(claimsIdentity);
                }
                else 
                {
                    context.Response.StatusCode = 401;
                    return;
                }

                await next();
                return;
            }

            if (!context.Request.Headers.TryGetValue("TPAPIKey", out StringValues providedApiKey))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("API key is missing.");
                return;
            }

            if (!string.Equals(tpApiKey, providedApiKey, StringComparison.OrdinalIgnoreCase))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Invalid API key.");
                return;
            }

            await next();
        });
    }
}
