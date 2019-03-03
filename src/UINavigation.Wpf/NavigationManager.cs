using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Egor92.UINavigation.Abstractions;

namespace Egor92.UINavigation.Wpf
{
    public class NavigationManager : INavigationManager
    {
        #region Fields

        private readonly Dispatcher _dispatcher;
        private readonly ContentControl _frameControl;
        private readonly IDictionary<string, object> _viewModelsByNavigationKey = new Dictionary<string, object>();
        private readonly IDictionary<Type, Type> _viewTypesByViewModelType = new Dictionary<Type, Type>();

        #endregion

        #region Ctor

        public NavigationManager(Dispatcher dispatcher, ContentControl frameControl)
        {
            _dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
            _frameControl = frameControl ?? throw new ArgumentNullException(nameof(frameControl));
        }

        #endregion

        public void Register<TViewModel, TView>(TViewModel viewModel, string navigationKey)
            where TViewModel : class
            where TView : FrameworkElement
        {
            if (navigationKey == null)
                throw new ArgumentNullException(nameof(navigationKey));

            _viewModelsByNavigationKey[navigationKey] = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
            _viewTypesByViewModelType[typeof(TViewModel)] = typeof(TView);
        }

        public void Navigate(string navigationKey, object arg = null)
        {
            if (navigationKey == null)
                throw new ArgumentNullException(nameof(navigationKey));

            InvokeInMainThread(() =>
            {
                InvokeNavigatingFrom();
                var viewModel = GetNewViewModel(navigationKey);
                InvokeNavigatingTo(viewModel, arg);

                var view = CreateNewView(viewModel);
                _frameControl.Content = view;
            });
        }

        private void InvokeInMainThread(Action action)
        {
            _dispatcher.Invoke(action);
        }

        private FrameworkElement CreateNewView(object viewModel)
        {
            var viewType = _viewTypesByViewModelType[viewModel.GetType()];
            var view = (FrameworkElement) Activator.CreateInstance(viewType);
            view.DataContext = viewModel;
            return view;
        }

        private object GetNewViewModel(string navigationKey)
        {
            return _viewModelsByNavigationKey[navigationKey];
        }

        private void InvokeNavigatingFrom()
        {
            var oldView = _frameControl.Content as FrameworkElement;
            var navigationAware = oldView?.DataContext as INavigationAware;
            navigationAware?.OnNavigatingFrom();
        }

        private static void InvokeNavigatingTo(object viewModel, object arg)
        {
            var navigationAware = viewModel as INavigationAware;
            navigationAware?.OnNavigatingTo(arg);
        }
    }
}
