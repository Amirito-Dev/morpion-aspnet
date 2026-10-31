using System.Xml;
using Microsoft.AspNetCore.SignalR;

namespace server.Hubs
{
    public class GameHub : Hub
    {
        public async Task<string> CreateGame()
        {
            // Générer un code de partie
            string gameId = Guid.NewGuid().ToString("N")[..6].ToUpper();
            await Groups.AddToGroupAsync(Context.ConnectionId, gameId);
            Console.WriteLine($"Partie crée avec le code : {gameId}");
            return gameId;
        }

        public async Task JoinGame(string gameId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, gameId);
            await Clients.OthersInGroup(gameId).SendAsync("PlayerJoined", Context.ConnectionId);
            await Clients.Caller.SendAsync("JoinedSuccessfully", gameId);
            Console.WriteLine($"Partie rejoint avec le code : {gameId}");
        }
    }
}