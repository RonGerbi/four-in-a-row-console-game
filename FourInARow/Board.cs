using System;

namespace FourInARow.Core
{
    public class Board
    {
        private Coins[,] m_BoardMatrix;
        private ColumnTracker[] m_TopCoinPerColumn;
        private const byte k_MaxRowLength = 8;
        private const byte k_MaxColumnLength = 8;
        private const byte k_MinRowLength = 4;
        private const byte k_MinColumnLength = 4;

        public Board(byte i_RowLength, byte i_ColumnLength)
        {
            buildGameBoard(i_RowLength, i_ColumnLength);
        }

        public Coins[,] BoardMatrix
        {
            get
            {
                return m_BoardMatrix;
            }
        }

        public ColumnTracker[] TopCoinPerColumn
        {
            get
            {
                return m_TopCoinPerColumn;
            }
        }

        public static byte MaxRowLength
        {
            get
            {
                return k_MaxRowLength;
            }
        }

        public static byte MaxColumnLength
        {
            get
            {
                return k_MaxColumnLength;
            }
        }

        public static byte MinRowLength
        {
            get
            {
                return k_MinRowLength;
            }
        }

        public static byte MinColumnLength
        {
            get
            {
                return k_MinColumnLength;
            }
        }

        public bool IsBoardFull()
        {
            bool isFull = true;
            int boardColumns = m_BoardMatrix.GetLength(1);

            for (int i = 0; i < boardColumns; i++)
            {
                if (!IsColumnFull(i))
                {
                    isFull = false;
                    break;
                }
            }

            return isFull;
        }

        public bool IsColumnFull(int i_ColumnIndex)
        {
            return m_TopCoinPerColumn[i_ColumnIndex].InsertIndex < 0;
        }

        public bool IsFourInARow(out Coins o_WinnerCoin)
        {
            int rowLength = m_BoardMatrix.GetLength(0);
            int columnLength = m_BoardMatrix.GetLength(1);
            bool hasFourInRow = false;
            o_WinnerCoin = new Coins(eCoins.Empty);

            for (int i = 0; i < rowLength; i++)
            {
                for (int j = 0; j < columnLength - 3; j++)
                {
                    if (m_BoardMatrix[i, j].Coin != eCoins.Empty &&
                        m_BoardMatrix[i, j].Coin == m_BoardMatrix[i, j + 1].Coin &&
                        m_BoardMatrix[i, j].Coin == m_BoardMatrix[i, j + 2].Coin &&
                        m_BoardMatrix[i, j].Coin == m_BoardMatrix[i, j + 3].Coin)
                    {
                        o_WinnerCoin.Coin = m_BoardMatrix[i, j].Coin;
                        hasFourInRow = true;
                    }
                }
            }

            return hasFourInRow;
        }

        public bool IsFourInAColumn(out Coins o_WinnerCoin)
        {
            int rowLength = m_BoardMatrix.GetLength(0);
            int columnLength = m_BoardMatrix.GetLength(1);
            bool hasFourInColumn = false;
            o_WinnerCoin = new Coins(eCoins.Empty);

            for (int j = 0; j < columnLength; j++)
            {
                for (int i = 0; i < rowLength - 3; i++)
                {
                    if (m_BoardMatrix[i, j].Coin != eCoins.Empty &&
                        m_BoardMatrix[i, j].Coin == m_BoardMatrix[i + 1, j].Coin &&
                        m_BoardMatrix[i, j].Coin == m_BoardMatrix[i + 2, j].Coin &&
                        m_BoardMatrix[i, j].Coin == m_BoardMatrix[i + 3, j].Coin)
                    {
                        o_WinnerCoin.Coin = m_BoardMatrix[i, j].Coin;
                        hasFourInColumn = true;
                    }
                }
            }

            return hasFourInColumn;
        }

