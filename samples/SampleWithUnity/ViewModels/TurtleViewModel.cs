using System.Windows.Input;
using Egor92.MvvmNavigation.Abstractions;
using ReactiveUI;

namespace SampleWithUnity.ViewModels
{
    public abstract class TurtleViewModel(INavigationManager navigationManager)
    {
        public abstract string Name { get; }

        public abstract string Color { get; }

        #region SelectAnotherTurtleCommand

        private ICommand _selectAnotherTurtleCommand;

        public ICommand SelectAnotherTurtleCommand => _selectAnotherTurtleCommand ??= ReactiveCommand.Create(SelectAnotherTurtle);

        private void SelectAnotherTurtle()
        {
            var nextNavigationKey = RandomViewSelector.GetNextNavigationKey();
            navigationManager.Navigate(nextNavigationKey);
        }

        #endregion
    }
}