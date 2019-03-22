﻿using Czeum.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Czeum.Abstractions.DTO;
using Czeum.DAL.Entities;
using Czeum.DAL.Interfaces;

namespace Czeum.DAL.Repositories
{
    public class BoardRepository<T> : IBoardRepository<T> where T : SerializedBoard
    {
        private readonly ApplicationDbContext _context;

        public BoardRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void DeleteBoard(T board)
        {
            _context.Boards.Remove(board);
            _context.SaveChanges();
        }

        public T GetById(int id)
        {
            var board = _context.Boards.Find(id);
            return ValidateBoard(board);
        }

        public T GetByMatchId(int id)
        {
            var board = _context.Boards.FirstOrDefault(b => b.Match.MatchId == id);
            return ValidateBoard(board);
        }

        public int InsertBoard(T board)
        {
            _context.Boards.Add(board);
            _context.SaveChanges();
            return board.BoardId;
        }

        public void UpdateBoard(T board)
        {
            var outdatedBoard = GetById(board.BoardId);
            _context.Entry(outdatedBoard).CurrentValues.SetValues(board);
            _context.SaveChanges();
        }

        public void UpdateBoardData(int id, string newData)
        {
            var board = GetById(id);
            board.BoardData = newData;
            _context.SaveChanges();
        }

        private static T ValidateBoard(SerializedBoard board)
        {
            if (board == null)
            {
                throw new NullReferenceException("There is no board with such ID.");
            }

            return (T)board;
        }

        public MoveResult GetMoveResultByMatchId(int matchId)
        {
            var board = GetByMatchId(matchId);
            return board.ToMoveResult();
        }
    }
}
