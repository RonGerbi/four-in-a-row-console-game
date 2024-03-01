namespace FourInARow.Core
{
    public struct Coins
    {
        private eCoins m_Coin;

        public Coins(eCoins coin)
        {
            m_Coin = coin;
        }

        public eCoins Coin
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

        public override string ToString()
        {
            string coinStr;

            switch (m_Coin)
            {
                case eCoins.X:
                    coinStr = "X";
                    break;
                case eCoins.O:
                    coinStr = "O";
                    break;
                default:
                    coinStr = "";
                    break;
            }

            return coinStr;
        }
    }
}
