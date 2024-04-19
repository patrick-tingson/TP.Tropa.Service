### Tropa Chat Messaging Server with .NET 8 Minimal API and SignalR
This project implements a real-time chat messaging server using ASP.NET 8 Minimal API, SignalR, and an in-memory cache for temporary data storage (for demonstration purposes). The code adheres to an N-Tier architecture and incorporates authentication and authorization mechanisms.

------------
##### Prerequisites:
- .NET 8 SDK (https://dotnet.microsoft.com/en-us/download)
- Basic understanding of ASP.NET Minimal API, SignalR, N-Tier architecture, and caching

------------
##### Tech Stack:
- ASP.NET 8 Minimal API
- SignalR
- Caching library (choose one based on preference): https://learn.microsoft.com/en-us/dotnet/api/system.runtime.caching.memorycache?view=dotnet-plat-ext-8.0 - Microsoft.Extensions.Caching.Memory
- Optional: Entity Framework Core (for future persistence layer): https://learn.microsoft.com/en-us/ef/core/get-started/overview/install

------------

##### Features:
- Real-time chat functionality using SignalR
- User authentication and authorization (implementation details omitted for brevity)
- N-Tier architecture separates concerns (Models, Controllers, Hubs, Services, Repositories)
- In-memory cache acts as a temporary database (for demo)
- (Optional) Future integration with a persistent database (e.g., SQL Server, CosmosDb) using Entity Framework Core

------------

##### Running the application:
- Restore NuGet packages: dotnet restore
- Run the server: dotnet run
- Access the application using a web client that supports SignalR (e.g., localhost:5000/)

------------

##### Notes
- This is a basic implementation for demonstration purposes.
- Authentication and authorization details are omitted for brevity. You'll need to implement them based on your chosen approach (e.g., JWT, IdentityServer).
- The in-memory cache is for demo only. Consider a persistent database for production environments.
- Error handling and logging are not included in this basic example.

------------

##### Further Enhancements:
- Implement user presence and typing indicators.
- Integrate private messaging functionality.
- Add file upload capabilities.
- Persist chat history in a database.

------------


##### Refer to the following resources for more details on the used technologies:
- ASP.NET 8 Minimal API: https://learn.microsoft.com/en-us/aspnet/core/tutorials/min-web-api?view=aspnetcore-8.0
- SignalR: https://learn.microsoft.com/en-us/aspnet/signalr/
- N-Tier Architecture: https://en.wikipedia.org/wiki/Multitier_architecture
- In-memory Caching: https://learn.microsoft.com/en-us/dotnet/api/system.runtime.caching.memorycache?view=dotnet-plat-ext-8.0
- Entity Framework Core: https://learn.microsoft.com/en-us/ef/core/get-started/overview/install

------------


### End