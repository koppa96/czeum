﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Connect4.Abstractions
{
    public interface IGameService
    {
        Status ExecuteMove(MoveData move, int playerId);
    }
}
