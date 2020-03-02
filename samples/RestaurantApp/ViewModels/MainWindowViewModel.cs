using System.Windows.Input;
using Egor92.MvvmNavigation.Abstractions;
using RestaurantApp.Constants;
using Samples.Common;

namespace RestaurantApp.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Fields

        private readonly INavigationManager _navigationManager;

        #endregion

        #region Ctor

        public MainWindowViewModel(INavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
        }

        #endregion

        #region ShowFoodSelectionCommand

        private ICommand _showFoodSelectionCommand;

        public ICommand ShowFoodSelectionCommand
        {
            get { return _showFoodSelectionCommand ?? (_showFoodSelectionCommand = new DelegateCommand(ShowFoodSelection)); }
        }

        private void ShowFoodSelection()
        {
            _navigationManager.Navigate(NavigationKeys.FoodSelection);
        }

        #endregion
    }
}
