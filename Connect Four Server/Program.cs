using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect_Four_Server
{
    internal class Program
    {
        private static Game game;

        static void Main(string[] args)
        {
            game = new Game();

            game.OnNextRound += Game_OnNextRound;
            game.OnPlayerWon += Game_OnGameEnded;
            game.OnGameDraw += Game_OnGameDraw;

            Game_OnNextRound(null, null);
        }

        private static void Game_OnNextRound(object sender, EventArgs e)
        {
            Console.Clear();
            Console.WriteLine("Aktueller Spieler {0}\n", game.CurrentPlayer);
            Console.WriteLine(game.PrintGrid());
            Console.WriteLine("Wo möchten sie setzten?");

            char playerAction = (char)Console.ReadKey().KeyChar;
            if (!char.IsNumber(playerAction))
            {
                Console.WriteLine("Ungültige Eingabe");
            }

            try
            {
                game.PlayerAction(game.CurrentPlayer, Convert.ToInt32(playerAction.ToString()) - 1);
            }
            catch (InvalidPlayerAction)
            {
                Game_OnNextRound(null, null);
            }
        }

        private static void Game_OnGameEnded(object sender, EventArgs e)
        {
            Console.Clear();
            Console.WriteLine(game.PrintGrid());
            Console.WriteLine("Spieler {0} hat gewonnen", game.CurrentPlayer);
            Console.WriteLine("Drücken Sie ENTER um das Spiel neu zu starten");
            Console.ReadLine();
            Main(null);
        }
        private static void Game_OnGameDraw(object sender, EventArgs e)
        {
            Console.Clear();
            Console.WriteLine(game.PrintGrid());
            Console.WriteLine("Das Spiel ist unentschieden");
            Console.WriteLine("Drücken Sie ENTER um das Spiel neu zu starten");
            Console.ReadLine();
            Main(null);
        }
    }
}
