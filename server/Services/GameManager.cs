using System.Collections.Concurrent;
using server.Models;

namespace server.Services
{
    public class GameManager : IGameManager
    {
        // Lier un id (string) à une partie
        ConcurrentDictionary<string, TicTacToeGame> games = new ConcurrentDictionary<string, TicTacToeGame>();

        /// <summary>
        /// Créer une partie
        /// </summary>
        /// <param name="hostId"></param>
        /// <returns>Le gameId de la partie crée</returns>
        public string CreateGame(string hostId)
        {
            string gameId = Guid.NewGuid().ToString("N")[..6].ToUpper();
            games.TryAdd(gameId, new TicTacToeGame(hostId, gameId));
            return gameId;
        }

        /// <summary>
        /// Rejoint une partie
        /// </summary>
        /// <param name="gameId">gameId de la partie à rejoindre</param>
        /// <returns>True si on veut rejoindre une partie existante, sinon false</returns>
        public bool JoinGame(string gameId)
        {   
            if (games.ContainsKey(gameId))
            {
                return true;
            }

            return false;
        }

    }
}