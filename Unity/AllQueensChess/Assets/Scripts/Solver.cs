using System.Collections;
using System.Collections.Generic;
using System;

public class Solver {

    private GameManager currentState;
    private IDictionary<ulong, byte> Memory = new Dictionary<ulong, byte>();

    public Solver()
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

    /*private byte Solve(GameManager state)
    {
        bool winFlag = false;
        uint serialized = state.serialize();
        if (Memory.ContainsKey(serialized))
        {
            return Memory[serialized];
        }
        byte primitive = state.primitive();
        if (primitive != 0)
        {

            return primitive;
        }

    }
    */
}
