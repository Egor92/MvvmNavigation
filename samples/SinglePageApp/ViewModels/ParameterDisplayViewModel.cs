using System.Windows.Input;
using Egor92.MvvmNavigation.Abstractions;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SinglePageApp.Constants;

namespace SinglePageApp.ViewModels
{
    public class ParameterDisplayViewModel(INavigationManager navigationManager) : ReactiveObject, INavigatedToAware
    {
        #region PassedParameter

        [Reactive] 
        public string PassedParameter { get; set; }

        #endregion

        #region ReturnBackCommand

        private ICommand _startOverCommand;

        public ICommand StartOverCommand => _startOverCommand ??= ReactiveCommand.Create(ReturnBack);

        private void ReturnBack()
        {
            navigationManager.Navigate(NavigationKeys.Welcome);
        }

        #endregion

        #region Implementation of INavigatedToAware

        public void OnNavigatedTo(object arg)
        {
            if (arg is not string str)
                return;

            PassedParameter = str;
        }

        #endregion
    }
}