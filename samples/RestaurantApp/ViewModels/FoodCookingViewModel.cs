using System.Threading;
using System.Threading.Tasks;
using Egor92.MvvmNavigation.Abstractions;
using RestaurantApp.Constants;
using RestaurantApp.Infrastructure.Implementations;
using RestaurantApp.Models;
using Samples.Common;

namespace RestaurantApp.ViewModels
{
    public class FoodCookingViewModel : ViewModelBase, IAsyncNavigatedToAware
    {
        #region Fields

        private readonly INavigationManager _navigationManager;
        private readonly DialogManager _dialogManager;

        #endregion

        #region Ctor

        public FoodCookingViewModel(INavigationManager navigationManager, DialogManager dialogManager)
        {
            _navigationManager = navigationManager;
            _dialogManager = dialogManager;
        }

        #endregion

        #region Properties

        #region Food

        private Food _food;

        public Food Food
        {
            get { return _food; }
            private set { SetProperty(ref _food, value); }
        }

        #endregion

        #region CookingProgress

        private int _cookingProgress;

        public int CookingProgress
        {
            get { return _cookingProgress; }
            private set { SetProperty(ref _cookingProgress, value); }
        }

        #endregion

        #endregion

        #region Implementation of INavigationAware

        public async Task OnNavigatedToAsync(object arg)
        {
            Food = (Food)arg;

            for (int i = 0; i <= 100; i++)
            {
                CookingProgress = i;
                await Task.Delay(50);
            }

            _dialogManager.ShowMessage($"Ваше блюдо готово. Садитесь кушать {Food.Name}, пожалуйста");
            _navigationManager.Navigate(NavigationKeys.FoodSelection);
        }

        #endregion
    }
}