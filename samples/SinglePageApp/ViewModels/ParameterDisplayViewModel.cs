using System.Windows.Input;
using Egor92.MvvmNavigation.Abstractions;
using Samples.Common;
using SinglePageApp.Constants;

namespace SinglePageApp.ViewModels
{
    public class ParameterDisplayViewModel : ViewModelBase, INavigatedToAware
    {
        #region Fields

        private readonly INavigationManager _navigationManager;

        #endregion

        #region Ctor

        public ParameterDisplayViewModel(INavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
        }

        #endregion

        #region PassedParameter

        private string _passedParameter;

        public string PassedParameter
        {
            get { return _passedParameter; }
            set { SetProperty(ref _passedParameter, value); }
        }

        #endregion

        #region ReturnBackCommand

        private ICommand _startOverCommand;

        public ICommand StartOverCommand
        {
            get { return _startOverCommand ?? (_startOverCommand = new DelegateCommand(ReturnBack)); }
        }

        private void ReturnBack()
        {
            _navigationManager.Navigate(NavigationKeys.Welcome);
        }

        #endregion

        #region Implementation of INavigatedToAware

        public void OnNavigatedTo(object arg)
        {
            if (!(arg is string))
                return;

            PassedParameter = (string) arg;
        }

        #endregion
    }
}
