using System.Xml;
using Microsoft.AspNetCore.SignalR;
using server.Services;

namespace server.Hubs
{
    public class GameHub : Hub
    {

        private readonly IGameManager gameManagerInstance;

        public GameHub(IGameManager gameManagerInstance)
        {
            this.gameManagerInstance = gameManagerInstance;
        }

        public async Task<string> CreateGame()
        {
            // Générer un code de partie
            string gameId = gameManagerInstance.CreateGame(Context.ConnectionId);
            gameManagerInstance.AssignPlayerToGame(Context.ConnectionId, gameId);
            await Groups.AddToGroupAsync(Context.ConnectionId, gameId);
            Console.WriteLine($"Partie crée avec le code : {gameId}");
            return gameId;
        }

        public async Task<bool> JoinGame(string gameId)
        {
            if (gameManagerInstance.JoinGame(gameId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, gameId);
                await Clients.OthersInGroup(gameId).SendAsync("PlayerJoined", Context.ConnectionId);
                await Clients.Caller.SendAsync("JoinedSuccessfully", gameId);
                gameManagerInstance.AssignPlayerToGame(Context.ConnectionId, gameId);
                Console.WriteLine($"Partie rejoint avec le code : {gameId}");
                return true;
            }

            Console.WriteLine("Partie avec le code " + gameId + " non existante");
            return false;
        }
    }
}