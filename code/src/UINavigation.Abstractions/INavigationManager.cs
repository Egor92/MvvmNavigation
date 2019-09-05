﻿using System;

namespace Egor92.UINavigation.Abstractions
{
    public interface INavigationManager
    {
        void Navigate(string navigationKey, object arg = null);

        event EventHandler<NavigationEventArgs> Navigated;
    }
}
