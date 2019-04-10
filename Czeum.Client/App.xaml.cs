﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using Prism.Unity.Windows;
using System.Threading.Tasks;
using Czeum.Client.Interfaces;
using Czeum.Client.Services;
using Microsoft.Practices.Unity;
using Prism.Logging;
using NLog;
using Prism.Windows.Navigation;

namespace Czeum.Client
{
    sealed partial class App : PrismUnityApplication
    {
        static readonly UnityContainer _container = new UnityContainer();
        public enum Experiences { Login }
   
        public static string Token { get; set; }
        public static readonly string AppUrl = "https://koppa96.sch.bme.hu/Czeum.Server";

        public App()
        {
            this.InitializeComponent();
        }

        protected override Task OnLaunchApplicationAsync(LaunchActivatedEventArgs args)
        {
            _container.RegisterInstance<INavigationService>(NavigationService);
            _container.RegisterType<IUserManagerService, DummyUserManagerService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ILoggerFacade, NLogAdapter>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ILobbyService, DummyLobbyService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IDialogService, DialogService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ILobbyRenderer, Connect4LobbyRenderer>("Connect4", new ContainerControlledLifetimeManager());
            _container.RegisterType<ILobbyRenderer, ChessLobbyRenderer>("Chess", new ContainerControlledLifetimeManager());
            _container.RegisterType<IEnumerable<ILobbyRenderer>, ILobbyRenderer[]>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ITypeDispatcher, TypeDispatcher>(new ContainerControlledLifetimeManager());
            this.NavigationService.Navigate(Experiences.Login.ToString(), null);
            return Task.FromResult<object>(null);
        }

        protected override object Resolve(Type type)
        {
            return _container.Resolve(type);
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        /*protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame() {
                    Name = "RootFrame"
                };

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {

                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    rootFrame.Navigate(typeof(LoginPage), e.Arguments);
                }
                // Ensure the current window is active
                Window.Current.Activate();
            }
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }*/
    }

    class NLogAdapter : ILoggerFacade
    {
        private Logger _logger = LogManager.GetCurrentClassLogger();
        public NLogAdapter()
        {
            var config = new NLog.Config.LoggingConfiguration();
            var logdebug = new NLog.Targets.DebuggerTarget("logdebug");
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logdebug);
            LogManager.Configuration = config;
        }

        public void Log(string message, Category category, Priority priority)
        {
            switch (category)
            {
                case Category.Debug:
                    _logger.Debug(message);
                    break;
                case Category.Exception:
                    _logger.Error(message);
                    break;
                case Category.Info:
                    _logger.Info(message);
                    break;
                case Category.Warn:
                    _logger.Warn(message);
                    break;
            }
        }
    }

}
