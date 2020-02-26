using System;
using JetBrains.Annotations;

namespace Egor92.MvvmNavigation
{
    public static class NavigationManagerBaseExtensions
    {
        public static void Register<TView>([NotNull] this NavigationManagerBase navigationManager,
                                           [NotNull] string navigationKey,
                                           [NotNull] Func<object> getViewModel)
            where TView : class, new()
        {
            Func<object> getView = Activator.CreateInstance<TView>;
            navigationManager.Register(navigationKey, getViewModel, getView);
        }

        public static void Register<TView>([NotNull] this NavigationManagerBase navigationManager,
                                           [NotNull] string navigationKey,
                                           object viewModel)
            where TView : class, new()
        {
            Func<object> getView = Activator.CreateInstance<TView>;
            navigationManager.Register(navigationKey, () => viewModel, getView);
        }
    }
}
