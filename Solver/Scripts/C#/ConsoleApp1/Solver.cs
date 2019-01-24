using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    public class Solver
    {
        public byte GetValueFromState(Game state)
        {
            ulong serialized = state.Serialize();
            return Program.Get(serialized);
        }

        public byte Solve(Game state)
        {
            Stack<ulong> stack = new Stack<ulong>();
            long count = 0;
            ulong firstStateSerialized;
            firstStateSerialized = state.Serialize();

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
                state = Game.Deserialize(serialized);

                Console.WriteLine(count.ToString() + "\n");
                Console.WriteLine(state.ToString() + "\n");

                byte primitive = state.primitive();
                if (primitive != 0)
                {
                    Program.Add(serialized, primitive);
                    stack.Pop();
                }
                else
                {
                    bool solvable = true;
                    bool winFlag = true;
                    var moveList = state.generateMoves();

                    foreach (var move in moveList)
                    {
                        serialized = state.Move(move).Serialize();
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