        public bool IsFourInTopLeftToBottomRightDiagonal(out Coins o_WinnerCoin)
        {
            int rowLength = m_BoardMatrix.GetLength(0);
            int columnLength = m_BoardMatrix.GetLength(1);
            bool hasFourInDiagonal = false;
            o_WinnerCoin = new Coins(eCoins.Empty);

            for (int i = 0; i < rowLength - 3; i++)
            {
                for (int j = 0; j < columnLength - 3; j++)
                {
                    if (m_BoardMatrix[i, j].Coin != eCoins.Empty &&
                        m_BoardMatrix[i, j].Coin == m_BoardMatrix[i + 1, j + 1].Coin &&
                        m_BoardMatrix[i, j].Coin == m_BoardMatrix[i + 2, j + 2].Coin &&
                        m_BoardMatrix[i, j].Coin == m_BoardMatrix[i + 3, j + 3].Coin)
                    {
                        o_WinnerCoin.Coin = m_BoardMatrix[i, j].Coin;
                        hasFourInDiagonal = true;
                    }
                }
            }

            return hasFourInDiagonal;
        }

        public bool IsFourInTopRightToBottomLeftDiagonal(out Coins o_WinnerCoin)
        {
            int rowLength = m_BoardMatrix.GetLength(0);
            int columnLength = m_BoardMatrix.GetLength(1);
            bool hasFourInDiagonal = false;
            o_WinnerCoin = new Coins(eCoins.Empty);

            for (int i = 0; i < rowLength - 3; i++)
            {
                for (int j = 3; j < columnLength; j++)
                {
                    if (m_BoardMatrix[i, j].Coin != eCoins.Empty &&
                        m_BoardMatrix[i, j].Coin == m_BoardMatrix[i + 1, j - 1].Coin &&
                        m_BoardMatrix[i, j].Coin == m_BoardMatrix[i + 2, j - 2].Coin &&
                        m_BoardMatrix[i, j].Coin == m_BoardMatrix[i + 3, j - 3].Coin)
                    {
                        o_WinnerCoin.Coin = m_BoardMatrix[i, j].Coin;
                        hasFourInDiagonal = true;
                    }
                }
            }

            return hasFourInDiagonal;
        }

        public bool IsValidColumn(int i_UserColumnChoice)
        {
            return i_UserColumnChoice >= 0 && i_UserColumnChoice < m_BoardMatrix.GetLength(1);
        }

        internal bool TryToDropCoin(Player i_CurrentPlayer, int i_Column, bool i_IsAIGame)
        {
            bool droppedCoin = true;

            if (!columnIsInRange(i_Column) || IsColumnFull(i_Column))
            {
                droppedCoin = false;
            }
            else
            {
                insertCoin(i_CurrentPlayer, i_Column, i_IsAIGame);
            }

            return droppedCoin;
        }

        public static bool TryExtractingLengthsOfRowsAndCols(string i_BoardSize, out byte o_RowLength, out byte o_ColumnLength)
        {
            bool extractionSucceeded = true;
            byte iterator = 0;
            o_RowLength = 0;
            o_ColumnLength = 0;

            if (!tryExtractLengthFromBoardSizeString(i_BoardSize, ref iterator, ref o_RowLength) ||
                !tryExtractLengthFromBoardSizeString(i_BoardSize, ref iterator, ref o_ColumnLength) ||
                !validBoardSize(o_RowLength, o_ColumnLength,
                k_MinRowLength, k_MaxRowLength,
                k_MinColumnLength, k_MaxColumnLength))
            {
                extractionSucceeded = false;
            }

            return extractionSucceeded;
        }

        private void buildGameBoard(byte i_RowLength, byte i_ColumnLength)
        {
            m_BoardMatrix = new Coins[i_RowLength, i_ColumnLength];

            for (int i = 0; i < i_RowLength; i++)
            {
                for (int j = 0; j < i_ColumnLength; j++)
                {
                    m_BoardMatrix[i, j] = new Coins(eCoins.Empty);
                }
            }

            initializeTopCoinPerColumn(i_ColumnLength);
        }

