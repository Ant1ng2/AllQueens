using System.Collections;
using System.Collections.Generic;
using System;

public class OtherSolver {
    /*
    private GameManager currentState;
    private IDictionary<ulong, byte> Memory = new Dictionary<ulong, byte>();
    private Stack<ulong> stack = new Stack<ulong>();

    public OtherSolver()
    {
        currentState = new GameManager();
    }

    public byte GetValueFromState(GameManager state)
    {
        ulong serialized = state.Serialize();
        if (!Memory.ContainsKey(serialized))
        {
            return 0;
        }
        return Memory[serialized];
    }

    private byte Solve(GameManager state)
    {
        long count = 0;
        ulong firstStateSerialized = state.Serialize();

        if (Memory.ContainsKey(firstStateSerialized))
        {
            return Memory[firstStateSerialized];
        }

        stack.Clear();
        stack.Push(firstStateSerialized);

        while (stack.Count > 0 && count < 4805077200)
        {
            count += 1;
            ulong serialized = stack.Peek();
            state = GameManager.Deserialize(serialized);

            byte primitive = state.primitive();
            if (primitive != 0)
            {
                Memory[serialized] = primitive;
                stack.Pop();
            }
            else
            {
                bool solvable = true;
                bool winFlag = true;
                var moveList = state.generateMoves();
                
                foreach (var move in moveList)
                {
                    serialized = state.doMove(move).Serialize();
                    if (!Memory.ContainsKey(serialized))
                    {
                        solvable = false;
                        if (!stack.Contains(serialized))
                        {
                            stack.Push(serialized);
                        }
                    }
                    else
                    {
                        byte value = Memory[serialized];
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
                        Memory[serialized] = 2;
                    }
                    else
                    {
                        Memory[serialized] = 1;
                    }
                }
            }
        }
        return Memory[firstStateSerialized];
    }
    */
}
