using System;
using JetBrains.Annotations;

namespace Egor92.UINavigation.Wpf.Internal
{
    internal sealed class NavigationData
    {
        internal NavigationData([NotNull] Func<object> viewModelFunc, [NotNull] Func<object> viewFunc)
        {
            ViewModelFunc = viewModelFunc ?? throw new ArgumentNullException(nameof(viewModelFunc));
            ViewFunc = viewFunc ?? throw new ArgumentNullException(nameof(viewFunc));
        }

        internal Func<object> ViewModelFunc { get; }

        internal Func<object> ViewFunc { get; }
    }
}
