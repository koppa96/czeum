﻿using System;
using System.Collections.Generic;
using Czeum.Core.DTOs.Chess;

namespace Czeum.ChessLogic.Pieces
{
    public class Queen : StraightLineMover
    {
        public override PieceInfo PieceInfo => new PieceInfo()
        {
            Type = PieceType.Queen,
            Color = Color,
            Row = Field?.Row ?? -1,
            Column = Field?.Column ?? -1
        };

        public Queen(ChessBoard board, Color color) : base(board, color)
        {
        }

        public override bool CanMoveTo(Field targetField)
        {
            if (!base.CanMoveTo(targetField))
            {
                return false;
            }

            return targetField.Row == Field!.Row && Board.RouteClear(Field!, targetField) ||
                   targetField.Column == Field!.Column && Board.RouteClear(Field!, targetField) ||
                   Math.Abs(targetField.Row - Field!.Row) == Math.Abs(targetField.Column - Field!.Column) &&
                   Board.RouteClear(Field!, targetField);
        }

        public override List<ChessMoveData> GetPossibleMoves()
        {
            var directions = new List<Direction>
            {
                Direction.Above,
                Direction.AboveRight,
                Direction.Right,
                Direction.BelowRight,
                Direction.Below,
                Direction.BelowLeft,
                Direction.Left,
                Direction.AboveLeft
            };

            return GetPossibleMoves(directions);
        }
    }
}
