﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace Czeum.Client.Interfaces {
    public interface IHubService {
        HubConnection Connection { get; }
        void CreateHubConnection();
        Task ConnectToHubAsync();
        Task DisconnectFromHub();
    }
}
