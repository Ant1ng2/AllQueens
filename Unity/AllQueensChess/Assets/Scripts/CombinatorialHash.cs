using System.Collections.Generic;
using UnityEngine;

public static class CombinatorialHash
{
    //Use to store references of the pieces
    private static uint Choose(int n, int k)
    {
        if (k > n) return 0;
        if (k * 2 > n) k = n - k;
        if (k == 0) return 1;

        uint result = (uint) n;
        for (uint i = 2; i <= k; ++i)
        {
            result *= (uint) (n - i + 1);
            result /= i;
        }
        return result;
    }

    public static ulong Hash(GameObject[,] board, Player turn)
    {
        uint hashPieces = 0;

        int l = 12;

        for (int i = 0; i < 25; i++)
        {
            GameObject j = board[i % 5, i / 5];
            if (j != null && l > 0)
            {
                hashPieces += Choose(24 - i, l);
                l--;
            }
        }
        return (ulong) hashPieces << 32;
    }

    public static GameObject[,] Unhash(ulong hash, List<GameObject> current, List<GameObject> other)
    {
        hash = hash >> 32;
        GameObject[,] board = new GameObject[5, 5];

        int l = 12;

        for (int i = 0; i < 25; i++)
        {
            uint value = Choose(24 - i, l);
            if (hash >= value)
            {
                hash -= value;
                l--;
                board[i % 5, i / 5] = current[0];
            }
        }
        return board;
    }
}
