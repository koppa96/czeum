﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Czeum.Client.Interfaces;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Prism.Windows.Navigation;

namespace Czeum.Client.Services {
    public class HubService : IHubService {
        private INavigationService navigationService;
        private IDialogService dialogService;

        private string BASE_URL = "https://localhost:5001";
        //private string BASE_URL = App.Current.Resources["BaseUrl"].ToString();
        private IUserManagerService userManagerService;

        public HubConnection Connection { get; private set; }

        public HubService(INavigationService navigationService, IDialogService dialogService, IUserManagerService userManagerService)
        {
            this.navigationService = navigationService;
            this.dialogService = dialogService;
            this.userManagerService = userManagerService;

            Connection = new HubConnectionBuilder()
                .WithUrl(Flurl.Url.Combine(BASE_URL, "notifications"), options => {
                    options.AccessTokenProvider = () =>
                        Task.FromResult(userManagerService.AccessToken);
                }).AddNewtonsoftJsonProtocol()
                .Build();
        }

        public async Task ConnectToHubAsync()
        {
            if(Connection.State == HubConnectionState.Connected)
            {
                return;
            }
            await Connection.StartAsync();
        }

        public async Task DisconnectFromHub()
        {
            await Connection.StopAsync();
        }
    
    }
}
