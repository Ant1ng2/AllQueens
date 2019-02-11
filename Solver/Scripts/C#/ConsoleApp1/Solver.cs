using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    public class Solver
    {
        //Local store of Game States (do not use for large games)
        private Dictionary<ulong, byte> memory= new Dictionary<ulong, byte>();
            
        private byte getValueData(Game state)
        {
            ulong serialized = state.Serialize();
            return Program.Get(serialized);
        }

        private byte getValueMem(Game state)
        {
            ulong serialized = state.Serialize();
            if (memory.ContainsKey(serialized))
            {
                return memory[serialized];
            }
            return 0;
        }
        /*
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
                state = state.Deserialize(serialized);

                Console.WriteLine(count.ToString() + "\n");
                Console.WriteLine(state.ToString() + "\n");

                byte primitive = state.Primitive();
                if (primitive != 0)
                {
                    Program.Add(serialized, primitive);
                    stack.Pop();
                }
                else
                {
                    bool solvable = true;
                    bool winFlag = true;
                    var moveList = state.GenerateMoves();

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
        */
        public byte Solve(Game state)
        {
            bool winFlag = false;
            bool tieFlag = false;

            ulong serialized = state.Serialize();

            if (memory.ContainsKey(serialized))
            {
                return memory[serialized];
            }
            byte primitive = state.Primitive();

            if (primitive != 0)
            {
                memory[serialized] = primitive;
                return primitive;
            }

            foreach (List<Vector2Int> move in state.GenerateMoves())
            {
                Game nextState = state.Move(move);
                if (Solve(nextState) == 1)
                {
                    memory[serialized] = 2;
                    winFlag = true;
                }
                if (Solve(nextState) == 3)
                {
                    tieFlag = true;
                }
            }
            if (!winFlag)
            {
                if (tieFlag)
                {
                    memory[serialized] = 3;
                    return 3;
                }
                memory[serialized] = 1;
                return 1;
            }
            return 2;
        }

        //Please run Solve on the root gameState before running this function. (Plans to implement in-game solving coming soon).
        public List<Vector2Int> getMove(Game state)
        {
            if (getValueMem(state) < 2)
            {
                return state.GenerateMoves()[0];
            }

            foreach (List<Vector2Int> move in state.GenerateMoves())
            {
                Game nextState = state.Move(move);

                if (getValueMem(nextState) == 1)
                {
                    return move;
                }
            }
            
            foreach(List<Vector2Int> move in state.GenerateMoves())
            {
                Game nextState = state.Move(move);

                if (getValueMem(nextState) == 3)
                {
                    return move;
                }
            }
            throw new Exception();
        }
    }
}
