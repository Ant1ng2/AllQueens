using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    public abstract class Game
    {
        public abstract string currentTurn { get; }
        public abstract string otherTurn { get; }

        public abstract byte Primitive();

        public abstract Game Move(List<Vector2Int> list);

        public abstract List<List<Vector2Int>> GenerateMoves();

        public abstract override string ToString();

        public abstract ulong Serialize();

        public abstract Game Deserialize(ulong hash);

        public abstract (bool, List<Vector2Int>) Prompt();
    }
}
