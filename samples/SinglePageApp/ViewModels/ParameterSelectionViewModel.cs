using System.Windows.Input;
using Egor92.MvvmNavigation.Abstractions;
using Samples.Common;
using SinglePageApp.Constants;

namespace SinglePageApp.ViewModels
{
    public class ParameterSelectionViewModel : ViewModelBase
    {
        #region Fields

        private readonly INavigationManager _navigationManager;

        #endregion

        #region Ctor

        public ParameterSelectionViewModel(INavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
        }

        #endregion

        #region Parameter

        private string _parameter;

        public string Parameter
        {
            get { return _parameter; }
            set { SetProperty(ref _parameter, value); }
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
            _navigationManager.Navigate(NavigationKeys.ParameterDisplay, Parameter);
        }

        #endregion
    }
}
