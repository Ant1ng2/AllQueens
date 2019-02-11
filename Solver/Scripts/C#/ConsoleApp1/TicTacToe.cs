using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    public class TicTacToe : Game
    {
        #region Private Variables
        private static string player1 = "X";
        private static string player2 = "O";

        private string[,] pieces = new string[3, 3];

        protected Vector2Int[] lineDirections = {
            new Vector2Int(0, 1),
            new Vector2Int(1, 0),
            new Vector2Int(1, 1),
            new Vector2Int(1, -1),
        };
        #endregion

        #region Public Variables
        public override string currentTurn { get; }
        public override string otherTurn { get; }
        #endregion

        #region Initialization
        public TicTacToe()
        {
            currentTurn = player1;
            otherTurn = player2;
        }

        public TicTacToe(string[,] board, string current)
        {
            pieces = board;
            currentTurn = current;
            if (current != player1)
            {
                otherTurn = player1;
            }
            else
            {
                otherTurn = player2;
            }
        }
        #endregion

        #region Private game info
        private string GetPiece(Vector2Int gridPoint)
        {
            if (gridPoint.x >= 0 && gridPoint.x < 3 && gridPoint.y >= 0 && gridPoint.y < 3)
            {
                return pieces[gridPoint.x, gridPoint.y];
            }
            return null;
        }
        #endregion

        #region Public game info
        public override byte Primitive()
        {
            int numPieces = 0;
            for (int j = 0; j < 3; j++)
            {
                for (int i = 0; i < 3; i++)
                {
                    Vector2Int position = new Vector2Int(i, j);
                    string piece = GetPiece(position);

                    if (piece != null)
                    {
                        numPieces += 1;
                        int lineLen = 0;
                        foreach (Vector2Int dir in lineDirections)
                        {
                            for (int f = -2; f < 3; f++)
                            {
                                Vector2Int check = new Vector2Int(position.x + f * dir.x, 
                                    position.y + f * dir.y);
                                if (GetPiece(check) != null && GetPiece(check) == piece)
                                {
                                    lineLen += 1;
                                }
                                else
                                {
                                    lineLen = 0;
                                }

                                if (lineLen >= 3)
                                {
                                    if (piece == otherTurn)
                                    {
                                        return 1;
                                    }
                                    if (piece == currentTurn)
                                    {
                                        return 2;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (numPieces >= 9)
            {
                return 3;
            }
            return 0;
        }

        public override Game Move(List<Vector2Int> list)
        {
            Vector2Int position = list[0];

            if (GetPiece(position) == null)
            {
                string[,] temp = (string[,])pieces.Clone();
                temp[position.x, position.y] = currentTurn;
                return new TicTacToe(temp, otherTurn);
            }
            return null;
        }

        public override List<List<Vector2Int>> GenerateMoves()
        {
            List<List<Vector2Int>> list = new List<List<Vector2Int>>();
            for (int i = 0; i < 9; i++)
            {
                Vector2Int position = new Vector2Int(i / 3, i % 3);
                List<Vector2Int> addition = new List<Vector2Int>() { new Vector2Int(i / 3, i % 3) };
                if (GetPiece(position) == null)
                {
                    list.Add(addition);
                }
            }
            return list;
        }

        public override string ToString()
        {
            string boardString = "";
            for (int j = 2; j >= 0; j--)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (GetPiece(new Vector2Int(i, j)) == null)
                    {
                        boardString += " ";
                    }
                    else
                    {
                        boardString += GetPiece(new Vector2Int(i, j));
                    }
                }
                boardString += "\n";
            }
            return boardString;
        }

        public override ulong Serialize()
        {
            string boardString = "1";
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (GetPiece(new Vector2Int(i, j)) == null)
                    {
                        boardString += "3";
                    }
                    else if (GetPiece(new Vector2Int(i, j)) == "X")
                    {
                        boardString += "2";
                    }
                    else
                    {
                        boardString += "1";
                    }
                }
            }
            return Convert.ToUInt64(boardString);
        }

        public override Game Deserialize(ulong hash)
        {
            throw new NotImplementedException();
        }

        public override (bool, List<Vector2Int>) Prompt()
        {
            Console.Write("Enter Position: ");
            string position = Console.ReadLine();

            if (position.Length != 3 || position[1] != ',')
            {
                return (false, null);
            }

            Vector2Int begin = new Vector2Int(position[0] - '0', position[2] - '0');

            List<Vector2Int> move = new List<Vector2Int>() { begin };

            bool exists = false;

            foreach (List<Vector2Int> possible in GenerateMoves())
            {
                if (move[0].Equals(possible[0]))
                {
                    exists = true;
                    break;
                }
            }
            return (exists, move);
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
