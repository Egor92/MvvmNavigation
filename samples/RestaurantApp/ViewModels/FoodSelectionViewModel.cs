using System;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Input;
using Egor92.MvvmNavigation.Abstractions;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using RestaurantApp.Constants;
using RestaurantApp.Infrastructure.Implementations;
using RestaurantApp.Models;

namespace RestaurantApp.ViewModels
{
    public class FoodSelectionViewModel(INavigationManager navigationManager, DialogManager dialogManager)
        : ReactiveObject, INavigatedToAware
    {
        [Reactive]
        public Food[] Foods { get; set; }

        [Reactive]
        public Food SelectedFood { get; set; }

        #region CookFoodCommand

        private ICommand _cookFoodCommand;

        public ICommand CookFoodCommand => _cookFoodCommand ??= ReactiveCommand.Create(CookFood, WhenCanCookFoodChanged());

        private IObservable<bool> WhenCanCookFoodChanged()
        {
            return this.ObservableForProperty(x => x.SelectedFood)
                .Select(x => x.Value != null);
        }

        private void CookFood()
        {
            dialogManager.ShowMessage($"Ваш заказ '{SelectedFood.Name}' отправлен к шеф-повару");
            navigationManager.Navigate(NavigationKeys.FoodCooking, SelectedFood);
        }

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
