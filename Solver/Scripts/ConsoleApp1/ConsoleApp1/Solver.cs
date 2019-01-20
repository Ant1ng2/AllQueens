using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    public class Solver
    {
        private Game currentState;
        private Stack<ulong> stack = new Stack<ulong>();

        public static void Main(string[] args)
        {
            Program.Initialize();
            Solver state = new Solver();
            state.Solve();
        }

        public Solver()
        {
            currentState = new Game();
        }

        public byte GetValueFromState(Game state)
        {
            ulong serialized = state.Serialize();
            return Program.Get(serialized);
        }

        private byte Solve()
        {
            long count = 0;
            ulong firstStateSerialized = currentState.Serialize();

            if (Program.Contains(firstStateSerialized))
            {
                return Program.Get(firstStateSerialized);
            }

            stack.Clear();
            stack.Push(firstStateSerialized);

            while (stack.Count > 0 && count < 4805077200)
            {
                count += 1;
                ulong serialized = stack.Peek();
                currentState = Game.Deserialize(serialized);

                Console.WriteLine(count.ToString() + "\n");
                Console.WriteLine(currentState.ToString() + "\n");

                byte primitive = currentState.primitive();
                if (primitive != 0)
                {
                    Program.Add(serialized, primitive);
                    stack.Pop();
                }
                else
                {
                    bool solvable = true;
                    bool winFlag = true;
                    var moveList = currentState.generateMoves();

                    foreach (var move in moveList)
                    {
                        serialized = currentState.Move(move).Serialize();
                        if (!Program.Contains(serialized))
                        {
                            solvable = false;
                            if (!stack.Contains(serialized))
                            {
                                stack.Push(serialized);
                            }
                        }
                        else
                        {
                            byte value = Program.Get(serialized);
                            if (value == 1)
                            {
                                winFlag = false;
                            }
                        }
                    }
                    if (solvable)
                    {
                        if (winFlag)
                        {
                            Program.Add(serialized, 2);
                        }
                        else
                        {
                            Program.Add(serialized, 1);
                        }
                    }
                }
            }
            return Program.Get(firstStateSerialized);
        }
    }
}
