﻿using System;
using JetBrains.Annotations;

namespace Egor92.UINavigation
{
    public interface IViewInteractionStrategy
    {
        object GetContent(object control);

        void SetContent([NotNull] object control, object content);

        object GetDataContext([NotNull] object control);

        void SetDataContext([NotNull] object control, object dataContext);

        void InvokeInDispatcher([NotNull] object control, [NotNull] Action action);
    }
}
