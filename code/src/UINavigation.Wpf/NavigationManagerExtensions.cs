﻿using System;
using JetBrains.Annotations;

namespace Egor92.UINavigation.Wpf
{
    public static class NavigationManagerExtensions
    {
        public static void Register<TView>([NotNull] this NavigationManager navigationManager,
                                           [NotNull] string navigationKey,
                                           [NotNull] Func<object> getViewModel)
            where TView : class, new()
        {
            Func<object> getView = Activator.CreateInstance<TView>;
            navigationManager.Register(navigationKey, getViewModel, getView);
        }

        public static void Register<TView>([NotNull] this NavigationManager navigationManager, [NotNull] string navigationKey, object viewModel)
            where TView : class, new()
        {
            Func<object> getView = Activator.CreateInstance<TView>;
            navigationManager.Register(navigationKey, () => viewModel, getView);
        }
    }
}
