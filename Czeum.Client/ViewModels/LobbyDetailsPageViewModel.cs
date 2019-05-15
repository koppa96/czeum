﻿using Czeum.Abstractions.DTO;
using Czeum.Client.Interfaces;
using Czeum.Client.Views;
using Prism.Commands;
using Prism.Logging;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace Czeum.Client.ViewModels
{
    public class LobbyDetailsPageViewModel : ViewModelBase
    {
        private ILobbyService lobbyService;
        private ILoggerFacade loggerService;
        private INavigationService navigationService;
        private IUserManagerService userManagerService;
        public ILobbyStore lobbyStore { get; private set; }
        public ICommand SaveSettingsCommand { get; private set; }
        public ICommand CreateMatchCommand { get; private set; }

        public bool IsUserGuest => lobbyService.CurrentLobby.Guest == userManagerService.Username;

        public LobbyDetailsPageViewModel(INavigationService navigationService, ILoggerFacade loggerService,
            ILobbyService lobbyService, IUserManagerService userManagerService, ILobbyStore lobbyStore
            )
        {
            this.lobbyService = lobbyService;
            this.navigationService = navigationService;
            this.loggerService = loggerService;
            this.userManagerService = userManagerService;
            this.lobbyStore = lobbyStore;
            SaveSettingsCommand = new DelegateCommand(SaveLobbySettings);
            CreateMatchCommand = new DelegateCommand(CreateMatch);
        }

        private void CreateMatch()
        {
            lobbyService.CreateMatch();
        }

        private void SaveLobbySettings()
        {
            lobbyService.UpdateLobby(lobbyStore.SelectedLobby);
        }

        public override void OnNavigatingFrom(NavigatingFromEventArgs e, Dictionary<string, object> viewModelState, bool suspending)
        {
            lobbyService.LeaveLobby();
            base.OnNavigatingFrom(e, viewModelState, suspending);
        }
    }
}