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
        uint hashColors = 0;

        int l = 12;
        int k = 6;

        for (int i = 0; i < 25; i++)
        {
            GameObject j = board[i % 5, i / 5];
            if (j != null && l > 0)
            {
                hashPieces += Choose(24 - i, l);
                l--;
                if (turn.pieces.Contains(j))
                {
                    hashColors += Choose(l, k);
                    k--;
                }
            }
        }
        return ((ulong) hashPieces << 32) | ((ulong) hashColors);
    }

    public static GameObject[,] Unhash(ulong hash, List<GameObject> current, List<GameObject> other)
    {
        uint hashPieces = (uint) (hash >> 32);
        uint hashColors = (uint)(hash & uint.MaxValue);

        GameObject[,] board = new GameObject[5, 5];

        int l = 12;
        int k = 6;
        int f = 6;

        for (int i = 0; i < 25; i++)
        {
            uint value = Choose(24 - i, l);
            if (hashPieces >= value)
            {
                hashPieces -= value;
                l--;
                value = Choose(l, k);
                if (hashColors >= value)
                {
                    hashColors -= value;
                    k--;
                    board[i % 5, i / 5] = current[k];
                }
                else
                {
                    f--;
                    board[i % 5, i / 5] = other[f];
                }
            }
        }
        return board;
    }
}