        private void initializeTopCoinPerColumn(byte i_ColumnLength)
        {
            int bottomRow = m_BoardMatrix.GetLength(0) - 1;
            m_TopCoinPerColumn = new ColumnTracker[i_ColumnLength];

            for (byte i = 0; i < i_ColumnLength; i++)
            {
                m_TopCoinPerColumn[i] = new ColumnTracker(bottomRow);
            }
        }

        private bool columnIsInRange(int i_Column)
        {
            return i_Column >= 0 && i_Column < m_BoardMatrix.GetLength(1);
        }

        private bool insertCoin(Player i_CurrentPlayer, int i_Column, bool i_AgainstAI)
        {
            bool coinInsertedSuccessfully;

            if (IsValidColumn(i_Column))
            {
                m_BoardMatrix[m_TopCoinPerColumn[i_Column].InsertIndex, i_Column] = i_CurrentPlayer.PlayerCoin;
                m_TopCoinPerColumn[i_Column].InsertIndex--;

                if (i_AgainstAI)
                {
                    updateColumnScores();
                }

                coinInsertedSuccessfully = true;
            }
            else
            {
                coinInsertedSuccessfully = false;
            }

            return coinInsertedSuccessfully;
        }

        private void updateColumnScores()
        {
            for (int i = 0; i < m_TopCoinPerColumn.Length; i++)
            {
                if (IsColumnFull(i))
                {
                    m_TopCoinPerColumn[i].Score = int.MinValue;
                }
                else
                {
                    int score = Math.Max(getColumnScore(i), getRowScore(i));
                    score = Math.Max(score, getDiagonalScore(i));
                    m_TopCoinPerColumn[i].Score = score;
                }
            }
        }

        private int getDiagonalScore(int i_ColumnIndex)
        {
            int nextInsertionIndex = m_TopCoinPerColumn[i_ColumnIndex].InsertIndex;
            int score;

            if (IsValidColumn(i_ColumnIndex - 1) && IsValidColumn(i_ColumnIndex + 1) &&
                !rowIsOutOfBoard(nextInsertionIndex + 1) && !rowIsOutOfBoard(nextInsertionIndex - 1) &&
                isSameCoin(m_BoardMatrix[nextInsertionIndex + 1, i_ColumnIndex - 1].Coin, m_BoardMatrix[nextInsertionIndex - 1, i_ColumnIndex + 1].Coin))
            {
                score = Math.Max(getContinuousBottomLeftTopRightDiagonalScore(i_ColumnIndex),
                    getContinuousTopLeftBottomRightDiagonalScore(i_ColumnIndex));
            }
            else
            {
                score = Math.Max(getGreaterBottomLeftTopRightDiagonalSideScore(i_ColumnIndex),
                    getGreaterTopLeftBottomRightDiagonalScore(i_ColumnIndex));
            }

            return score;
        }

        private int getGreaterBottomLeftTopRightDiagonalSideScore(int i_ColumnIndex)
        {
            eCoins scoredCoin;
            int currentRow = m_TopCoinPerColumn[i_ColumnIndex].InsertIndex;
            int leftColumn = i_ColumnIndex - 1;
            int rightColumn = i_ColumnIndex + 1;
            int leftScore = 1;
            int rightScore = 1;

            if (!rowIsOutOfBoard(currentRow + 1) && IsValidColumn(leftColumn))
            {
                scoredCoin = m_BoardMatrix[currentRow + 1, leftColumn].Coin;

                leftScore = scoreBottomLeftDiagonal(scoredCoin, currentRow, leftColumn);
            }

            if (!rowIsOutOfBoard(currentRow - 1) && IsValidColumn(rightColumn))
            {
                scoredCoin = m_BoardMatrix[currentRow - 1, rightColumn].Coin;

                rightScore = scoreTopRightDiagonal(scoredCoin, currentRow, rightColumn);
            }

            return Math.Max(rightScore, leftScore);
        }

