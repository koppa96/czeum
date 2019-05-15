﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Czeum.DAL.Entities
{
    public class Match
    {
        public int MatchId { get; set; }

        public ApplicationUser Player1 { get; set; }
        public ApplicationUser Player2 { get; set; }

        public MatchState State { get; set; }
        public SerializedBoard Board { get; set; }

        public List<StoredMessage> Messages { get; set; }
    }
}
