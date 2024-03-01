using System;

namespace FourInARow.UI
{
    public static class UserMessages
    {
        public static void ShowPoints(string i_FirstPlayer, string i_SecondPlayer,
            string i_FirstPlayerPoints, string i_SecondPlayerPoints)
        {
            string pointsStr = String.Format("{0} points: {1} | {2} points: {3}",
                i_FirstPlayer,
                i_FirstPlayerPoints,
                i_SecondPlayer,
                i_SecondPlayerPoints);

            Console.WriteLine(pointsStr);
        }

        public static void DeclareWinner(string i_Winner)
        {
            string winnerDeclaration = String.Format("{0} has won!", i_Winner);

            Console.WriteLine(winnerDeclaration);
        }

        public static void DeclareDraw()
        {
            string drawMsg = String.Format("It's a tie!");

            Console.WriteLine(drawMsg);
        }

        public static void ShowInvalidColumnInputMsg()
        {
            String msg = String.Format("Please make sure to choose an available column(a whole number in the mentioned range):");

            Console.WriteLine(msg);
        }

        public static void AskUserForColumn(int i_FirstColumnNumber, int i_LastColumnNumber)
        {
            String msg = String.Format(@"Please choose in which column to insert your coin (between {0} - {1}):",
                i_FirstColumnNumber.ToString(),
                i_LastColumnNumber.ToString());

            Console.WriteLine(msg);
        }

        public static void ShowModeSelectionMenu()
        {
            String msg = String.Format(@"Select game mode:
1. Against AI
2. Against a friend");

            Console.WriteLine(msg);
        }

        public static void ShowInvalidModeMsg()
        {
            String msg = String.Format(@"Please select a valid option:
1. Against a friend
2. Against AI");

            Console.WriteLine(msg);
        }

        public static void ShowInvalidYesNoInputMsg()
        {
            string invalidInputMsg = String.Format("Please enter a valid answer('y' or 'n')");

            Console.WriteLine(invalidInputMsg);
        }

        public static void AskToStartAnotherRound()
        {
            string newRoundQuestion = String.Format("Do you want to start a new round?(enter 'y' for yes and 'n' for no)");

            Console.WriteLine(newRoundQuestion);
        }

        public static void ShowInvalidBoardSizeMsg(int i_MinRowLength, int i_MinColumnLength,
            int i_MaxRowLength, int i_MaxColumnLength)
        {
            string msg = String.Format(@"Invalid board size,
Please make sure your input is a valid board size in the range - {0}X{1} to {2}X{3}:",
i_MinRowLength, i_MinColumnLength, i_MaxRowLength, i_MaxColumnLength);

            Console.WriteLine(msg);
        }

        public static void AskForBoardSize(int i_MinRowLength, int i_MinColumnLength,
            int i_MaxRowLength, int i_MaxColumnLength)
        {
            string msg = String.Format("Please insert the desired game board size (from {0}X{1} to {2}X{3}):",
                i_MinRowLength, i_MinColumnLength, i_MaxRowLength, i_MaxColumnLength);

            Console.WriteLine(msg);
        }
    }
}
