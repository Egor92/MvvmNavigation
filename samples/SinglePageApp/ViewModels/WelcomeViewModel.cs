using System.Windows.Input;
using Egor92.MvvmNavigation.Abstractions;
using ReactiveUI;
using SinglePageApp.Constants;

namespace SinglePageApp.ViewModels
{
    public class WelcomeViewModel(INavigationManager navigationManager) : ReactiveObject
    {
        #region GoNextCommand

        private ICommand _goNextCommand;

        public ICommand GoNextCommand => _goNextCommand ??= ReactiveCommand.Create(GoNext);

        private void GoNext()
        {
            navigationManager.Navigate(NavigationKeys.ParameterSelection);
        }

        #endregion
    }
}
