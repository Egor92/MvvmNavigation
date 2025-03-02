using System;
using System.Diagnostics.CodeAnalysis;
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

        public Navigator([NotNull] object frameControl, IViewInteractionStrategy viewInteractionStrategy,
            [NotNull] IDataStorage dataStorage)
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

        [SuppressMessage("ReSharper", "ConvertToLambdaExpression")]
        public NavigationData Navigate(string navigationKey, object arg)
        {
            EnsureKeyIsCorrect(navigationKey);
            return InvokeInUiThread(() => { return NavigateInternalAsync(navigationKey, arg).GetAwaiter().GetResult(); });
        }

        public Task<NavigationData> NavigateAsync(string navigationKey, object arg, CancellationToken token = default)
        {
            EnsureKeyIsCorrect(navigationKey);

            return InvokeInUiThreadAsync(() => { return NavigateInternalAsync(navigationKey, arg); }, token);
        }

        private void EnsureKeyIsCorrect(string navigationKey)
        {
            if (navigationKey == null)
                throw new ArgumentNullException(nameof(navigationKey));

            var isKeyRegistered = CanNavigate(navigationKey);
            if (!isKeyRegistered)
                throw new InvalidOperationException(ExceptionMessages.KeyIsNotRegistered(navigationKey));
        }

        private async Task<NavigationData> NavigateInternalAsync(string navigationKey, object arg)
        {
            await InvokeNavigatingFromAsync().ConfigureAwait(false);
            var viewModel = GetViewModel(navigationKey);

            var view = CreateView(navigationKey, viewModel);
            _viewInteractionStrategy.SetContent(_frameControl, view);
            await InvokeNavigatedToAsync(viewModel, arg).ConfigureAwait(false);

            return new NavigationData
            {
                View = view,
                ViewModel = viewModel
            };
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
                return Task.FromResult(new NavigationData
                {
                    ViewModel = null,
                    View = view,
                });
            }, token);
        }

        private T InvokeInUiThread<T>(Func<T> action)
        {
            return _viewInteractionStrategy.InvokeInUiThread(_frameControl, action);
        }

        private Task<T> InvokeInUiThreadAsync<T>(Func<Task<T>> action, CancellationToken token)
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

        private async Task InvokeNavigatingFromAsync()
        {
            var oldView = _viewInteractionStrategy.GetContent(_frameControl);
            if (oldView != null)
            {
                var oldViewModel = _viewInteractionStrategy.GetDataContext(oldView);

                var navigatingFromAware = oldViewModel as INavigatingFromAware;
                navigatingFromAware?.OnNavigatingFrom();

                if (oldViewModel is IAsyncNavigatingFromAware asyncNavigatingFromAware)
                {
                    await asyncNavigatingFromAware.OnNavigatingFromAsync().ConfigureAwait(false);
                }
            }
        }

        private static async Task InvokeNavigatedToAsync(object viewModel, object arg)
        {
            var navigatedToAware = viewModel as INavigatedToAware;
            navigatedToAware?.OnNavigatedTo(arg);

            if (viewModel is IAsyncNavigatedToAware asyncNavigatedToAware)
            {
                await asyncNavigatedToAware.OnNavigatedToAsync(arg).ConfigureAwait(false);
            }
        }
    }
}