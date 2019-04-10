﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Czeum.Abstractions.DTO;
using Czeum.Client.Interfaces;
using Czeum.DTO.Chess;
using Prism.Logging;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;

namespace Czeum.Client.ViewModels {
    class LobbyDetailsPageViewModel : ViewModelBase{
        private ILobbyService lobbyService;
        private ILoggerFacade loggerService;
        private INavigationService navigationService;
        private IUserManagerService userManagerService;
        private ITypeDispatcher typeDispatcher;

        private ILobbyRenderer lobbyRenderer;

        public LobbyData SelectedLobby  => lobbyService.CurrentLobby;

        public bool IsUserGuest => lobbyService.CurrentLobby.Guest == userManagerService.Username;

        public LobbyDetailsPageViewModel(INavigationService navigationService, ILoggerFacade loggerService,
            ILobbyService lobbyService, IUserManagerService userManagerService, ITypeDispatcher typeDispatcher
            )
        {
            this.lobbyService = lobbyService;
            this.navigationService = navigationService;
            this.loggerService = loggerService;
            this.userManagerService = userManagerService;
            this.typeDispatcher = typeDispatcher;

            lobbyRenderer = typeDispatcher.DispatchLobbyRenderer(lobbyService.CurrentLobby);
        }
    }
}
