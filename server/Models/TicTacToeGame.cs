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
        public bool IsGameOver { get; private set; }
        public string? Winner {get; private set; }

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
            if (!IsStarted || IsGameOver) return false;
            if (playerId != CurrentTurn) return false;
            if (cellIndex < 0 || cellIndex > 8 || Board[cellIndex] != null) return false; // Case déjà prise

            string symbol = (playerId == HostId) ? "X" : "O";
            Board[cellIndex] = symbol;

            CheckWinOrDraw(symbol);


            // Changer le tour
            CurrentTurn = (playerId == HostId) ? OpponentId! : HostId;

            return true;
        }

        private void CheckWinOrDraw(string symbol)
        {
            // Les 8 lignes gagnantes possibles
            int[][] winningLines = 
            [
                [0, 1, 2], [3, 4, 5], [6, 7, 8], // Horizontales
                [0, 3, 6], [1, 4, 7], [2, 5, 8], // Verticales
                [0, 4, 8], [2, 4, 6]             // Diagonales
            ];

            // Vérifier la victoire
            foreach (var line in winningLines)
            {
                if (Board[line[0]] == symbol && Board[line[1]] == symbol && Board[line[2]] == symbol)
                {
                    IsGameOver = true;
                    Winner = symbol;
                    return;
                }
            }

            // Vérifier l'égalité (grille pleine)
            if (!Board.Contains(null))
            {
                IsGameOver = true;
                Winner = "Draw";
            }
        }
    }
}