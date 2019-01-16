using System.Collections.Generic;
using UnityEngine;

public static class CombinatorialHash
{
    //Use to store references of the pieces
    private static GameObject[] currentPieces;
    private static GameObject[] otherPieces;

    private static int currentIndex = 0;
    private static int otherIndex = 0;

    public static void SetPieces(List<GameObject> current, List<GameObject> other)
    {
        currentPieces = current.ToArray();
        otherPieces = other.ToArray();
    }

    private static GameObject GenerateCurrentPiece()
    {
        if (currentPieces.Length == 0)
        {
            return null;
        }

        currentIndex++;

        if (currentIndex >= currentPieces.Length)
        {
            currentIndex = currentIndex % currentPieces.Length;
        }
        return currentPieces[currentIndex];
    }

    private static GameObject GenerateOtherPiece()
    {
        if (otherPieces.Length == 0)
        {
            return null;
        }

        otherIndex++;

        if (otherIndex >= otherPieces.Length)
        {
            otherIndex = otherIndex % otherPieces.Length;
        }
        return otherPieces[otherIndex];
    }

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
        uint hashColor = 0;

        int l = 12;

        for (int i = 0; i < 25; i++)
        {
            GameObject j = board[i % 5, i / 5];
            if (j != null && l > 0)
            {
                hashPieces += Choose(24 - i, l);
                hashColor *= 2;
                if (turn.pieces.Contains(j))
                {
                    hashColor += 1;
                }
                l--;
            }
        }
        return ((ulong) hashPieces) << 32 | hashColor;
    }

    public static GameObject[,] Unhash(ulong hash)
    {
        uint hashPieces = (uint) (hash >> 32);
        uint temp  = (uint) (hash & uint.MaxValue);
        uint hashColor = 0;

        while (temp > 0)
        {
            uint bit = temp % 2;
            hashColor *= 2;
            hashColor += bit;
            temp /= 2;
        }

        GameObject[,] board = new GameObject[5, 5];

        int l = 12;

        for (int i = 0; i < 25; i++)
        {
            uint value = Choose(24 - i, l);
            if (value < hashPieces)
            {
                if (hashColor % 2 == 1)
                {
                    board[i % 5, i / 5] = GenerateCurrentPiece();
                }
                else
                {
                    board[i % 5, i / 5] = GenerateOtherPiece();
                }
                hashPieces -= value;
                hashColor /= 2;
            }
        }
        return board;
    }
}
