using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Egor92.UINavigation.Abstractions;
using Egor92.UINavigation.Wpf.Internal;

namespace Egor92.UINavigation.Wpf
{
    public class NavigationManager : INavigationManager
    {
        #region Fields

        private readonly ContentControl _frameControl;
        private readonly IDictionary<string, NavigationData> _navigationDatasByKey = new Dictionary<string, NavigationData>();

        #endregion

        #region Ctor

        public NavigationManager(ContentControl frameControl)
        {
            _frameControl = frameControl ?? throw new ArgumentNullException(nameof(frameControl));
        }

        #endregion

        #region Navigated

        public event EventHandler<NavigationEventArgs> Navigated;

        private void RaiseNavigated(NavigationEventArgs e)
        {
            Navigated?.Invoke(this, e);
        }

        #endregion

        public void Register<TView>(string navigationKey, Func<object> viewModelFunc)
            where TView : FrameworkElement, new()
        {
            if (navigationKey == null)
                throw new ArgumentNullException(nameof(navigationKey));

            if (viewModelFunc == null)
                throw new ArgumentNullException(nameof(viewModelFunc));

            var isKeyAlreadyRegistered = _navigationDatasByKey.ContainsKey(navigationKey);
            if (isKeyAlreadyRegistered)
                throw new InvalidOperationException(ExceptionMessages.CanNotRegisterKeyTwice);

            _navigationDatasByKey[navigationKey] = new NavigationData(viewModelFunc, typeof(TView));
        }

        public void Navigate(string navigationKey, object arg = null)
        {
            if (navigationKey == null)
                throw new ArgumentNullException(nameof(navigationKey));

            var isKeyRegistered = _navigationDatasByKey.ContainsKey(navigationKey);
            if (!isKeyRegistered)
                throw new InvalidOperationException(ExceptionMessages.KeyIsNotRegistered(navigationKey));

            InvokeInMainThread(() =>
            {
                InvokeNavigatedFrom();
                var viewModel = GetNewViewModel(navigationKey);

                var view = CreateNewView(navigationKey, viewModel);
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

        private FrameworkElement CreateNewView(string navigationKey, object viewModel)
        {
            var navigationData = _navigationDatasByKey[navigationKey];
            var viewType = navigationData.ViewType;
            var view = (FrameworkElement) Activator.CreateInstance(viewType);
            view.DataContext = viewModel;
            return view;
        }

        private object GetNewViewModel(string navigationKey)
        {
            var navigationData = _navigationDatasByKey[navigationKey];
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
