﻿using Czeum.ClientCallback;

namespace Czeum.Server.Hubs
{
    public interface ICzeumClient : IGameClient, ILobbyClient, IErrorClient, IFriendClient
    {
    }
}
