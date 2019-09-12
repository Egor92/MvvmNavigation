using System;

namespace Egor92.UINavigation.Wpf.Internal
{
    internal sealed class NavigationData
    {
        internal NavigationData(Func<object> viewModelFunc, Func<object> viewFunc)
        {
            ViewModelFunc = viewModelFunc;
            ViewFunc = viewFunc;
        }

        internal Func<object> ViewModelFunc { get; }

        internal Func<object> ViewFunc { get; }
    }
}
