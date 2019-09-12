using System;
using JetBrains.Annotations;

namespace Egor92.UINavigation.Abstractions
{
    public interface INavigationManager
    {
        void Navigate([NotNull] string navigationKey, object arg = null);

        event EventHandler<NavigationEventArgs> Navigated;
    }
}
