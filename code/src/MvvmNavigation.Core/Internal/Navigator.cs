using System;
using System.Threading;
using System.Threading.Tasks;
using Egor92.MvvmNavigation.Abstractions;
using JetBrains.Annotations;

namespace Egor92.MvvmNavigation.Internal
{
    public class Navigator
    {
        #region Fields

        private readonly object _frameControl;
        private readonly IViewInteractionStrategy _viewInteractionStrategy;
        private readonly IDataStorage _dataStorage;

        #endregion

        #region Ctor

        public Navigator([NotNull] object frameControl, IViewInteractionStrategy viewInteractionStrategy, [NotNull] IDataStorage dataStorage)
        {
            _frameControl = frameControl ?? throw new ArgumentNullException(nameof(frameControl));
            _viewInteractionStrategy = viewInteractionStrategy ?? throw new ArgumentNullException(nameof(viewInteractionStrategy));
            _dataStorage = dataStorage ?? throw new ArgumentNullException(nameof(dataStorage));
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

            var navigationData = new RegistrationData(getViewModel, getView);
            _dataStorage.Add(navigationKey, navigationData);
        }

        public bool CanNavigate(string navigationKey)
        {
            return _dataStorage.IsExist(navigationKey);
        }

        public NavigationData Navigate(string navigationKey, object arg)
        {
            if (navigationKey == null)
                throw new ArgumentNullException(nameof(navigationKey));

            var isKeyRegistered = CanNavigate(navigationKey);
            if (!isKeyRegistered)
                throw new InvalidOperationException(ExceptionMessages.KeyIsNotRegistered(navigationKey));

            return InvokeInUiThread(() =>
            {
                InvokeNavigatedFrom();
                var viewModel = GetViewModel(navigationKey);

                var view = CreateView(navigationKey, viewModel);
                _viewInteractionStrategy.SetContent(_frameControl, view);
                InvokeNavigatedTo(viewModel, arg);

                return new NavigationData()
                {
                    View = view,
                    ViewModel = viewModel
                };
            });
        }
        
        public Task<NavigationData> NavigateAsync(string navigationKey, object arg, CancellationToken token = default)
        {
            if (navigationKey == null)
                throw new ArgumentNullException(nameof(navigationKey));

            var isKeyRegistered = CanNavigate(navigationKey);
            if (!isKeyRegistered)
                throw new InvalidOperationException(ExceptionMessages.KeyIsNotRegistered(navigationKey));

            return InvokeInUiThreadAsync(() =>
            {
                InvokeNavigatedFrom();
                var viewModel = GetViewModel(navigationKey);

                var view = CreateView(navigationKey, viewModel);
                _viewInteractionStrategy.SetContent(_frameControl, view);
                InvokeNavigatedTo(viewModel, arg);

                return new NavigationData()
                {
                    View = view,
                    ViewModel = viewModel
                };
            }, token);
        }

        public NavigationData Navigate([NotNull] object view)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            return InvokeInUiThread(() =>
            {
                _viewInteractionStrategy.SetContent(_frameControl, view);
                return new NavigationData
                {
                    ViewModel = null,
                    View = view,
                };
            });
        }
        
        public Task<NavigationData> NavigateAsync([NotNull] object view, CancellationToken token = default)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            return InvokeInUiThreadAsync(() =>
            {
                _viewInteractionStrategy.SetContent(_frameControl, view);
                return new NavigationData
                {
                    ViewModel = null,
                    View = view,
                };
            }, token);
        }

        private T InvokeInUiThread<T>(Func<T> action)
        {
            return _viewInteractionStrategy.InvokeInUiThread(_frameControl, action);
        }
        
        private Task<T> InvokeInUiThreadAsync<T>(Func<T> action, CancellationToken token)
        {
            return _viewInteractionStrategy.InvokeInUiThreadAsync(_frameControl, action, token);
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
