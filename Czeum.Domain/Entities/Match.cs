﻿using System;
using System.Collections.Generic;
using Czeum.Core.Domain;
using Czeum.Domain.Enums;

namespace Czeum.Domain.Entities
{
    public class Match : EntityBase
    {
        public bool IsQuickMatch { get; set; }
        public MatchState State { get; private set; }
        public SerializedBoard Board { get; set; }

        public List<StoredMessage> Messages { get; set; }
        public List<UserMatch> Users { get; set; }
        public int CurrentPlayerIndex { get; set; }
        
        public User Winner { get; set; }
        public Guid? WinnerId { get; set; }

        public DateTime CreateTime { get; set; }
        public DateTime? LastMove { get; set; }

        public Match()
        {
            State = MatchState.InProgress;
        }

        public void NextTurn()
        {
            if (State == MatchState.InProgress)
            {
                CurrentPlayerIndex++;
                if (CurrentPlayerIndex >= Users.Count)
                {
                    CurrentPlayerIndex = 0;
                }
            }
            else
            {
                throw new InvalidOperationException("This match has already ended.");
            }
        }

        public void CurrentPlayerWon()
        {
            if (State == MatchState.Finished)
            {
                throw new InvalidOperationException("This match has already ended.");
            }

            Winner = Users[CurrentPlayerIndex].User;
            State = MatchState.Finished;
        }

        public void Draw()
        {
            State = MatchState.Finished;
        }
    }
}
