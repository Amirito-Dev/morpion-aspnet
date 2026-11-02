namespace server.Models
{
    public class TicTacToeGame
    {
        public string HostId { get; private set; }
        public string? OpponentId { get; set; }
        public string GameId { get; private set; }
        public string CurrentTurn { get; private set; }
        
        public bool IsStarted { get; private set; }
        public string?[] Board { get; private set; }

        public TicTacToeGame(string hostId, string gameId)
        {
            HostId = hostId;
            GameId = gameId;
            Board = new string?[9];
            CurrentTurn = hostId; // L'hôte commence toujours
            IsStarted = false;
        }

        public void StartGame() => IsStarted = true;

        // Retourne true si le coup est valide et false sinon
        public bool PlayTurn(string playerId, int cellIndex)
        {
            if (!IsStarted) return false;
            if (playerId != CurrentTurn) return false;
            if (cellIndex < 0 || cellIndex > 8 || Board[cellIndex] != null) return false; // Case déjà prise

            Board[cellIndex] = (playerId == HostId) ? "X" : "O";

            // Changer le tour
            CurrentTurn = (playerId == HostId) ? OpponentId! : HostId;

            return true;
        }
    }
}