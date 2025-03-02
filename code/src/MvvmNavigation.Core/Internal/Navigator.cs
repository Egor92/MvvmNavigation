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
            return InvokeInUiThread(() =>
            {
                InvokeNavigatingFrom(arg);
                InvokeNavigatingFromAsync(arg, CancellationToken.None);
                
                var navigationData = NavigateInternal(navigationKey);

                InvokeNavigatedTo(arg);
                InvokeNavigatedToAsync(arg, CancellationToken.None);

                return navigationData;
            });
        }

        [SuppressMessage("ReSharper", "MethodHasAsyncOverloadWithCancellation")]
        public Task<NavigationData> NavigateAsync(string navigationKey, object arg, CancellationToken token = default)
        {
            EnsureKeyIsCorrect(navigationKey);

            return InvokeInUiThreadAsync(async () =>
            {
                InvokeNavigatingFrom(arg);
                await InvokeNavigatingFromAsync(arg, token);
                
                var navigationData = NavigateInternal(navigationKey);

                InvokeNavigatedTo(arg);
                await InvokeNavigatedToAsync(arg, token);

                return navigationData;
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
                return Task.FromResult(new NavigationData
                {
                    ViewModel = null,
                    View = view,
                });
            }, token);
        }

        private void EnsureKeyIsCorrect(string navigationKey)
        {
            if (navigationKey == null)
                throw new ArgumentNullException(nameof(navigationKey));

            var isKeyRegistered = CanNavigate(navigationKey);
            if (!isKeyRegistered)
                throw new InvalidOperationException(ExceptionMessages.KeyIsNotRegistered(navigationKey));
        }

        private NavigationData NavigateInternal(string navigationKey)
        {
            var viewModel = GetViewModel(navigationKey);
            var view = CreateView(navigationKey, viewModel);
            _viewInteractionStrategy.SetContent(_frameControl, view);
            var navigationData = new NavigationData
            {
                View = view,
                ViewModel = viewModel
            };
            return navigationData;
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

        private void InvokeNavigatingFrom(object arg)
        {
            InvokeForViewAndViewModel<INavigatingFromAware>(x => x.OnNavigatingFrom(arg));
        }

        private void InvokeNavigatedTo(object arg)
        {
            InvokeForViewAndViewModel<INavigatedToAware>(x => x.OnNavigatedTo(arg));
        }

        private Task InvokeNavigatingFromAsync(object arg, CancellationToken token)
        {
            return InvokeForViewAndViewModelAsync<IAsyncNavigatingFromAware>(x => x.OnNavigatingFromAsync(arg, token));
        }

        private Task InvokeNavigatedToAsync(object arg, CancellationToken token)
        {
            return InvokeForViewAndViewModelAsync<IAsyncNavigatedToAware>(x => x.OnNavigatedToAsync(arg, token));
        }

        private void InvokeForViewAndViewModel<T>(Action<T> action)
        {
            var (view, viewModel) = GetViewAndViewModel();
            Invoke(view);
            Invoke(viewModel);
            return;

            void Invoke(object target)
            {
                if (target is T t)
                {
                    action(t);
                }
            }
        }

        private async Task InvokeForViewAndViewModelAsync<T>(Func<T, Task> action)
        {
            var (view, viewModel) = GetViewAndViewModel();
            await InvokeAsync(view).ConfigureAwait(false);
            await InvokeAsync(viewModel).ConfigureAwait(false);
            return;

            Task InvokeAsync(object target)
            {
                if (target is T t)
                {
                    return action(t);
                }

                return Task.CompletedTask;
            }
        }

        private (object view, object viewModel) GetViewAndViewModel()
        {
            var view = _viewInteractionStrategy.GetContent(_frameControl);
            var viewModel = view is not null
                ? _viewInteractionStrategy.GetDataContext(view)
                : null;
            return (view, viewModel);
        }
    }
}