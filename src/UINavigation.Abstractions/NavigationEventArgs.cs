using System;

namespace Egor92.UINavigation.Abstractions
{
    public class NavigationEventArgs : EventArgs
    {
        public NavigationEventArgs(object view, object viewModel, object navigationKey, object navigationArg)
        {
            View = view;
            ViewModel = viewModel;
            NavigationKey = navigationKey;
            NavigationArg = navigationArg;
        }

        public object View { get; }

        public object ViewModel { get; }

        public object NavigationKey { get; }

        public object NavigationArg { get; }
    }
}
