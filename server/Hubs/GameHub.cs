using System.Text.RegularExpressions;
using System.Xml;
using Microsoft.AspNetCore.SignalR;
using server.Models;
using server.Services;

namespace server.Hubs
{
    public class GameHub : Hub
    {
        private readonly IGameManager _gameManager;

        public GameHub(IGameManager gameManager)
        {
            _gameManager = gameManager;
        }

        public async Task<string> CreateGame()
        {
            string gameId = _gameManager.CreateGame(Context.ConnectionId);
            await Groups.AddToGroupAsync(Context.ConnectionId, gameId);
            return gameId;
        }

        public async Task<bool> JoinGame(string gameId)
        {
            if (_gameManager.JoinGame(gameId, Context.ConnectionId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, gameId);
                var game = _gameManager.GetGameByPlayerId(Context.ConnectionId);

                // Informer les deux joueurs que la partie commence
                await Clients.Client(game!.HostId).SendAsync("GameStarted", new { symbol = "X" });
                await Clients.Client(game.OpponentId!).SendAsync("GameStarted", new { symbol = "O" });

                await Clients.Group(gameId).SendAsync("UpdateTurn", game.CurrentTurn);
                
                return true;
            }
            return false;
        }

        public async Task PlayTurn(int cellIndex)
        {
            var game = _gameManager.GetGameByPlayerId(Context.ConnectionId);
            if (game == null) return;

            // Vérifier si le tour est valide
            if (game.PlayTurn(Context.ConnectionId, cellIndex))
            {
                string symbol = (Context.ConnectionId == game.HostId) ? "X" : "O";

                // On dit à tout le monde quelle case vient d'être remplie
                await Clients.Group(game.GameId).SendAsync("TurnPlayed", new { cell = cellIndex, symbol = symbol });

                if (game.IsGameOver)
                {
                    await Clients.Group(game.GameId).SendAsync("GameOver", game.Winner);

                }
                else{
                    // On dit à tout le monde à qui c'est le tour maintenant
                    await Clients.Group(game.GameId).SendAsync("UpdateTurn", game.CurrentTurn);
                }
            }
        }

        public async Task SendMessage(string message)
        {
            var game = _gameManager.GetGameByPlayerId(Context.ConnectionId);
            if(game == null) return;
            string symbol = (Context.ConnectionId == game.HostId) ? "X" : "O";
            Console.WriteLine("Message : " + message + " sent by user " + symbol);
            await Clients.OthersInGroup(game.GameId).SendAsync("ReceiveMessage", message);
        }
    }
}