﻿using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Egor92.MvvmNavigation.Abstractions
{
    public static class NavigationManagerExtensions
    {
        public static NavigationData Navigate(this INavigationManager navigationManager, [NotNull] string navigationKey)
        {
            return navigationManager.Navigate(navigationKey, null);
        }

        public static Task<NavigationData> NavigateAsync(
            this INavigationManager navigationManager,
            [NotNull] string navigationKey,
            CancellationToken token = default)
        {
            return navigationManager.NavigateAsync(navigationKey, null, token);
        }
    }
}