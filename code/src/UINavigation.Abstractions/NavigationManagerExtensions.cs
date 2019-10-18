using JetBrains.Annotations;

namespace Egor92.UINavigation.Abstractions
{
    public static class NavigationManagerExtensions
    {
        public static void Navigate(this INavigationManager navigationManager, [NotNull] string navigationKey)
        {
            navigationManager.Navigate(navigationKey, null);
        }
    }
}
