using System.Windows;
using Egor92.MvvmNavigation;
using Egor92.MvvmNavigation.Abstractions;
using SinglePageApp.Constants;
using SinglePageApp.ViewModels;
using SinglePageApp.Views;

namespace SinglePageApp
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
