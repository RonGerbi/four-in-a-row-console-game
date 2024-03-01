namespace FourInARow.Core
{
    public class SmartBoardSquare
    {
        private Coins m_Coin;
        private SquareScoreManager[] m_SquareScores;

        public SmartBoardSquare(Coins i_Coin, SquareScoreManager[] i_SquareScores)
        {
            m_Coin = i_Coin;

            if (i_SquareScores != null)
            {
                m_SquareScores = new SquareScoreManager[i_SquareScores.Length];

                for (int i = 0; i < i_SquareScores.Length; i++)
                {
                    m_SquareScores[i] = new SquareScoreManager();
                }
            }
        }

        public Coins Coin
        {
            get
            {
                return m_Coin;
            }
            set
            {
                m_Coin = value;
            }
        }

        public int?[] Scores
        {
            get
            {
                return m_SquareScores;
            }
            set
            {
                m_SquareScores = value;
            }
        }
    }
}
