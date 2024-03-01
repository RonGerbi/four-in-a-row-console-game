namespace FourInARow.Core
{
    public class GameManager
    {
        private Board m_GameBoard;
        private GameModes? m_GameMode;
        private Player[] m_Players;
        private int m_CurrentPlayer;

        public GameManager(byte i_BoardRowLength, byte i_BoardColumnLength, GameModes? i_GameMode)
        {
            Coins firstPlayerCoin = new Coins(eCoins.X);
            Coins secondPlayerCoin = new Coins(eCoins.O);
            m_GameMode = i_GameMode;

            createPlayers(firstPlayerCoin, secondPlayerCoin);
            setStartingPlayer();

            m_GameBoard = new Board(i_BoardRowLength, i_BoardColumnLength);
        }

        public Board GameBoard
        {
            get
            {
                return m_GameBoard;
            }
        }

        public GameModes? GameMode
        {
            get
            {
                return m_GameMode;
            }
        }

        public Player CurrentPlayer
        {
            get
            {
                return m_Players[m_CurrentPlayer];
            }
        }

        public Player XPlayer
        {

            get
            {
                return m_Players[0];
            }
        }

        public Player OPlayer
        {

            get
            {
                return m_Players[1];
            }
        }

        public void SwitchTurn()
        {
            m_CurrentPlayer = (m_CurrentPlayer + 1) % m_Players.Length;
        }

        public void Reset()
        {
            m_GameBoard = new Board((byte)GameBoard.BoardMatrix.GetLength(0), (byte)GameBoard.BoardMatrix.GetLength(1));
            setStartingPlayer();
        }

        public bool GameIsOver(out Player? o_Winner)
        {
            bool gameOver = false;
            o_Winner = null;

            if (m_GameBoard.IsBoardFull() || gameIsWon(out o_Winner))
            {
                gameOver = true;
            }

            return gameOver;
        }

        public bool TryToDropCoin(int i_Column)
        {
            bool isAIGame = m_GameMode.HasValue && m_GameMode.Value.GameMode == eGameModes.Solo;

            return m_GameBoard.TryToDropCoin(m_Players[m_CurrentPlayer], i_Column, isAIGame);
        }

        public void AddAPointToPlayer(Player? i_Player)
        {
            if (i_Player.HasValue)
            {
                Player player = i_Player.Value;

                switch (player.PlayerCoin.Coin)
                {
                    case eCoins.X:
                        m_Players[0].Points++;
                        break;
                    case eCoins.O:
                        m_Players[1].Points++;
                        break;
                }
            }
        }

        public int GetAIColumnChoice()
        {
            int topScore = int.MinValue;
            int smartColumnChoiceIndex = 0;

            for (int i = 0; i < m_GameBoard.BoardMatrix.GetLength(1); i++)
            {
                if (m_GameBoard.TopCoinPerColumn[i].Score > topScore)
                {
                    topScore = m_GameBoard.TopCoinPerColumn[i].Score;
                    smartColumnChoiceIndex = i;
                }
            }

            return smartColumnChoiceIndex;
        }

        private bool gameIsWon(out Player? o_Winner)
        {
            Coins winnerCoin;
            bool isWon;

            isWon = m_GameBoard.IsFourInARow(out winnerCoin) ||
                m_GameBoard.IsFourInAColumn(out winnerCoin) ||
                m_GameBoard.IsFourInTopLeftToBottomRightDiagonal(out winnerCoin) ||
                m_GameBoard.IsFourInTopRightToBottomLeftDiagonal(out winnerCoin);
            o_Winner = getWinner(winnerCoin);

            return isWon;
        }

        private Player? getWinner(Coins i_WinnerCoin)
        {
            Player? winner = null;

            if (i_WinnerCoin.Coin != eCoins.Empty)
            {
                switch (i_WinnerCoin.Coin)
                {
                    case eCoins.X:
                        winner = m_Players[0];
                        break;
                    case eCoins.O:
                        winner = m_Players[1];
                        break;
                    default:
                        winner = null;
                        break;
                }
            }

            return winner;
        }

        private void createPlayers(Coins i_FirstPlayerCoin, Coins i_SecondPlayerCoin)
        {
            m_Players = new Player[2];
            m_Players[0] = new Player(i_FirstPlayerCoin);
            m_Players[1] = new Player(i_SecondPlayerCoin);
        }

        private void setStartingPlayer()
        {
            m_CurrentPlayer = 0;
        }
    }
}
