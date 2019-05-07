﻿using Czeum.Client.Interfaces;
using Czeum.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace Czeum.Client.Models
{
    class MatchStore : IMatchStore, INotifyPropertyChanged
    {
        public ObservableCollection<MatchStatus> MatchList { get; private set; }
        private MatchStatus selectedMatch;

        public MatchStatus SelectedMatch {
            get => selectedMatch;
            set {
                selectedMatch = value;
                CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedMatch")); });
            }
        }

        public MatchStore()
        {
            MatchList = new ObservableCollection<MatchStatus>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public async Task AddMatch(MatchStatus match)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { MatchList.Add(match); });
        }

        public async Task ClearMatches()
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { MatchList.Clear(); });
        }

        public async Task RemoveMatch(int matchId)
        {
            var matchToRemove = MatchList.FirstOrDefault(x => x.MatchId == matchId);
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { MatchList.Remove(matchToRemove); });
        }

        public async Task UpdateMatch(MatchStatus match)
        {
            var matchToUpdate = MatchList.FirstOrDefault(x => x.MatchId== match.MatchId);
            int index = MatchList.IndexOf(matchToUpdate);
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                //dirty hack to refresh item in the list
                MatchList.RemoveAt(index);
                MatchList.Insert(index, match);

                if ((selectedMatch != null) && (selectedMatch.MatchId == match.MatchId))
                {
                    SelectedMatch = match;
                }
            });
        }

        public void SelectMatch(MatchStatus match)
        {
            var matchToSelect = MatchList.Where(x => x.MatchId == match.MatchId).FirstOrDefault();
            SelectedMatch = matchToSelect;
        }
    }
}