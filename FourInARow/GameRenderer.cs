using System;

namespace FourInARow.UI
{
    class GameRenderer
    {
        public static void DrawSingleSquare(string i_CoinInSquare)
        {
            string currentGameSquare;

            if (i_CoinInSquare.Equals(""))
            {
                currentGameSquare = string.Format("|   ");
            }
            else
            {
                currentGameSquare = string.Format("| {0} ", i_CoinInSquare);
            }

            Console.Write(currentGameSquare);
        }

        public static void DrawLineSeperator(int i_ColumnLength)
        {
            for (int j = 0; j < i_ColumnLength; j++)
            {
                Console.Write("====");
            }

            Console.WriteLine("=");
        }

        public static void ShowColumnIndexes(int i_ColumnLength)
        {
            for (int i = 0; i < i_ColumnLength; i++)
            {
                int currentColumn = i + 1;
                Console.Write(String.Format("  {0} ", currentColumn.ToString()));
            }

            Console.WriteLine();
        }

        public static void DrawBoardRightEnd()
        {
            Console.WriteLine("|");
        }
    }
}
