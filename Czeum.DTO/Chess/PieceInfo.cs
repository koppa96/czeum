﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Czeum.DTO.Chess
{
    /// <summary>
    /// A representation of a chess piece that is sent to the clients.
    /// </summary>
    public class PieceInfo
    {
        public Color Color { get; set; }
        public PieceType Type { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
    }
}
