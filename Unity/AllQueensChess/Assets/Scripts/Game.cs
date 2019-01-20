using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game
{
    private static string white = "w";
    private static string black = "b";

    private string[,] pieces = new string[5, 5];

    private string currentTurn { get; }
    private string otherTurn { get; }

    protected Vector2Int[] lineDirections = {new Vector2Int(0,1), new Vector2Int(1, 0),
        new Vector2Int(1, 1), new Vector2Int(1, -1)};

    public Game()
    {
        currentTurn = white;
        otherTurn = black;

        AddPiece(black, 0, 0);
        AddPiece(black, 0, 2);
        AddPiece(black, 2, 0);
        AddPiece(black, 4, 0);
        AddPiece(black, 1, 4);
        AddPiece(black, 3, 4);

        AddPiece(white, 4, 4);
        AddPiece(white, 4, 2);
        AddPiece(white, 2, 4);
        AddPiece(white, 0, 4);
        AddPiece(white, 1, 0);
        AddPiece(white, 3, 0);
    }

    public Game(string[,] board, string current)
    {
        pieces = board;
        currentTurn = current;
        if (current != white)
        {
            otherTurn = white;
        }
        else
        {
            otherTurn = black;
        }
    }

    public byte primitive()
    {
        for (int i = 0; i < 25; i++)
        {
            Vector2Int gridPoint = new Vector2Int(i % 5, i / 5);
            if (PieceAtGrid(gridPoint) != null)
            {
                string piece = PieceAtGrid(gridPoint);
                foreach (Vector2Int dir in lineDirections)
                {
                    int lineLen = 0;
                    for (int j = -4; j < 5; j++)
                    {
                        if (DoesGridPointBelongToPlayer(new Vector2Int(dir.x * j + gridPoint.x, 
                            dir.y * j + gridPoint.y),
                            piece))
                        {
                            lineLen++;
                            if (lineLen >= 4)
                            {
                                if (piece == currentTurn)
                                {
                                    return 2;
                                }
                                if (piece == otherTurn)
                                {
                                    return 1;
                                }
                            }
                        }
                    }
                }
            }
        }
        return 0;
    }

    private void AddPiece(string player, int col, int row)
    {
        pieces[col, row] = player;
    }

    public string PieceAtGrid(Vector2Int gridPoint)
    {
        if (gridPoint.x >= 0 && gridPoint.x < 5 && gridPoint.y >= 0 && gridPoint.y < 5)
        {
            return null;
        }
        return pieces[gridPoint.x, gridPoint.y];
    }

    public bool FriendlyPieceAt(Vector2Int gridPoint)
    {
        string piece = PieceAtGrid(gridPoint);

        if (piece == null)
        {
            return false;
        }
 
        return pieces[gridPoint.x, gridPoint.y] == currentTurn;
    }

    private bool DoesGridPointBelongToPlayer(Vector2Int gridPoint, string player)
    {
        return (PieceAtGrid(gridPoint) != null && PieceAtGrid(gridPoint) == player);
    }

    public Game Move(Vector2Int start, Vector2Int end)
    {
        if (FriendlyPieceAt(start) && PieceAtGrid(end) == null)
        {
            string[,] temp = (string[,]) pieces.Clone();
            temp[start.x, start.y] = null;
            temp[end.x, end.y] = currentTurn;
            return new Game(temp, otherTurn);
        }
        return null;
    }

    private List<Vector2Int> MovesForPiece(Vector2Int gridPoint)
    {
        if (PieceAtGrid(gridPoint) != null)
        {
            List<Vector2Int> list = new List<Vector2Int>();
            for (int i = -4; i < 5; i++)
            {
                foreach (Vector2Int dir in lineDirections)
                {
                    list.Add(new Vector2Int(dir.x * i + gridPoint.x, dir.y * i + gridPoint.y));
                }
            }
            list.RemoveAll(tile => tile.x < 0 || tile.x > 4
                || tile.y < 0 || tile.y > 4);
            list.RemoveAll(tile => PieceAtGrid(tile) != null);
            return list;
        }
        return null;
    }

    public List<List<Vector2Int>> generateMoves()
    {
        List<List<Vector2Int>> list = new List<List<Vector2Int>>();

        for (int i = 0; i < 25; i++)
        {
            Vector2Int start = new Vector2Int(i / 5, i % 5);

            if (FriendlyPieceAt(start))
            {
                foreach (Vector2Int end in MovesForPiece(start))
                {
                    list.Add(new List<Vector2Int> { start, end });
                }
            }
        }
        return list;
    }

    public override string ToString()
    {
        string boardString = "";
        for (int col = 4; col >= 0; col--)
        {
            for (int row = 0; row < 5; row++)
            {
                if (DoesGridPointBelongToPlayer(new Vector2Int(row, col), white))
                {
                    boardString += "■";
                }
                else if (DoesGridPointBelongToPlayer(new Vector2Int(row, col), black))
                {
                    boardString += "□";
                }
                else
                {
                    boardString += "▪ ";
                }
            }
            boardString += "\n";
        }
        return boardString;
    }

    public ulong Serialize(Game game)
    {
        return 0;
    }

    public static Game Deserialize(ulong hash)
    {
        return null;
    }
}
