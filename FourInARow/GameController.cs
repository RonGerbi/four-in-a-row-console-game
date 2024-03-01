using FourInARow.Core;
using FourInARow.UI;
using Ex02.ConsoleUtils;

namespace FourInARow.Controller
{
    class GameController
    {
        private GameManager m_GameManager;

        public GameController()
        {
            m_GameManager = setupGame();
        }

        public void StartGame()
        {
            Player? winner;
            bool hasQuitted = false;

            while (!m_GameManager.GameIsOver(out winner))
            {
                int chosenColumn;

                drawBoard();
                chosenColumn = playTurn();

                if (chosenColumn == -1)
                {
                    hasQuitted = true;
                    break;
                }
                else if (!m_GameManager.GameIsOver(out winner))
                {
                    m_GameManager.SwitchTurn();
                }
            }

            drawBoard();

            if (hasQuitted || winner != null)
            {
                if (winner == null)
                {
                    winner = getRemainingPlayer();
                }

                m_GameManager.AddAPointToPlayer(winner);
                UserMessages.DeclareWinner(winner.ToString());
            }
            else
            {
                UserMessages.DeclareDraw();
            }

            UserMessages.ShowPoints(m_GameManager.XPlayer.ToString(), m_GameManager.OPlayer.ToString(),
                m_GameManager.XPlayer.Points.ToString(), m_GameManager.OPlayer.Points.ToString());

            if (wantAnotherRound())
            {
                m_GameManager.Reset();
                StartGame();
            }
        }

        private static GameManager setupGame()
        {
            byte boardColumnLength;
            byte boardRowLength;

            getValidBoardSize(out boardRowLength, out boardColumnLength);

            GameModes? selectedGameMode = getValidGameMode();
            GameManager gameManager = new GameManager(boardRowLength, boardColumnLength, selectedGameMode);

            return gameManager;
        }

        private void drawBoard()
        {
            int rowLength = m_GameManager.GameBoard.BoardMatrix.GetLength(0);
            int columnLength = m_GameManager.GameBoard.BoardMatrix.GetLength(1);

            Screen.Clear();
            GameRenderer.ShowColumnIndexes(columnLength);

            for (int i = 0; i < rowLength; i++)
            {
                for (int j = 0; j < columnLength; j++)
                {
                    GameRenderer.DrawSingleSquare(m_GameManager.GameBoard.BoardMatrix[i, j].ToString());
                }

                GameRenderer.DrawBoardRightEnd();
                GameRenderer.DrawLineSeperator(columnLength);
            }
        }

        private int playTurn()
        {
            int chosenColumn;
            bool hasDroppedCoin;

            do
            {
                if (m_GameManager.GameMode.HasValue && m_GameManager.GameMode.Value.GameMode == eGameModes.OneVsOne)
                {
                    chosenColumn = getUserColumnChoice(m_GameManager.GameBoard,
                        m_GameManager.GameBoard.BoardMatrix.GetLength(1));
                    hasDroppedCoin = m_GameManager.TryToDropCoin(chosenColumn);
                }
                else
                {
                    if (m_GameManager.CurrentPlayer.PlayerCoin.Coin == m_GameManager.OPlayer.PlayerCoin.Coin)
                    {
                        chosenColumn = m_GameManager.GetAIColumnChoice();
                        hasDroppedCoin = m_GameManager.TryToDropCoin(chosenColumn);
                    }
                    else
                    {
                        chosenColumn = getUserColumnChoice(m_GameManager.GameBoard,
                        m_GameManager.GameBoard.BoardMatrix.GetLength(1));
                        hasDroppedCoin = m_GameManager.TryToDropCoin(chosenColumn);
                    }
                }
                if (chosenColumn == -1)
                {
                    break;
                }
            } while (!hasDroppedCoin);

            return chosenColumn;
        }

