﻿using System.Windows.Controls;
using Egor92.MvvmNavigation.Internal;
using JetBrains.Annotations;

namespace Egor92.MvvmNavigation
{
    [FrameControlType(typeof(ContentControl))]
    public class NavigationManager : NavigationManagerBase
    {
        public NavigationManager([NotNull] ContentControl frameControl)
            : base(frameControl, new ViewInteractionStrategy())
        {
        }

        public NavigationManager([NotNull] ContentControl frameControl, [NotNull] IDataStorage dataStorage)
            : base(frameControl, new ViewInteractionStrategy(), dataStorage)
        {
        }
    }
}
