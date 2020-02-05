using System.Windows;
using Egor92.UINavigation;
using Egor92.UINavigation.Abstractions;
using Egor92.UINavigation.Wpf;
using UINavigation.Samples.Wpf.SinglePageApp.Constants;
using UINavigation.Samples.Wpf.SinglePageApp.ViewModels;
using UINavigation.Samples.Wpf.SinglePageApp.Views;

namespace UINavigation.Samples.Wpf.SinglePageApp
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var mainWindow = new MainWindow();
            var navigationManager = new NavigationManager(mainWindow);
            navigationManager.Register<WelcomeView>(NavigationKeys.Welcome, () => new WelcomeViewModel(navigationManager));
            navigationManager.Register<ParameterSelectionView>(NavigationKeys.ParameterSelection, () => new ParameterSelectionViewModel(navigationManager));
            navigationManager.Register<ParameterDisplayView>(NavigationKeys.ParameterDisplay, () => new ParameterDisplayViewModel(navigationManager));

            mainWindow.Show();
            navigationManager.Navigate(NavigationKeys.Welcome);
        }
    }
}
