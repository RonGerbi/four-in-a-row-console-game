namespace FourInARow.Core
{
    public struct Player
    {
        private Coins m_Coin;
        private int m_points;

        public Player(Coins i_Coin)
        {
            m_Coin = i_Coin;
            m_points = 0;
        }

        public Coins PlayerCoin
        {
            get
            {
                return m_Coin;
            }
        }

        public int Points
        {
            get
            {
                return m_points;
            }
            set
            {
                m_points = value;
            }
        }

        public override string ToString()
        {
            string playerToString = string.Format("{0} player", m_Coin.ToString());

            return playerToString;
        }
    }
}
