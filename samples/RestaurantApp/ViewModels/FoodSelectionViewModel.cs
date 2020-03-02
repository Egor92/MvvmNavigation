using System.Linq;
using Egor92.MvvmNavigation.Abstractions;
using RestaurantApp.Constants;
using RestaurantApp.Infrastructure.Implementations;
using RestaurantApp.Models;
using Samples.Common;

namespace RestaurantApp.ViewModels
{
    public class FoodSelectionViewModel : ViewModelBase, INavigatedToAware
    {
        #region Fields

        private readonly INavigationManager _navigationManager;
        private readonly DialogManager _dialogManager;

        #endregion

        #region Ctor

        public FoodSelectionViewModel(INavigationManager navigationManager, DialogManager dialogManager)
        {
            _navigationManager = navigationManager;
            _dialogManager = dialogManager;
        }

        #endregion

        #region Properties

        #region Foods

        private Food[] _foods;

        public Food[] Foods
        {
            get { return _foods; }
            private set { SetProperty(ref _foods, value); }
        }

        #endregion

        #region SelectedFood

        private Food _selectedFood;

        public Food SelectedFood
        {
            get { return _selectedFood; }
            set
            {
                SetProperty(ref _selectedFood, value);
                CookFoodCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region CookFoodCommand

        private DelegateCommand _cookFoodCommand;

        public DelegateCommand CookFoodCommand
        {
            get { return _cookFoodCommand ?? (_cookFoodCommand = new DelegateCommand(CookFood, CanCookFood)); }
        }

        private void CookFood()
        {
            _dialogManager.ShowMessage($"Ваш заказ '{SelectedFood.Name}' отправлен к шеф-повару");
            _navigationManager.Navigate(NavigationKeys.FoodCooking, SelectedFood);
        }

        private bool CanCookFood()
        {
            return SelectedFood != null;
        }

        #endregion

        #endregion

        #region Implementation of INavigationAware

        public void OnNavigatedTo(object arg)
        {
            Foods = new Food[]
            {
                new Food("Борщ", 70),
                new Food("Щи", 60),
                new Food("Картофель", 30),
                new Food("Рис", 25),
                new Food("Макароны", 20),
                new Food("Оливье", 45),
                new Food("Чай", 15),
                new Food("Компот", 20),
            };

            SelectedFood = Foods.FirstOrDefault();
        }

        #endregion
    }
}
