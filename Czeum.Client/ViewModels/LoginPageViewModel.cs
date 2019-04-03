﻿using Czeum.Client.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Prism.Windows.Mvvm;
using System.ComponentModel;
using System.Windows.Input;
using Prism.Commands;
using Microsoft.Practices.Unity;
using Windows.UI.Xaml;

namespace Czeum.Client.ViewModels
{
    class LoginPageViewModel : ViewModelBase, ILoginPageViewModel, INotifyPropertyChanged
    {
        private enum PageState { Login, Register}
        private PageState pageState = PageState.Login;
        private IUserManagerService ums;

        private String _Name;
        public String Name {
            get { return _Name; }
            set { SetProperty(ref _Name, value);  }
        }
        private String m_Password;
        public String Password { 
			get { return m_Password; }
            set { SetProperty(ref m_Password, value); }
		}
        private String _ConfirmPassword;
        public String ConfirmPassword {
			get { return _ConfirmPassword; }
            set {SetProperty(ref _ConfirmPassword, value); }
        }
        private String _Email;
        public String Email
        {
            get { return _Email; }
            set { SetProperty(ref _Email, value); }
        }
        private Visibility _RegistrationInfoVisibility = Visibility.Collapsed;
        public Visibility RegistrationInfoVisibility
        {
            get { return _RegistrationInfoVisibility; }
            set { SetProperty(ref _RegistrationInfoVisibility, value); }
        }
        

        public ICommand ToggleClickCommand { get; private set; }
        public ICommand PerformClickCommand { get; private set; }
        

        private void PerformClick()
        {
            if (pageState == PageState.Login)
            {
                ums.LoginAsync(new DTO.UserManagement.LoginModel { Username = Name, Password = Password });
            }
            else
            {
                ums.RegisterAsync(new DTO.UserManagement.RegisterModel { Username = Name, Password = Password, ConfirmPassword = ConfirmPassword });
            }
        }

        private void ToggleClick()
        {
            switch (pageState)
            {
                case PageState.Register:
                    RegistrationInfoVisibility = Visibility.Collapsed;
                    pageState = PageState.Login;
                    break;
                case PageState.Login:
                    RegistrationInfoVisibility = Visibility.Visible;
                    pageState = PageState.Register;
                    break;
            }
        }

        public LoginPageViewModel(IUserManagerService userManagerService)
        {
            ums = userManagerService;
            PerformClickCommand = new DelegateCommand(PerformClick);
            ToggleClickCommand = new DelegateCommand(ToggleClick);
            RegistrationInfoVisibility = Visibility.Collapsed;
        }

    }
}