        private int getGreaterTopLeftBottomRightDiagonalScore(int i_ColumnIndex)
        {
            eCoins scoredCoin;
            int currentRow = m_TopCoinPerColumn[i_ColumnIndex].InsertIndex;
            int leftColumn = i_ColumnIndex - 1;
            int rightColumn = i_ColumnIndex + 1;
            int leftScore = 1;
            int rightScore = 1;

            if (!rowIsOutOfBoard(currentRow - 1) && IsValidColumn(leftColumn))
            {
                scoredCoin = m_BoardMatrix[currentRow - 1, leftColumn].Coin;

                leftScore = scoreTopLeftDiagonal(scoredCoin, currentRow, leftColumn);
            }

            if (!rowIsOutOfBoard(currentRow + 1) && IsValidColumn(rightColumn))
            {
                scoredCoin = m_BoardMatrix[currentRow + 1, rightColumn].Coin;

                rightScore = scoreBottomRightDiagonal(scoredCoin, currentRow, rightColumn);
            }

            return Math.Max(rightScore, leftScore);
        }

        private int getContinuousBottomLeftTopRightDiagonalScore(int i_ColumnIndex)
        {
            eCoins scoredCoin;
            int currentRow = m_TopCoinPerColumn[i_ColumnIndex].InsertIndex;
            int leftColumn = i_ColumnIndex - 1;
            int rightColumn = i_ColumnIndex + 1;
            int score = 1;

            if (!rowIsOutOfBoard(currentRow + 1) && IsValidColumn(leftColumn))
            {
                scoredCoin = m_BoardMatrix[currentRow + 1, leftColumn].Coin;

                score = scoreBottomLeftDiagonal(scoredCoin, currentRow, leftColumn);

                currentRow = m_TopCoinPerColumn[i_ColumnIndex].InsertIndex - 1;

                score += scoreTopRightDiagonal(scoredCoin, currentRow, rightColumn);
            }
            else if (!rowIsOutOfBoard(currentRow) && IsValidColumn(rightColumn))
            {
                scoredCoin = m_BoardMatrix[m_TopCoinPerColumn[i_ColumnIndex].InsertIndex, i_ColumnIndex].Coin;

                score += scoreBottomLeftDiagonal(scoredCoin, currentRow, leftColumn);

                currentRow = m_TopCoinPerColumn[i_ColumnIndex].InsertIndex - 1;

                score += scoreTopRightDiagonal(scoredCoin, currentRow, rightColumn);
            }

            return score;
        }

        private int getContinuousTopLeftBottomRightDiagonalScore(int i_ColumnIndex)
        {
            eCoins scoredCoin;
            int currentRow = m_TopCoinPerColumn[i_ColumnIndex].InsertIndex;
            int leftColumn = i_ColumnIndex - 1;
            int rightColumn = i_ColumnIndex + 1;
            int score = 1;

            if (!rowIsOutOfBoard(currentRow) && IsValidColumn(leftColumn))
            {
                scoredCoin = m_BoardMatrix[currentRow - 1, leftColumn].Coin;

                score = scoreTopLeftDiagonal(scoredCoin, currentRow, leftColumn);

                currentRow = m_TopCoinPerColumn[i_ColumnIndex].InsertIndex + 1;

                score += scoreBottomRightDiagonal(scoredCoin, currentRow, rightColumn);
            }
            else if (!rowIsOutOfBoard(currentRow) && IsValidColumn(rightColumn))
            {
                scoredCoin = m_BoardMatrix[m_TopCoinPerColumn[i_ColumnIndex].InsertIndex, i_ColumnIndex].Coin;

                score += scoreTopLeftDiagonal(scoredCoin, currentRow, leftColumn);

                currentRow = m_TopCoinPerColumn[i_ColumnIndex].InsertIndex + 1;

                score += scoreBottomRightDiagonal(scoredCoin, currentRow, rightColumn);
            }

            return score;
        }

