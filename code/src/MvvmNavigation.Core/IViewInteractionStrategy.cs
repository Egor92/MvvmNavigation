﻿using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Egor92.MvvmNavigation
{
    public interface IViewInteractionStrategy
    {
        object GetContent(object control);

        void SetContent([NotNull] object control, object content);

        object GetDataContext([NotNull] object control);

        void SetDataContext([NotNull] object control, object dataContext);

        void InvokeInUIThread([NotNull] object control, [NotNull] Action action);

        T InvokeInUiThread<T>([NotNull] object control, [NotNull] Func<T> action);

        Task<T> InvokeInUiThreadAsync<T>([NotNull] object control, [NotNull] Func<Task<T>> action, CancellationToken token);
    }
}
