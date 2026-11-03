using System.Collections.Concurrent;
using server.Models;

namespace server.Services
{
   public class GameManager : IGameManager
    {
        private ConcurrentDictionary<string, TicTacToeGame> games = new();
        private ConcurrentDictionary<string, string> gamePlayers = new();

        public string CreateGame(string hostId)
        {
            string gameId = Guid.NewGuid().ToString("N")[..6].ToUpper();
            games.TryAdd(gameId, new TicTacToeGame(hostId, gameId));
            gamePlayers.TryAdd(hostId, gameId);
            return gameId;
        }

        public bool JoinGame(string gameId, string playerId)
        {   
            if (games.TryGetValue(gameId, out var game))
            {
                lock (game){
                    if (game.OpponentId != null) return false; // La partie est déjà pleine

                    game.OpponentId = playerId;
                    gamePlayers.TryAdd(playerId, gameId);
                    game.StartGame();
                    return true;
                }

            }
            return false;
        }

        public TicTacToeGame? GetGameByPlayerId(string playerId)
        {
            if (gamePlayers.TryGetValue(playerId, out string? gameId))
            {
                games.TryGetValue(gameId, out var game);
                return game;
            }
            return null;
        }

        public void RemoveGame(string gameId)
        {
            // Si la partie existe, on la retire du dictionnaire des parties
            if (games.TryRemove(gameId, out var game))
            {
                // On retire l'hôte du dictionnaire de liaison joueur -> partie
                gamePlayers.TryRemove(game.HostId, out _);

                // On retire l'adversaire s'il avait déjà rejoint la partie
                if (game.OpponentId != null)
                {
                    gamePlayers.TryRemove(game.OpponentId, out _);
                }
            }
        }
    }
}