        private int scoreBottomRightDiagonal(eCoins i_ScoredCoin, int i_CurrentRow, int i_RightColumn)
        {
            int score = 1;

            while (!rowIsOutOfBoard(i_CurrentRow + 1) && IsValidColumn(i_RightColumn) &&
                isSameCoin(m_BoardMatrix[i_CurrentRow + 1, i_RightColumn].Coin, i_ScoredCoin))
            {
                score++;
                i_CurrentRow++;
                i_RightColumn++;
            }

            return score;
        }

        private int scoreTopLeftDiagonal(eCoins i_ScoredCoin, int i_CurrentRow, int i_LeftColumn)
        {
            int score = 1;

            while (!rowIsOutOfBoard(i_CurrentRow + 1) && IsValidColumn(i_LeftColumn) &&
                isSameCoin(m_BoardMatrix[i_CurrentRow + 1, i_LeftColumn].Coin, i_ScoredCoin))
            {
                score++;
                i_CurrentRow--;
                i_LeftColumn--;
            }

            return score;
        }

        private int scoreTopRightDiagonal(eCoins i_ScoredCoin, int i_CurrentRow, int i_RightColumn)
        {
            int score = 1;

            while (!rowIsOutOfBoard(i_CurrentRow - 1) && IsValidColumn(i_RightColumn) &&
                isSameCoin(m_BoardMatrix[i_CurrentRow - 1, i_RightColumn].Coin, i_ScoredCoin))
            {
                score++;
                i_CurrentRow--;
                i_RightColumn++;
            }

            return score;
        }

        private int scoreBottomLeftDiagonal(eCoins i_ScoredCoin, int i_CurrentRow, int i_LeftColumn)
        {
            int score = 1;

            while (!rowIsOutOfBoard(i_CurrentRow + 1) && IsValidColumn(i_LeftColumn) &&
                isSameCoin(m_BoardMatrix[i_CurrentRow + 1, i_LeftColumn].Coin, i_ScoredCoin))
            {
                score++;
                i_CurrentRow++;
                i_LeftColumn--;
            }

            return score;
        }

        private int getColumnScore(int i_ColumnIndex)
        {
            int rowBelow = m_TopCoinPerColumn[i_ColumnIndex].InsertIndex + 1;
            int score = 1;

            if (!rowIsOutOfBoard(rowBelow) && IsValidColumn(i_ColumnIndex))
            {
                eCoins coinCombo = m_BoardMatrix[rowBelow, i_ColumnIndex].Coin;
                score = countAmoutOfCoinsBelow(rowBelow, i_ColumnIndex, coinCombo) + 1;
            }

            if (!isWinnableColumnStrategy(score, i_ColumnIndex))
            {
                score = 1;
            }

            return score;
        }

        private int getRowScore(int i_ColumnIndex)
        {
            int currentRow = m_TopCoinPerColumn[i_ColumnIndex].InsertIndex;
            int score;

            if (IsValidColumn(i_ColumnIndex + 1) &&
                IsValidColumn(i_ColumnIndex - 1) &&
                !rowIsOutOfBoard(currentRow) &&
                isSameCoin(m_BoardMatrix[currentRow, i_ColumnIndex + 1].Coin, m_BoardMatrix[currentRow, i_ColumnIndex - 1].Coin))
            {
                score = getContinuousRowScore(i_ColumnIndex);
            }
            else
            {
                score = getGreaterRowSideScore(i_ColumnIndex);
            }

            return score;
        }

        private int countAmoutOfCoinsBelow(int i_CurrentRow, int i_Column, eCoins i_CoinToCount)
        {
            int count = 0;

            while (!rowIsOutOfBoard(i_CurrentRow) &&
                IsValidColumn(i_Column) &&
                isSameCoin(m_BoardMatrix[i_CurrentRow, i_Column].Coin, i_CoinToCount))
            {
                count++;
                i_CurrentRow++;
            }

            return count;
        }

