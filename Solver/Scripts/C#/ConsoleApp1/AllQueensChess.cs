using System.Collections;
using System.Collections.Generic;
using System;

namespace ConsoleApp1
{
    public class AllQueensChess : Game
    {
        #region Private variables
        private static string white = "w";
        private static string black = "b";

        private string[,] pieces = new string[5, 5];

        protected Vector2Int[] lineDirections = {new Vector2Int(0,1), new Vector2Int(1, 0),
            new Vector2Int(1, 1), new Vector2Int(1, -1),
            new Vector2Int(0, -1), new Vector2Int(-1,0),
            new Vector2Int(-1, -1), new Vector2Int(-1, 1)};
        #endregion

        #region Public Variables
        public override string currentTurn { get; }
        public override string otherTurn { get;  }
        #endregion

        #region Initialize
        public AllQueensChess()
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

        private AllQueensChess(string[,] board, string current)
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
        #endregion

        #region Public Methods

        public override byte Primitive()
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

        public override Game Move(List<Vector2Int> list)
        {
            Vector2Int start = list[0];
            Vector2Int end = list[1];

            if (FriendlyPieceAt(start) && PieceAtGrid(end) == null)
            {
                string[,] temp = (string[,])pieces.Clone();
                temp[start.x, start.y] = null;
                temp[end.x, end.y] = currentTurn;
                return new AllQueensChess(temp, otherTurn);
            }
            return null;
        }

        public override List<List<Vector2Int>> GenerateMoves()
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
                        boardString += "W";
                    }
                    else if (DoesGridPointBelongToPlayer(new Vector2Int(row, col), black))
                    {
                        boardString += "B";
                    }
                    else
                    {
                        boardString += "-";
                    }
                }
                boardString += "\n";
            }
            return boardString;
        }

        public override ulong Serialize()
        {
            ulong min = ulong.MaxValue;

            string[,] temp = pieces;

            //Removes symmetries
            for (int i = 0; i < 4; i++)
            {
                temp = rotate(temp);
                for (int j = 0; j < 2; j++)
                {
                    temp = flip(temp);
                    ulong value = CombinatorialHash.HashString(pieces, currentTurn);
                    if (value <= min)
                    {
                        min = value;
                    }
                }
            }
            return min;
        }

        public override Game Deserialize(ulong hash)
        {
            string[,] board = CombinatorialHash.UnhashString(hash, white, black);
            return new AllQueensChess(board, white);
        }

        public override (bool, List<Vector2Int>) Prompt()
        {
            Console.Write("Enter Piece: ");
            string first = Console.ReadLine();
            Console.Write("Enter End: ");
            string second = Console.ReadLine();

            Vector2Int begin = new Vector2Int(first[0] - '0', first[2] - '0');
            Vector2Int finish = new Vector2Int(second[0] - '0', second[2] - '0');

            List<Vector2Int> move = new List<Vector2Int>() { begin, finish };

            bool exists = false;

            foreach (List<Vector2Int> possible in GenerateMoves())
            {
                if (move[0].Equals(possible[0]) && move[1].Equals(possible[1]))
                {
                    exists = true;
                    break;
                }
            }
            return (exists, move);
        }

        #endregion

        #region Private Methods
        private void AddPiece(string player, int col, int row)
        {
            pieces[col, row] = player;
        }

        private string PieceAtGrid(Vector2Int gridPoint)
        {
            if (gridPoint.x >= 0 && gridPoint.x < 5 && gridPoint.y >= 0 && gridPoint.y < 5)
            {
                return pieces[gridPoint.x, gridPoint.y];
            }
            return null;
        }

        private bool FriendlyPieceAt(Vector2Int gridPoint)
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

        private List<Vector2Int> MovesForPiece(Vector2Int gridPoint)
        {
            if (PieceAtGrid(gridPoint) != null)
            {
                List<Vector2Int> list = new List<Vector2Int>();
                foreach (Vector2Int dir in lineDirections)
                {
                    for (int i = 1; i < 5; i++)
                    {
                        Vector2Int position = new Vector2Int(dir.x * i + gridPoint.x, dir.y * i + gridPoint.y);
                        if (PieceAtGrid(position) != null)
                        {
                            i = 5;
                        }
                        else
                        {
                            list.Add(position);
                        }
                    }
                }
                list.RemoveAll(tile => tile.x < 0 || tile.x > 4
                    || tile.y < 0 || tile.y > 4);
                list.RemoveAll(tile => PieceAtGrid(tile) != null);
                return list;
            }
            return null;
        }
        #endregion

        #region Optimizations
        private static string[,] rotate(string[,] src)
        {
            int width;
            int height;
            string[,] dst;

            width = src.GetUpperBound(0) + 1;
            height = src.GetUpperBound(1) + 1;
            dst = new string[height, width];

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

        private static string[,] flip(string[,] arrayToFlip)
        {
            int rows = arrayToFlip.GetLength(0);
            int columns = arrayToFlip.GetLength(1);
            string[,] flippedArray = new string[rows, columns];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    flippedArray[i, j] = arrayToFlip[(rows - 1) - i, j];
                }
            }
            return flippedArray;
        }
        #endregion
    }
}
