using System;
using Egor92.MvvmNavigation.Abstractions;
using Egor92.MvvmNavigation.Internal;
using JetBrains.Annotations;

namespace Egor92.MvvmNavigation
{
    public abstract class NavigationManagerBase : INavigationManager
    {
        #region Fields

        private readonly object _frameControl;
        private readonly IViewInteractionStrategy _viewInteractionStrategy;
        private readonly IDataStorage _dataStorage;

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
            _frameControl = frameControl ?? throw new ArgumentNullException(nameof(frameControl));
            _viewInteractionStrategy = viewInteractionStrategy ?? throw new ArgumentNullException(nameof(viewInteractionStrategy));
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

            InvokeInDispatcher(() =>
            {
                InvokeNavigatedFrom();
                var viewModel = GetViewModel(navigationKey);

                var view = CreateView(navigationKey, viewModel);
                _viewInteractionStrategy.SetContent(_frameControl, view);
                InvokeNavigatedTo(viewModel, arg);

                var navigationEventArgs = new NavigationEventArgs(view, viewModel, navigationKey, arg);
                RaiseNavigated(navigationEventArgs);
            });
        }

        private void InvokeInDispatcher(Action action)
        {
            _viewInteractionStrategy.InvokeInUIThread(_frameControl, action);
        }

        private object CreateView(string navigationKey, object viewModel)
        {
            var navigationData = _dataStorage.Get(navigationKey);
            var view = navigationData.ViewFunc();
            if (view != null)
            {
                _viewInteractionStrategy.SetDataContext(view, viewModel);
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
            var oldView = _viewInteractionStrategy.GetContent(_frameControl);
            if (oldView != null)
            {
                var oldViewModel = _viewInteractionStrategy.GetDataContext(oldView);
                var navigationAware = oldViewModel as INavigatingFromAware;
                navigationAware?.OnNavigatingFrom();
            }
        }

        private static void InvokeNavigatedTo(object viewModel, object arg)
        {
            var navigationAware = viewModel as INavigatedToAware;
            navigationAware?.OnNavigatedTo(arg);
        }
    }
}
