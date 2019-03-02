﻿using Connect4.Abstractions;
using Connect4.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Connect4.Connect4Logic
{
    public class Connect4Board : ISerializableBoard<SerializedConnect4Board>
    {
        private const int DefaultWidth = 7, DefaultHeight = 6;

        private int height, width;
        private Item[,] board;

        public bool Full 
        {
            get
            {
                //Elegendő csupán az első sort ellenőrizni
                for (int i = 0; i < width; i++)
                {
                    if (board[0, i] == Item.None)
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public Connect4Board(int width = DefaultWidth, int height = DefaultHeight)
        {
            this.width = width;
            this.height = height;
            board = new Item[height, width];
        }

        public bool PlaceItem(Item item, int column)
        {
            if (column > width - 1)
            {
                throw new ArgumentException("This board does not have a column with that index.");
            }

            for (int i = height - 1; i >= 0; i--)
            {
                if (board[i, column] == Item.None)
                {
                    board[i, column] = item;
                    return true;
                }
            }

            return false;
        }

        public void FillFromSerialized(SerializedConnect4Board serializedBoard)
        {
            width = serializedBoard.Width;
            height = serializedBoard.Height;
            board = new Item[height, width];

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    switch (serializedBoard.BoardData[i * width + j])
                    {
                        case 'R':
                            board[i, j] = Item.Red;
                            break;
                        case 'Y':
                            board[i, j] = Item.Yellow;
                            break;
                        case 'N':
                            board[i, j] = Item.None;
                            break;
                        default:
                            throw new ArgumentException($"Error while processing the data of the board: {serializedBoard.BoardData}");
                    }
                }
            }
        }

        private bool HorizontalMatch(Item item)
        {
            for (int i = 0; i < height; i++)
            {
                int count = 0;
                for (int j = 0; j < width; j++)
                {
                    if (board[i, j] == item)
                    {
                        count++;

                        if (count == 4)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        count = 0;
                    }
                }
            }

            return false;
        }

        private bool VerticalMatch(Item item)
        {
            for (int j = 0; j < width; j++)
            {
                int count = 0;
                for (int i = 0; i < height; i++)
                {
                    if (board[i, j] == item)
                    {
                        count++;

                        if (count == 4)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        count = 0;
                    }
                }
            }

            return false;
        }

        private bool LeftDiagonalMatch(Item item)
        {
            int startRow = height - 4, startCol = 0;

            while (startCol <= width - 4)
            {
                int count = 0;
                for (int i = startRow, j = startCol; i < height && j < width; i++, j++)
                {
                    if (board[i, j] == item)
                    {
                        count++;

                        if (count == 4)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        count = 0;
                    }
                }

                if (startRow != 0)
                {
                    startRow--;
                }
                else
                {
                    startCol++;
                }
            }

            return false;
        }

        private bool RightDiagonalMatch(Item item)
        {
            int startRow = 0, startCol = 3;

            while (startRow <= height - 4)
            {
                int count = 0;
                for (int i = startRow, j = startCol; i < height && j >= 0; i++, j--)
                {
                    if (board[i, j] == item)
                    {
                        count++;

                        if (count == 4)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        count = 0;
                    }
                }

                if (startCol != height - 1)
                {
                    startCol++;
                }
                else
                {
                    startRow++;
                }
            }

            return false;
        }

        public Item CheckWinner()
        {
            foreach (Item item in Enum.GetValues(typeof(Item)))
            {
                if (item != Item.None && (HorizontalMatch(item) || VerticalMatch(item) || LeftDiagonalMatch(item) || RightDiagonalMatch(item)))
                {
                    return item;
                }
            }

            return Item.None;
        }

        public SerializedConnect4Board ToSerialized()
        {
            SerializedConnect4Board serialized = new SerializedConnect4Board
            {
                Width = width,
                Height = height
            };

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    switch (board[i, j])
                    {
                        case Item.Red:
                            builder.Append('R');
                            break;
                        case Item.Yellow:
                            builder.Append('Y');
                            break;
                        case Item.None:
                            builder.Append('N');
                            break;
                    }
                }
            }
            serialized.BoardData = builder.ToString();

            return serialized;
        }
    }
}
