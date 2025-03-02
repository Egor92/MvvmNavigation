using System.Windows.Input;
using Egor92.MvvmNavigation.Abstractions;
using ReactiveUI;
using RestaurantApp.Constants;

namespace RestaurantApp.ViewModels
{
    public class MainWindowViewModel(INavigationManager navigationManager) : ReactiveObject
    {
        #region ShowFoodSelectionCommand

        private ICommand _showFoodSelectionCommand;

        public ICommand ShowFoodSelectionCommand => _showFoodSelectionCommand ??= ReactiveCommand.Create(ShowFoodSelection);

        private async void ShowFoodSelection()
        {
            await navigationManager.NavigateAsync(NavigationKeys.FoodSelection);
        }

        #endregion
    }
}
