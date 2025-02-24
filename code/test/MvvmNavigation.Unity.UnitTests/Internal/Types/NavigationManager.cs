using System;
using System.Threading;
using System.Threading.Tasks;
using Egor92.MvvmNavigation.Abstractions;

namespace Egor92.MvvmNavigation.Unity.UnitTests.Internal.Types
{
    internal class NavigationManager : INavigationManager
    {
        public bool CanNavigate(string navigationKey)
        {
            throw new NotImplementedException();
        }

        public NavigationData Navigate(string navigationKey, object arg)
        {
            throw new NotImplementedException();
        }

        public Task<NavigationData> NavigateAsync(string navigationKey, object arg, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public NavigationData NavigateBack()
        {
            throw new NotImplementedException();
        }

        public Task<NavigationData> NavigateBackAsync(CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

#pragma warning disable 67
        public event EventHandler<NavigationEventArgs> Navigated;
#pragma warning restore 67
    }
}
