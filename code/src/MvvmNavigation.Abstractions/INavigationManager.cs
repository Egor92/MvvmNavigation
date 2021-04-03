using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Egor92.MvvmNavigation.Abstractions
{
    public interface INavigationManager
    {
        bool CanNavigate(string navigationKey);

        NavigationData Navigate([NotNull] string navigationKey, object arg);

        Task<NavigationData> NavigateAsync([NotNull] string navigationKey, object arg, CancellationToken token = default);

        NavigationData NavigateBack();

        Task<NavigationData> NavigateBackAsync(CancellationToken token = default);

        event EventHandler<NavigationEventArgs> Navigated;
    }
}