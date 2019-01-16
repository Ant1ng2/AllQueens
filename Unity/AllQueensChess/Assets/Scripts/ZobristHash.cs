using UnityEngine;
using System;

public static class ZobristHash {

    private static uint[,] table = new uint[2,25];

    static ZobristHash()
    {
        System.Random rand = new System.Random();

        for (int j = 0; j < 25; j++)
        {
            for (int i = 0; i < 2; i++)
            {
                uint thirtyBits = (uint) rand.Next(1 << 30);
                uint twoBits = (uint) rand.Next(1 << 2);
                table[i, j] = (thirtyBits << 2) | twoBits;
            }
        }
    }

    public static uint Hash(GameObject[,] board, Player turn)
    {
        uint hash = 0;

        for (int i = 0; i < 25; i++)
        {
            GameObject j = board[i % 5, i / 5];
            if (j != null)
            {
                if (turn.pieces.Contains(j))
                {
                    hash = hash ^ table[0, i];
                }
                else
                {
                    hash = hash ^ table[1, i];
                }
            }
        }
        return hash;
    }
}
