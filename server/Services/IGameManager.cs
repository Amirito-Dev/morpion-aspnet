using server.Models;

namespace server.Services
{
    public interface IGameManager
    {
        public string CreateGame(string hostId);
        public bool JoinGame(string gameId, string playerId);
        public TicTacToeGame? GetGameByPlayerId(string playerId);
    }
}