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
    public Text boardString;

    private GameObject[,] pieces;

    private Player white;
    private Player black;
    public Player currentPlayer;
    public Player otherPlayer;
    public Player winner;

    protected Vector2Int[] lineDirections = {new Vector2Int(0,1), new Vector2Int(1, 0),
        new Vector2Int(1, 1), new Vector2Int(1, -1)};

    public GameManager()
    {
        Start();
    }

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        pieces = new GameObject[5, 5];

        white = new Player("white", true);
        black = new Player("black", false);

        currentPlayer = white;
        otherPlayer = black;

        InitialSetup();
        hashText.text = Serialize().ToString();
        boardString.text = BoardToString(pieces);
    }

    private void InitialSetup()
    {
        AddPiece(blackQueen, black, 0, 0);
        AddPiece(blackQueen, black, 0, 2);
        AddPiece(blackQueen, black, 2, 0);
        AddPiece(blackQueen, black, 4, 0);
        AddPiece(blackQueen, black, 1, 4);
        AddPiece(blackQueen, black, 3, 4);

        AddPiece(whiteQueen, white, 4, 4);
        AddPiece(whiteQueen, white, 4, 2);
        AddPiece(whiteQueen, white, 2, 4);
        AddPiece(whiteQueen, white, 0, 4);
        AddPiece(whiteQueen, white, 1, 0);
        AddPiece(whiteQueen, white, 3, 0);
        CombinatorialHash.SetPieces(white.pieces, black.pieces);
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

        if (board != null) {
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
        ulong hash = Serialize();
        hashText.text = hash.ToString();
        boardString.text = BoardToString(Deserialize(hash));
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
    }

    public string BoardToString(GameObject[,] pieces)
    {
        string boardString = "";
        for (int col = 4; col >= 0; col--)
        {
            for (int row = 0; row < 5; row++)
            {
                if (white.pieces.Contains(pieces[row, col]))
                {
                    boardString += "■";
                }
                else if (black.pieces.Contains(pieces[row, col]))
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

    // Solver Exclusive methods

    private GameManager(GameObject[,] pieces, Player currentPlayer, Player otherPlayer, Player white, Player black, Player winner) {
        this.pieces = (GameObject[,]) pieces.Clone();

        this.white = white;
        this.black = black;
        this.winner = winner;

        this.currentPlayer = currentPlayer;
        this.otherPlayer = otherPlayer;
    }

    public GameManager doMove(List<Vector2Int> move) {

        Vector2Int startGridPoint = move[0];
        Vector2Int endGridPoint = move[1];

        GameObject startPiece = PieceAtGrid(startGridPoint);

        GameManager newGameManager = new GameManager(pieces, currentPlayer, otherPlayer, white, black, winner);
        newGameManager.Move(startPiece, endGridPoint);
        newGameManager.checkWin(endGridPoint);
        newGameManager.NextPlayer();

        return newGameManager;
    }

    public List<List<Vector2Int>> generateMoves()
    {
        List<List<Vector2Int>> list = new List<List<Vector2Int>>();

        for (int i = 0; i < 25; i++)
        {
            Vector2Int start = new Vector2Int(i / 5, i % 5);
            GameObject piece = PieceAtGrid(start);

            if (piece && DoesPieceBelongToCurrentPlayer(piece))
            {
                foreach (Vector2Int end in MovesForPiece(piece))
                {
                    list.Add(new List<Vector2Int> { start, end });
                }
            }
        }
        return list;
    }

    public byte primitive() {
        if (winner != null) {
            if (winner == currentPlayer) {
                return 2;
            }
            if (winner == otherPlayer) {
                return 1;
            }
        }
        return 0;
    }

    private void checkWin(Vector2Int gridPoint) {
        if (winner != null) {
            return;
        }
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
                        winner = currentPlayer;
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

    private GameObject[,] flip(GameObject[,] src)
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
                dst[col, row] = src[col, height - 1 - row];
            }
        }
        return dst;
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
                if (CombinatorialHash.Hash(temp, currentPlayer) <= min)
                {
                    min = value;
                }
            }
        }
        return min;
    }

    public GameObject[,] Deserialize(ulong hash)
    {
        return CombinatorialHash.Unhash(hash);
    }
}
