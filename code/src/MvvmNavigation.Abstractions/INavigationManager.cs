using System;
using JetBrains.Annotations;

namespace Egor92.MvvmNavigation.Abstractions
{
    public interface INavigationManager
    {
        bool CanNavigate(string navigationKey);

        void Navigate([NotNull] string navigationKey, object arg);

        event EventHandler<NavigationEventArgs> Navigated;
    }
}
