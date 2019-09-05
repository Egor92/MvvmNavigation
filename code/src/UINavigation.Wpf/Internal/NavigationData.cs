using System;

namespace Egor92.UINavigation.Wpf.Internal
{
    internal class NavigationData
    {
        public NavigationData(Func<object> viewModelFunc, Type viewType)
        {
            ViewModelFunc = viewModelFunc;
            ViewType = viewType;
        }

        internal Func<object> ViewModelFunc { get; }

        internal Type ViewType { get; }
    }
}
