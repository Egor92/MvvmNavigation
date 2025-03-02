using System.Threading;
using System.Threading.Tasks;
using Egor92.MvvmNavigation.Abstractions;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using RestaurantApp.Constants;
using RestaurantApp.Infrastructure.Implementations;
using RestaurantApp.Models;

namespace RestaurantApp.ViewModels
{
    public class FoodCookingViewModel(INavigationManager navigationManager, DialogManager dialogManager)
        : ReactiveObject, IAsyncNavigatedToAware
    {
        [Reactive]
        public Food Food { get; set; }

        [Reactive]
        public int CookingProgress { get; set; }

        #region Implementation of INavigationAware

        public async Task OnNavigatedToAsync(object arg, CancellationToken token = default)
        {
            Food = (Food)arg;

            for (int i = 0; i <= 100; i++)
            {
                CookingProgress = i;
                await Task.Delay(50, token);
            }

            dialogManager.ShowMessage($"Ваше блюдо готово. Садитесь кушать {Food.Name}, пожалуйста");
            navigationManager.Navigate(NavigationKeys.FoodSelection);
        }

        #endregion
    }
}