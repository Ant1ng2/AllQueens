using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    public class GameManager
    {
        private static Game game;
        private static Solver solver;

        public static void Initialize(Game state, Solver solve)
        {
            game = state;
            solver = solve;
            if (solver != null)
            {
                solver.Solve(game);
            }
        }

        public static void Main(string[] args)
        {
            Initialize(new Game(), null);

            while (game.primitive() == 0)
            {
                Write();
                if (game.currentTurn == "W" || solver == null)
                {
                    Prompt();
                }
                else
                {
                }

                Console.Write("-------------------------------\n");
            }

            Write();
            Console.Write("Game Over");
        }

        private static void Write()
        {
            if (solver != null)
            {
                Console.Write("Solver: " + solver.Solve(game).ToString() + "\n");
            }
            Console.Write("Primitive: " + game.primitive().ToString() + "\n");
            Console.Write(game.currentTurn + "'s turn" + "\n");
            Console.Write(game.ToString() + "\n");
        }

        private static void Prompt()
        {
            Console.Write("Enter Piece: ");
            string first = Console.ReadLine();
            Console.Write("Enter End: ");
            string second = Console.ReadLine();

            Vector2Int begin = new Vector2Int(first[0] - '0', first[2] - '0');
            Vector2Int finish = new Vector2Int(second[0] - '0', second[2] - '0');

            List<Vector2Int> move = new List<Vector2Int>() { begin, finish };

            bool exists = false;

            foreach (List<Vector2Int> possible in game.generateMoves())
            {
                if (move[0].Equals(possible[0]) && move[1].Equals(possible[1]))
                {
                    exists = true;
                }
            }

            if (exists)
            {
                game = game.Move(move);
            }
            else
            {
                Console.Write("Not a valid move, try again \n");
            }

        }
    }
}
