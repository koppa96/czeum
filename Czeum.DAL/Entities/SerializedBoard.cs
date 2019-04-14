﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Czeum.Abstractions;
using Czeum.Abstractions.DTO;

namespace Czeum.DAL.Entities
{
    public abstract class SerializedBoard : ISerializedBoard
    {
        [Key]
        public int BoardId { get; set; }
        public string BoardData { get; set; }

        public Match Match { get; set; }

        [ForeignKey("Match")]
        public int MatchId { get; set; }
    }
}
