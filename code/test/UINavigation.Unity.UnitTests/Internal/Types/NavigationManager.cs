using System;
using Egor92.UINavigation.Abstractions;

namespace Egor92.UINavigation.Unity.UnitTests.Internal.Types
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

        public event EventHandler<NavigationEventArgs> Navigated;
    }
}
