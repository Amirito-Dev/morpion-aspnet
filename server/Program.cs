using server.Services;

var builder = WebApplication.CreateBuilder(args);

var corsPolicy = "AllowFrontend";

builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicy, policy =>
    {
        policy.SetIsOriginAllowed(origin => true)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddSignalR();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton<IGameManager, GameManager>();

var app = builder.Build();

app.UseRouting();

app.UseCors(corsPolicy);
// Cherche index.html dans wwwroot
app.UseDefaultFiles();
// Autoriser le serveur à distribuer de l'html
app.UseStaticFiles();

app.MapHub<server.Hubs.GameHub>("/tictactoe");

app.Run();