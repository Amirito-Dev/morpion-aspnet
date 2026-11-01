namespace server.Services
{
    public interface IGameManager
    {
        // Créer une partie et obtenir un gameId
        string CreateGame(string hostId);
        // Essayer de rejoindre une partie via un gameId
        bool JoinGame(string gameId);
    }
}