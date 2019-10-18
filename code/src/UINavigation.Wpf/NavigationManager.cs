using System;
using System.Windows;
using System.Windows.Controls;
using Egor92.UINavigation.Abstractions;
using Egor92.UINavigation.Wpf.Internal;
using JetBrains.Annotations;

namespace Egor92.UINavigation.Wpf
{
    public class NavigationManager : INavigationManager
    {
        #region Fields

        private readonly ContentControl _frameControl;
        private readonly IDataStorage _dataStorage;

        #endregion

        #region Ctor

        public NavigationManager([NotNull] ContentControl frameControl)
            : this(frameControl, new DataStorage())
        {
        }

        public NavigationManager([NotNull] ContentControl frameControl, [NotNull] IDataStorage dataStorage)
        {
            _frameControl = frameControl ?? throw new ArgumentNullException(nameof(frameControl));
            _dataStorage = dataStorage ?? throw new ArgumentNullException(nameof(dataStorage));
        }

        #endregion

        #region Navigated

        public event EventHandler<NavigationEventArgs> Navigated;

        private void RaiseNavigated(NavigationEventArgs e)
        {
            Navigated?.Invoke(this, e);
        }

        #endregion

        public void Register([NotNull] string navigationKey, [NotNull] Func<object> getViewModel, [NotNull] Func<object> getView)
        {
            if (navigationKey == null)
                throw new ArgumentNullException(nameof(navigationKey));

            if (getViewModel == null)
                throw new ArgumentNullException(nameof(getViewModel));

            if (getView == null)
                throw new ArgumentNullException(nameof(getView));

            var isKeyAlreadyRegistered = _dataStorage.IsExist(navigationKey);
            if (isKeyAlreadyRegistered)
                throw new InvalidOperationException(ExceptionMessages.CanNotRegisterKeyTwice);

            var navigationData = new NavigationData(getViewModel, getView);
            _dataStorage.Add(navigationKey, navigationData);
        }

        public bool CanNavigate(string navigationKey)
        {
            return _dataStorage.IsExist(navigationKey);
        }

        public void Navigate(string navigationKey, object arg)
        {
            if (navigationKey == null)
                throw new ArgumentNullException(nameof(navigationKey));

            var isKeyRegistered = CanNavigate(navigationKey);
            if (!isKeyRegistered)
                throw new InvalidOperationException(ExceptionMessages.KeyIsNotRegistered(navigationKey));

            InvokeInMainThread(() =>
            {
                InvokeNavigatedFrom();
                var viewModel = GetViewModel(navigationKey);

                var view = CreateView(navigationKey, viewModel);
                _frameControl.Content = view;
                InvokeNavigatedTo(viewModel, arg);

                var navigationEventArgs = new NavigationEventArgs(view, viewModel, navigationKey, arg);
                RaiseNavigated(navigationEventArgs);
            });
        }

        private void InvokeInMainThread(Action action)
        {
            var dispatcher = _frameControl.Dispatcher;
            if (dispatcher.CheckAccess())
            {
                action();
            }
            else
            {
                dispatcher.Invoke(action);
            }
        }

        private object CreateView(string navigationKey, object viewModel)
        {
            var navigationData = _dataStorage.Get(navigationKey);
            var view = navigationData.ViewFunc();
            if (view is FrameworkElement frameworkElement)
            {
                frameworkElement.DataContext = viewModel;
            }

            return view;
        }

        private object GetViewModel(string navigationKey)
        {
            var navigationData = _dataStorage.Get(navigationKey);
            return navigationData.ViewModelFunc();
        }

        private void InvokeNavigatedFrom()
        {
            var oldView = _frameControl.Content as FrameworkElement;
            var navigationAware = oldView?.DataContext as INavigatingFromAware;
            navigationAware?.OnNavigatingFrom();
        }

        private static void InvokeNavigatedTo(object viewModel, object arg)
        {
            var navigationAware = viewModel as INavigatedToAware;
            navigationAware?.OnNavigatedTo(arg);
        }
    }
}
