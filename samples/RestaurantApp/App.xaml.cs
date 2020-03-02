using System.Windows;
using Egor92.MvvmNavigation;
using RestaurantApp.Constants;
using RestaurantApp.Infrastructure.Implementations;
using RestaurantApp.ViewModels;
using RestaurantApp.Views;

namespace RestaurantApp
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var window = new MainWindow();
            var navigationManager = new NavigationManager(window.FrameContent);

            var dialogManager = new DialogManager();

            var viewModel = new MainWindowViewModel(navigationManager);
            window.DataContext = viewModel;

            navigationManager.Register<FoodSelectionView>(NavigationKeys.FoodSelection, () => new FoodSelectionViewModel(navigationManager, dialogManager));
            navigationManager.Register<FoodCookingView>(NavigationKeys.FoodCooking, () => new FoodCookingViewModel(navigationManager, dialogManager));

            window.Show();
        }
    }
}
