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
            dialogManager.ShowMessage($"Your order '{SelectedFood.Name}' has been sent to the chef.");
            navigationManager.Navigate(NavigationKeys.FoodCooking, SelectedFood);
        }

        #endregion

        #region Implementation of INavigationAware

        public void OnNavigatedTo(object arg)
        {
            Foods = new Food[]
            {
                new Food("Borscht", 70),
                new Food("Shchi", 60),
                new Food("Potatoes", 30),
                new Food("Rice", 25),
                new Food("Pasta", 20),
                new Food("Olivier Salad", 45),
                new Food("Tea", 15),
                new Food("Compote", 20)
            };

            SelectedFood = Foods.FirstOrDefault();
        }

        #endregion
    }
}
