using System.Windows.Input;
using Egor92.MvvmNavigation.Abstractions;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SinglePageApp.Constants;

namespace SinglePageApp.ViewModels
{
    public class ParameterSelectionViewModel(INavigationManager navigationManager) : ReactiveObject
    {
        #region Parameter

        [Reactive]
        public string Parameter { get; set; }

        #endregion

        #region GoNextCommand

        private ICommand _goNextCommand;

        public ICommand GoNextCommand => _goNextCommand ??= ReactiveCommand.Create(GoNext);

        private void GoNext()
        {
            navigationManager.Navigate(NavigationKeys.ParameterDisplay, Parameter);
        }

        #endregion
    }
}