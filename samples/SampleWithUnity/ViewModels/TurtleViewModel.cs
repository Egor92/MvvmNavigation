using System.Windows.Input;
using Egor92.MvvmNavigation.Abstractions;
using Samples.Common;

namespace SampleWithUnity.ViewModels
{
    public abstract class TurtleViewModel
    {
        private readonly INavigationManager _navigationManager;

        protected TurtleViewModel(INavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
        }

        #region Name

        public abstract string Name { get; }

        #endregion

        #region Name

        public abstract string Color { get; }

        #endregion

        #region SelectAnotherTurtleCommand

        private ICommand _selectAnotherTurtleCommand;

        public ICommand SelectAnotherTurtleCommand
        {
            get { return _selectAnotherTurtleCommand ?? (_selectAnotherTurtleCommand = new DelegateCommand(SelectAnotherTurtle)); }
        }

        private void SelectAnotherTurtle()
        {
            var nextNavigationKey = RandomViewSelector.GetNextNavigationKey();
            _navigationManager.Navigate(nextNavigationKey);
        }

        #endregion
    }
}
