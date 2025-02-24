using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Egor92.MvvmNavigation.Abstractions;
using Egor92.MvvmNavigation.Internal;
using JetBrains.Annotations;

namespace Egor92.MvvmNavigation
{
    public abstract class NavigationManagerBase : INavigationManager
    {
        #region Fields

        private readonly Stack<NavigationData> _navigationHistory = new();
        private readonly Navigator _navigator;

        #endregion

        #region Ctor

        protected NavigationManagerBase([NotNull] object frameControl, IViewInteractionStrategy viewInteractionStrategy)
            : this(frameControl, viewInteractionStrategy, new DataStorage())
        {
        }

        protected NavigationManagerBase([NotNull] object frameControl,
            IViewInteractionStrategy viewInteractionStrategy,
            [NotNull] IDataStorage dataStorage)
        {
            _navigator = new Navigator(frameControl, viewInteractionStrategy, dataStorage);
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
            _navigator.Register(navigationKey, getViewModel, getView);
        }

        public bool CanNavigate(string navigationKey)
        {
            return _navigator.CanNavigate(navigationKey);
        }

        public NavigationData Navigate(string navigationKey, object arg)
        {
            var navigationData = _navigator.Navigate(navigationKey, arg);
            var navigationEventArgs = new NavigationEventArgs(navigationData.View, navigationData.ViewModel, navigationKey, arg);
            SaveNavigationHistory(navigationData.ViewModel, navigationData.View);
            RaiseNavigated(navigationEventArgs);
            return navigationData;
        }

        public async Task<NavigationData> NavigateAsync(string navigationKey, object arg, CancellationToken token = default)
        {
            var navigationData = await _navigator.NavigateAsync(navigationKey, arg, token);
            var navigationEventArgs = new NavigationEventArgs(navigationData.View, navigationData.ViewModel, navigationKey, arg);
            SaveNavigationHistory(navigationData.ViewModel, navigationData.View);
            RaiseNavigated(navigationEventArgs);
            return navigationData;
        }

        private void SaveNavigationHistory(object viewModel, object view)
        {
            _navigationHistory.Push(new NavigationData
            {
                ViewModel = viewModel,
                View = view,
            });
        }

        public NavigationData NavigateBack()
        {
            var navigationData = _navigationHistory.Pop();
            return _navigator.Navigate(navigationData.View);
        }

        public Task<NavigationData> NavigateBackAsync(CancellationToken token = default)
        {
            var navigationData = _navigationHistory.Pop();
            return _navigator.NavigateAsync(navigationData.View, token);
        }
    }
}