        private bool wantAnotherRound()
        {
            const char yes = 'y';
            const char no = 'n';
            bool isValidInput = true;
            bool startAnotherRound = false;
            string choice;

            do
            {
                if (isValidInput)
                {
                    UserMessages.AskToStartAnotherRound();
                }
                else
                {
                    Screen.Clear();
                    drawBoard();
                    UserMessages.ShowInvalidYesNoInputMsg();
                }

                choice = UserInteractions.GetUserStringInput();
                choice = choice.ToLower();

                if (choice.Length == 1)
                {
                    startAnotherRound = choice[0].Equals(yes);
                    isValidInput = startAnotherRound || choice[0].Equals(no);
                }
                else
                {
                    isValidInput = false;
                }
            } while (!isValidInput);

            return startAnotherRound;
        }

        private static GameModes? getValidGameMode()
        {
            bool isValidMode = true;
            GameModes? selectedGameMode;

            do
            {
                if (isValidMode)
                {
                    UserMessages.ShowModeSelectionMenu();
                }
                else
                {
                    UserMessages.ShowInvalidModeMsg();
                }

                string choiceStr = UserInteractions.GetUserStringInput();
                isValidMode = GameModes.TryParse(choiceStr, out selectedGameMode);
            } while (!isValidMode);

            return selectedGameMode;
        }

        private static void getValidBoardSize(out byte i_BoardRowLength, out byte i_BoardColumnLength)
        {
            bool isValidBoardSize = true;

            do
            {
                if (!isValidBoardSize)
                {
                    UserMessages.ShowInvalidBoardSizeMsg(Board.MinRowLength, Board.MinColumnLength,
                        Board.MaxRowLength, Board.MaxColumnLength);
                }
                else
                {
                    UserMessages.AskForBoardSize(Board.MinRowLength, Board.MinColumnLength,
                        Board.MaxRowLength, Board.MaxColumnLength);
                }

                string boardSizeStr = UserInteractions.GetUserStringInput();
                isValidBoardSize = Board.TryExtractingLengthsOfRowsAndCols(boardSizeStr, out i_BoardRowLength, out i_BoardColumnLength);
            } while (!isValidBoardSize);
        }

        private static int getUserColumnChoice(Board i_GameBoard, int i_LastColumnNumber)
        {
            const int k_FirstColumnNumber = 1;
            int userColumnChoice;
            int userChoice;
            bool isColumnFull = true;
            bool isValidColumn;
            bool isValidInput = true;

            do
            {
                if (isValidInput)
                {
                    UserMessages.AskUserForColumn(k_FirstColumnNumber, i_LastColumnNumber);
                }
                else
                {
                    UserMessages.ShowInvalidColumnInputMsg();
                }

                string userColumnChoiceStr = UserInteractions.GetUserStringInput();

                if (hasQuitted(userColumnChoiceStr))
                {
                    userColumnChoice = -1;
                    break;
                }

                isValidInput = int.TryParse(userColumnChoiceStr, out userColumnChoice);
                isValidColumn = i_GameBoard.IsValidColumn(userColumnChoice - 1);

                if (isValidColumn)
                {
                    isColumnFull = i_GameBoard.IsColumnFull(userColumnChoice - 1);
                }

                isValidInput = isValidInput && !isColumnFull;
            } while (!isValidInput);

            userChoice = userColumnChoice == -1 ? userColumnChoice : userColumnChoice - 1;

            return userChoice;
        }

        private Player? getRemainingPlayer()
        {
            Player? winner;

            if (m_GameManager.CurrentPlayer.PlayerCoin.Coin == m_GameManager.XPlayer.PlayerCoin.Coin)
            {
                winner = m_GameManager.OPlayer;
            }
            else
            {
                winner = m_GameManager.XPlayer;
            }

            return winner;
        }

        private static bool hasQuitted(string i_UserChoiceStr)
        {
            return i_UserChoiceStr.ToUpper().Equals("Q");
        }
    }
}
