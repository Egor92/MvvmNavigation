using System;
using JetBrains.Annotations;

namespace Egor92.UINavigation.Wpf
{
    public sealed class NavigationData
    {
        public NavigationData([NotNull] Func<object> viewModelFunc, [NotNull] Func<object> viewFunc)
        {
            ViewModelFunc = viewModelFunc ?? throw new ArgumentNullException(nameof(viewModelFunc));
            ViewFunc = viewFunc ?? throw new ArgumentNullException(nameof(viewFunc));
        }

        public Func<object> ViewModelFunc { get; }

        public Func<object> ViewFunc { get; }
    }
}
