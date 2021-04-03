using System;

namespace Egor92.MvvmNavigation.Abstractions
{
    public class NavigationEventArgs : EventArgs
    {
        public NavigationEventArgs(object view, object viewModel, string navigationKey, object navigationArg)
        {
            View = view;
            ViewModel = viewModel;
            NavigationKey = navigationKey;
            NavigationArg = navigationArg;
        }

        public object View { get; }

        public object ViewModel { get; }

        public string NavigationKey { get; }

        public object NavigationArg { get; }
    }
}
