namespace server.Models
{
    public class TicTacToeGame
    {
        string hostId;
        string gameId;
        //string opponentId;
        bool started;

        public TicTacToeGame(string hostId, string gameId)
        {
            this.hostId = hostId;
            this.gameId = gameId;
            this.started = false;
        }
    }
}