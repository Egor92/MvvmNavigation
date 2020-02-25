using System;
using System.Diagnostics.CodeAnalysis;
using Egor92.MvvmNavigation.Abstractions;

namespace Egor92.MvvmNavigation.Unity.UnitTests.Internal.Types
{
    internal class NavigationManager : INavigationManager
    {
        public bool CanNavigate(string navigationKey)
        {
            throw new NotImplementedException();
        }

        public void Navigate(string navigationKey, object arg)
        {
            throw new NotImplementedException();
        }

        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        public event EventHandler<NavigationEventArgs> Navigated;
    }
}
