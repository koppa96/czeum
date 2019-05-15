﻿using Czeum.Client.Interfaces;
using Czeum.DTO;
using Czeum.DTO.Connect4;
using Prism.Commands;
using Prism.Windows.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Czeum.Client.ViewModels
{
    class Connect4PageViewModel : ViewModelBase
    {
        public IMatchStore matchStore { get; }
        private IMatchService matchService;

        public MatchStatus Match { get => matchStore.SelectedMatch; }

        public ICommand ObjectPlacedCommand { get; set; }

        public Connect4PageViewModel(IMatchStore matchStore, IMatchService matchService)
        {
            this.matchStore = matchStore;
            this.matchService = matchService;

            ObjectPlacedCommand = new DelegateCommand<Tuple<int, int>>(ObjectPlaced);
        }

        private void ObjectPlaced(Tuple<int, int> position)
        {
            var moveData = new Connect4MoveData() { MatchId = matchService.CurrentMatch.MatchId, Column = position.Item2 };
            matchService.DoMove(moveData);
        }

    }
}