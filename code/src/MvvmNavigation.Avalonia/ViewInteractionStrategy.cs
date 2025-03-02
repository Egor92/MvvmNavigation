using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using JetBrains.Annotations;

namespace Egor92.MvvmNavigation
{
    public class ViewInteractionStrategy : IViewInteractionStrategy
    {
        public object GetContent(object control)
        {
            if (control == null)
            {
                throw new ArgumentNullException(nameof(control));
            }

            if (!(control is IContentControl contentControl))
            {
                return null;
            }

            return contentControl.Content;
        }

        public void SetContent(object control, [NotNull] object content)
        {
            if (control == null)
            {
                throw new ArgumentNullException(nameof(control));
            }

            if (control is IContentControl contentControl)
            {
                contentControl.Content = content;
            }
        }

        public object GetDataContext(object control)
        {
            if (control == null)
            {
                throw new ArgumentNullException(nameof(control));
            }

            if (control is IDataContextProvider dataContextProvider)
            {
                return dataContextProvider.DataContext;
            }

            return null;
        }

        public void SetDataContext(object control, object dataContext)
        {
            if (control == null)
            {
                throw new ArgumentNullException(nameof(control));
            }

            if (control is IDataContextProvider dataContextProvider)
            {
                dataContextProvider.DataContext = dataContext;
            }
        }

        public void InvokeInUIThread(object control, Action action)
        {
            if (control == null)
            {
                throw new ArgumentNullException(nameof(control));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var dispatcher = Dispatcher.UIThread;
            if (dispatcher == null || dispatcher.CheckAccess())
            {
                action();
            }
            else
            {
                dispatcher.InvokeAsync(action).GetAwaiter().GetResult();
            }
        }

        public T InvokeInUiThread<T>(object control, Func<T> action)
        {
            var task = InvokeInUiThreadAsync(control, async () => action(), default);
            return task.GetAwaiter().GetResult();
        }

        public async Task<T> InvokeInUiThreadAsync<T>(object control, Func<Task<T>> action, CancellationToken token)
        {
            if (control == null)
            {
                throw new ArgumentNullException(nameof(control));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var dispatcher = Dispatcher.UIThread;
            if (dispatcher == null || dispatcher.CheckAccess())
            {
                return await action().ConfigureAwait(false);
            }
            else
            {
                return await dispatcher.InvokeAsync(action).ConfigureAwait(false);
            }
        }
    }
}