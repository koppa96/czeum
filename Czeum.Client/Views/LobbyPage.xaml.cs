﻿using Czeum.Abstractions.DTO;
using Czeum.Client.ViewModels;
using Czeum.DTO.Connect4;
using Prism.Windows.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Czeum.Client.Views {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LobbyPage : SessionStateAwarePage, INotifyPropertyChanged {
        public LobbyPage() {
            this.InitializeComponent();
            DataContextChanged += LobbyPageControl_DataContextChanged;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public LobbyPageViewModel ConcreteDataContext => DataContext as LobbyPageViewModel;

        private void LobbyPageControl_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ConcreteDataContext)));
        }
    }
}