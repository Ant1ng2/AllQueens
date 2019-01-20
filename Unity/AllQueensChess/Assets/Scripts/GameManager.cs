/*
 * Copyright (c) 2018 Razeware LLC
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * Notwithstanding the foregoing, you may not use, copy, modify, merge, publish,
 * distribute, sublicense, create a derivative work, and/or sell copies of the
 * Software in any work that is designed, intended, or marketed for pedagogical or
 * instructional purposes related to programming, coding, application development,
 * or information technology.  Permission for such use, copying, modification,
 * merger, publication, distribution, sublicensing, creation of derivative works,
 * or sale is expressly withheld.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    private List<Vector2Int> moveLocations;
    private List<GameObject> locationHighlights;

    public static GameManager instance;

    public Board board;
    public GameObject whiteQueen;
    public GameObject blackQueen;

    public Text winText;
    public Text hashText;
    public Text hashConfirmationText;
    public Text boardString;
    public Text gameBoardString;
    public InputField field;

    private GameObject[,] pieces;

    private static Player white;
    private static Player black;
    public Player currentPlayer;
    public Player otherPlayer;
    public Player winner;

    private Game game = new Game();

    protected Vector2Int[] lineDirections = {new Vector2Int(0,1), new Vector2Int(1, 0),
        new Vector2Int(1, 1), new Vector2Int(1, -1)};

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        ClearBoard();

        InitialSetup();
        SetText();
    }

    private void ClearBoard()
    {
        if (pieces != null)
        {
            for (int i = 0; i < 25; i++)
            {
                GameObject piece = pieces[i % 5, i / 5];
                if (piece != null)
                {
                    Destroy(piece);
                }
            }

        }

        pieces = new GameObject[5, 5];

        white = new Player("white", true);
        black = new Player("black", false);

        currentPlayer = white;
        otherPlayer = black;
    }

    public void Hashify()
    {
        ClearBoard();
        string[,] boardString = CombinatorialHash.UnhashString(Convert.ToUInt64(field.text), "w", "b");
        for (int i = 0; i < 25; i++)
        {
            if (boardString[i % 5, i / 5] == "w")
            {
                AddPiece(whiteQueen, white, i % 5, i / 5);
            }
            if (boardString[i % 5, i / 5] == "b")
            {
                AddPiece(blackQueen, black, i % 5, i / 5);
            }
        }
        SetText();
    }

    private void SetText()
    {
        ulong hash = Serialize();
        hashText.text = hash.ToString();
        game = Game.Deserialize(hash);
        boardString.text = game.ToString();

        ulong gameHash = game.Serialize();
        hashConfirmationText.text = gameHash.ToString();
        gameBoardString.text = Game.Deserialize(gameHash).ToString();
    }

    private void InitialSetup()
    {
        for (int i = 0; i < 25; i++)
        {
            if (game.DoesGridPointBelongToPlayer(new Vector2Int(i % 5, i / 5), "b"))
            {
                AddPiece(blackQueen, black, i % 5, i / 5);
            }
            if (game.DoesGridPointBelongToPlayer(new Vector2Int(i % 5, i / 5), "w"))
            {
                AddPiece(whiteQueen, white, i % 5, i / 5);
            }
        }
    }

    public void AddPiece(GameObject prefab, Player player, int col, int row)
    {
        GameObject pieceObject = board.AddPiece(prefab, col, row);
        player.pieces.Add(pieceObject);
        pieces[col, row] = pieceObject;
    }

    public void SelectPieceAtGrid(Vector2Int gridPoint)
    {
        GameObject selectedPiece = pieces[gridPoint.x, gridPoint.y];
        if (selectedPiece)
        {
            board.SelectPiece(selectedPiece);
        }
    }

    public void SelectPiece(GameObject piece)
    {
        board.SelectPiece(piece);
    }

    public void DeselectPiece(GameObject piece)
    {
        board.DeselectPiece(piece);
    }

    public GameObject PieceAtGrid(Vector2Int gridPoint)
    {
        if (gridPoint.x > 4 || gridPoint.y > 4 || gridPoint.x < 0 || gridPoint.y < 0)
        {
            return null;
        }
        return pieces[gridPoint.x, gridPoint.y];
    }

    public Vector2Int GridForPiece(GameObject piece)
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (pieces[i, j] == piece)
                {
                    return new Vector2Int(i, j);
                }
            }
        }

        return new Vector2Int(-1, -1);
    }

    public bool FriendlyPieceAt(Vector2Int gridPoint)
    {
        GameObject piece = PieceAtGrid(gridPoint);

        if (piece == null) {
            return false;
        }

        if (otherPlayer.pieces.Contains(piece))
        {
            return false;
        }

        return true;
    }

    public bool DoesPieceBelongToCurrentPlayer(GameObject piece)
    {
        return currentPlayer.pieces.Contains(piece);
    }

    public void Move(GameObject piece, Vector2Int gridPoint)
    {
        Vector2Int startGridPoint = GridForPiece(piece);
        pieces[startGridPoint.x, startGridPoint.y] = null;
        pieces[gridPoint.x, gridPoint.y] = piece;

        board.MovePiece(piece, gridPoint);

        foreach (Vector2Int dir in lineDirections) {
            int lineLen = 0;
            for (int i = -4; i < 5; i++)
            {
                GameObject newPiece = PieceAtGrid(new Vector2Int(dir.x * i + gridPoint.x, dir.y * i + gridPoint.y));
                if (newPiece && DoesPieceBelongToCurrentPlayer(newPiece))
                {
                    lineLen++;
                    if (lineLen >= 4)
                    {
                        winText.text = currentPlayer.name + " wins!";
                        Destroy(board.GetComponent<TileSelecter>());
                        Destroy(board.GetComponent<MoveSelecter>());
                        return;
                    }
                }
                else
                {
                    lineLen = 0;
                }
            }
        }
    }

    public List<Vector2Int> MovesForPiece(GameObject pieceObject)
    {
        Piece piece = pieceObject.GetComponent<Piece>();
        Vector2Int gridPoint = GridForPiece(pieceObject);
        var locations = piece.MoveLocations(gridPoint);

        // filter out offboard locations
        locations.RemoveAll(tile => tile.x < 0 || tile.x > 4
            || tile.y < 0 || tile.y > 4);

        // filter out locations with friendly piece
        locations.RemoveAll(tile => FriendlyPieceAt(tile));

        return locations;
    }

    public void NextPlayer()
    {
        Player tempPlayer = currentPlayer;
        currentPlayer = otherPlayer;
        otherPlayer = tempPlayer;
        SetText();
    }

    private GameObject[,] rotate(GameObject[,] src)
    {
        int width;
        int height;
        GameObject[,] dst;

        width = src.GetUpperBound(0) + 1;
        height = src.GetUpperBound(1) + 1;
        dst = new GameObject[height, width];

        for (int row = 0; row < height; row++)
        {
            for (int col = 0; col < width; col++)
            {
                int newRow;
                int newCol;

                newRow = col;
                newCol = height - (row + 1);

                dst[newCol, newRow] = src[col, row];
            }
        }
        return dst;
    }

    private GameObject[,] flip(GameObject[,] arrayToFlip)
    {
        int rows = arrayToFlip.GetLength(0);
        int columns = arrayToFlip.GetLength(1);
        GameObject[,] flippedArray = new GameObject[rows, columns];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                flippedArray[i, j] = arrayToFlip[(rows - 1) - i, j];
            }
        }
        return flippedArray;
    }

    public ulong Serialize()
    {
        ulong min = ulong.MaxValue;

        GameObject[,] temp = pieces;

        //Removes symmetries
        for (int i = 0; i < 4; i++)
        {
            temp = rotate(temp);
            for (int j = 0; j < 2; j++)
            {
                temp = flip(temp);
                ulong value = CombinatorialHash.Hash(temp, currentPlayer);
                if (value <= min)
                {
                    min = value;
                }
            }
        }
        return min;
    }
}
