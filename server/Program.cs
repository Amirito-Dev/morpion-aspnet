using server.Services;

var builder = WebApplication.CreateBuilder(args);

var corsPolicy = "AllowFrontend";

builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicy, policy =>
    {
        policy.SetIsOriginAllowed(origin => true) // Accepte localhost / Live Server / ports arbitraires en dev
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddSignalR();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IGameManager, GameManager>();

var app = builder.Build();

app.UseRouting();

app.UseCors(corsPolicy);

app.MapHub<server.Hubs.GameHub>("/gameHub");

app.Run();