namespace FourInARow
{
    public class ColumnTracker
    {
        private int m_InsertIndex;
        private int m_Score;

        public ColumnTracker(int i_NextInsertionIndex)
        {
            m_InsertIndex = i_NextInsertionIndex;
            m_Score = 1;
        }

        internal int InsertIndex
        {
            get
            {
                return m_InsertIndex;
            }
            set
            {
                m_InsertIndex = value;
            }
        }

        internal int Score
        {
            get
            {
                return m_Score;
            }
            set
            {
                m_Score = value;
            }
        }
    }
}
