using System.Windows.Input;
using Egor92.MvvmNavigation.Abstractions;
using MvvmNavigation.Samples.Common;
using SinglePageApp.Constants;

namespace SinglePageApp.ViewModels
{
    public class WelcomeViewModel : ViewModelBase
    {
        #region Fields

        private readonly INavigationManager _navigationManager;

        #endregion

        #region Ctor

        public WelcomeViewModel(INavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
        }

        #endregion

        #region GoNextCommand

        private ICommand _goNextCommand;

        public ICommand GoNextCommand
        {
            get { return _goNextCommand ?? (_goNextCommand = new DelegateCommand(GoNext)); }
        }

        private void GoNext()
        {
            _navigationManager.Navigate(NavigationKeys.ParameterSelection);
        }

        #endregion
    }
}