        private bool isSameCoin(eCoins i_FirstCoin, eCoins i_SecondCoin)
        {
            bool isValidCoinsEqual = i_FirstCoin != eCoins.Empty && i_FirstCoin == i_SecondCoin;

            return isValidCoinsEqual;
        }

        private bool rowIsOutOfBoard(int i_Row)
        {
            bool isRowValid = false;

            if (i_Row < 0 || i_Row >= m_BoardMatrix.GetLength(0))
            {
                isRowValid = true;
            }

            return isRowValid;
        }

        private bool isWinnableColumnStrategy(int i_Score, int i_Column)
        {
            bool isWinnableStrategy = false;

            if (i_Score < 4 && m_TopCoinPerColumn[i_Column].InsertIndex >= (4 - i_Score))
            {
                isWinnableStrategy = true;
            }

            return isWinnableStrategy;
        }

        private int getContinuousRowScore(int i_ColumnIndex)
        {
            eCoins scoredCoin = eCoins.Empty;
            int currentRow = m_TopCoinPerColumn[i_ColumnIndex].InsertIndex;
            int score = 1;
            int columnIndexDelta = 1;

            if (IsValidColumn(i_ColumnIndex + columnIndexDelta))
            {
                scoredCoin = m_BoardMatrix[currentRow, i_ColumnIndex + columnIndexDelta].Coin;
            }
            else if (IsValidColumn(i_ColumnIndex - columnIndexDelta))
            {
                scoredCoin = m_BoardMatrix[currentRow, i_ColumnIndex - columnIndexDelta].Coin;
            }
            
            while (!rowSideIsBlocked(i_ColumnIndex, scoredCoin, currentRow, -columnIndexDelta) &&
                !rowSideIsBlocked(i_ColumnIndex, scoredCoin, currentRow, columnIndexDelta) &&
                isSameCoin(m_BoardMatrix[currentRow, i_ColumnIndex - columnIndexDelta].Coin, m_BoardMatrix[currentRow, i_ColumnIndex + columnIndexDelta].Coin))
            {
                score += 2;
                columnIndexDelta++;
            }

            while (!rowSideIsBlocked(i_ColumnIndex, scoredCoin, currentRow, -columnIndexDelta) ||
                rowSideHasEmptySquare(i_ColumnIndex, currentRow, i_ColumnIndex - columnIndexDelta))
            {
                score++;
                columnIndexDelta++;
            }

            while (!rowSideIsBlocked(i_ColumnIndex, scoredCoin, currentRow, columnIndexDelta) ||
                rowSideHasEmptySquare(i_ColumnIndex, currentRow, i_ColumnIndex + columnIndexDelta))
            {
                score++;
                columnIndexDelta++;
            }

            if (score < 4 &&
                rowSideIsBlocked(i_ColumnIndex, scoredCoin, currentRow, -columnIndexDelta) &&
                rowSideIsBlocked(i_ColumnIndex, scoredCoin, currentRow, columnIndexDelta))
            {
                score = 1;
            }

            return score;
        }

