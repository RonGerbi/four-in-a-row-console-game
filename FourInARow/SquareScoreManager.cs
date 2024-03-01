namespace FourInARow.Core
{
    public class SquareScoreManager
    {
        const byte k_Directions = 5;
        private int?[] m_RowSquareScores;
        private int?[] m_ColumnSquareScores;
        private int?[] m_TopLeftBottomRightSquareScores;
        private int?[] m_TopRightBottomLeftSquareScores;

        public SquareScoreManager()
        {
            m_RowSquareScores = new int?[k_Directions];
            m_ColumnSquareScores = new int?[k_Directions];
            m_TopLeftBottomRightSquareScores = new int?[k_Directions];
            m_TopRightBottomLeftSquareScores = new int?[k_Directions];
        }

        public int?[] RowSquareScores
        {
            get
            {
                return m_RowSquareScores;
            }
        }

        public int?[] ColumnSquareScores
        {
            get
            {
                return m_ColumnSquareScores;
            }
        }

        public int?[] TopLeftBottomRightSquareScores
        {
            get
            {
                return m_TopLeftBottomRightSquareScores;
            }
        }

        public int?[] TopRightBottomLeftSquareScores
        {
            get
            {
                return m_TopRightBottomLeftSquareScores;
            }
        }
    }
}
