using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    public class GameManager
    {
        private static Game game = new AllQueensChess();
        private static Solver solver;

        private static String playerTurn = game.currentTurn;

        public static void Initialize( Solver solve)
        {
            solver = solve;
            if (solver != null)
            {
                solver.Solve(game);
            }
        }

        public static void Main(string[] args)
        {
            Initialize(null);

            while (game.Primitive() == 0)
            {
                Write();
                if (game.currentTurn == playerTurn || solver == null)
                {
                    Prompt();
                }
                else
                {
                    List<Vector2Int> move = solver.getMove(game);
                    game = game.Move(move);
                }

                Console.Write("-------------------------------\n");
            }

            Write();
            Console.Write("Game Over (Press any key to continue)");
            Console.ReadKey();
        }

        private static void Write()
        {
            if (solver != null)
            {
                Console.Write("Solver: " + solver.Solve(game).ToString() + "\n");
            }
            Console.Write("Primitive: " + game.Primitive().ToString() + "\n");
            Console.Write(game.currentTurn + "'s turn" + "\n");
            Console.Write(game.ToString() + "\n");
        }

        private static void Prompt()
        {
            var response = game.Prompt();

            if (response.Item1)
            {
                game = game.Move(response.Item2);
            }
            else
            {
                Console.Write("Not a valid move, try again \n");
            }
        }
    }
}