        private int getGreaterRowSideScore(int i_ColumnIndex)
        {
            eCoins scoredCoin = eCoins.Empty;
            int currentRow = m_TopCoinPerColumn[i_ColumnIndex].InsertIndex;
            int scoreLeft = 1;
            int scoreRight = 1;
            int finalScore;
            int columnIndexDelta = 1;

            if (IsValidColumn(i_ColumnIndex + columnIndexDelta) && !rowIsOutOfBoard(currentRow))
            {
                scoredCoin = m_BoardMatrix[currentRow, i_ColumnIndex + columnIndexDelta].Coin;
            }
            else if (IsValidColumn(i_ColumnIndex - columnIndexDelta) && !rowIsOutOfBoard(currentRow))
            {
                scoredCoin = m_BoardMatrix[currentRow, i_ColumnIndex - columnIndexDelta].Coin;
            }

            while (IsValidColumn(i_ColumnIndex - columnIndexDelta) &&
                !rowSideHasEmptySquare(i_ColumnIndex, currentRow, -columnIndexDelta) &&
                isSameCoin(scoredCoin, m_BoardMatrix[currentRow, i_ColumnIndex - columnIndexDelta].Coin))
            {
                scoreLeft++;
                columnIndexDelta++;
            }

            while (IsValidColumn(i_ColumnIndex + columnIndexDelta) &&
                !rowIsOutOfBoard(currentRow) &&
                !rowSideHasEmptySquare(i_ColumnIndex, currentRow, columnIndexDelta) &&
                isSameCoin(scoredCoin, m_BoardMatrix[currentRow, i_ColumnIndex + columnIndexDelta].Coin))
            {
                scoreRight++;
                columnIndexDelta++;
            }

            if (scoreRight > scoreLeft)
            {
                if (scoreRight < 4 &&
                    rowSideIsBlocked(i_ColumnIndex, scoredCoin, currentRow, columnIndexDelta) &&
                    rowSideIsBlocked(i_ColumnIndex, scoredCoin, currentRow, -1))
                {
                    finalScore = 1;
                }
                else
                {
                    finalScore = scoreRight;
                }
            }
            else
            {
                if (scoreLeft < 4 &&
                    rowSideIsBlocked(i_ColumnIndex, scoredCoin, currentRow, -columnIndexDelta) &&
                    rowSideIsBlocked(i_ColumnIndex, scoredCoin, currentRow, 1))
                {
                    finalScore = 1;
                }
                else
                {
                    finalScore = scoreLeft;
                }
            }

            return finalScore;
        }

        private bool rowSideIsBlocked(int i_Column, eCoins i_NonBlockingCoin, int i_CurrentRow, int i_ColumnIndexDelta)
        {
            return !rowIsOutOfBoard(i_CurrentRow) &&
                (!IsValidColumn(i_Column + i_ColumnIndexDelta) ||
                m_BoardMatrix[i_CurrentRow, i_Column + i_ColumnIndexDelta].Coin != i_NonBlockingCoin &&
                m_BoardMatrix[i_CurrentRow, i_Column + i_ColumnIndexDelta].Coin != eCoins.Empty);
        }

        private bool rowSideHasEmptySquare(int i_Column, int i_CurrentRow, int i_ColumnIndexDelta)
        {
            bool hasEmptySquare = false;

            if (!rowIsOutOfBoard(i_CurrentRow) &&
                IsValidColumn(i_Column + i_ColumnIndexDelta) &&
                m_BoardMatrix[i_CurrentRow, i_Column + i_ColumnIndexDelta].Coin == eCoins.Empty)
            {
                hasEmptySquare = true;
            }

            return hasEmptySquare;
        }

        private static bool tryExtractLengthFromBoardSizeString(string i_BoardSize,
            ref byte io_Iterator,
            ref byte io_ExtractedLength)
        {
            bool hasSucceeded = true;

            while (io_Iterator < i_BoardSize.Length && !i_BoardSize[io_Iterator].Equals('X') && !i_BoardSize[io_Iterator].Equals('x'))
            {
                if (char.IsDigit(i_BoardSize[io_Iterator]))
                {
                    byte currentDigit;

                    if (byte.TryParse(i_BoardSize[io_Iterator].ToString(), out currentDigit))
                    {
                        io_ExtractedLength *= (byte)(io_Iterator * 10);
                        io_ExtractedLength += currentDigit;
                    }
                    else
                    {
                        hasSucceeded = false;
                        break;
                    }
                }

                io_Iterator++;
            }

            io_Iterator++;

            return hasSucceeded;
        }

        private static bool validBoardSize(byte i_RowLength, byte i_ColumnLength,
            byte v_MinRowLength, byte v_MaxRowLength,
            byte v_MinColLength, byte v_MaxColLength)
        {
            return i_RowLength >= v_MinRowLength &&
                i_RowLength <= v_MaxRowLength &&
                i_ColumnLength >= v_MinColLength &&
                i_ColumnLength <= v_MaxColLength;
        }
    }
}
