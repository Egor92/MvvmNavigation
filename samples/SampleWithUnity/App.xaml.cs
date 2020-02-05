using System.Windows;
using Egor92.UINavigation.Abstractions;
using Egor92.UINavigation.Unity;
using SampleWithUnity.ViewModels;
using SampleWithUnity.Views;
using Unity;

namespace SampleWithUnity
{
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var mainWindow = new MainWindow();
            var unityContainer = new UnityContainer();

            unityContainer.RegisterNavigationManager(mainWindow);
            unityContainer.RegisterNavigationRule<LeonardoViewModel, TurtleView>(NavigationKeys.Leonardo);
            unityContainer.RegisterNavigationRule<RaphaelViewModel, TurtleView>(NavigationKeys.Raphael);
            unityContainer.RegisterNavigationRule<MichelangeloViewModel, TurtleView>(NavigationKeys.Michelangelo);
            unityContainer.RegisterNavigationRule<DonatelloViewModel, TurtleView>(NavigationKeys.Donatello);

            var navigationManager = unityContainer.Resolve<INavigationManager>();
            navigationManager.Navigate(NavigationKeys.Leonardo);

            mainWindow.Show();
        }
    }
}
