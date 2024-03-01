using FourInARow.Controller;

namespace FourInARow
{
    class Program
    {
        public static void Main()
        {
            startGame();
        }

        private static void startGame()
        {
            GameController gameController = new GameController();
            gameController.StartGame();
        }
    }
}
