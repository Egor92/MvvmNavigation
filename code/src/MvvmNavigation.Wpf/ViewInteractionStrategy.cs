using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Egor92.MvvmNavigation.Internal;
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

            if (!(control is ContentControl contentControl))
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

            if (control is ContentControl contentControl)
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

            if (control is FrameworkElement frameworkElement)
            {
                return frameworkElement.DataContext;
            }

            return null;
        }

        public void SetDataContext(object control, object dataContext)
        {
            if (control == null)
            {
                throw new ArgumentNullException(nameof(control));
            }

            if (control is FrameworkElement frameworkElement)
            {
                frameworkElement.DataContext = dataContext;
            }
        }

        [Obsolete]
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

            if (!(control is DispatcherObject dispatcherObject))
            {
                return;
            }

            var dispatcher = dispatcherObject.Dispatcher;
            if (dispatcher == null || dispatcher.CheckAccess())
            {
                action();
            }
            else
            {
                dispatcher.Invoke(action, DispatcherPriority.Normal);
            }
        }

        public T InvokeInUiThread<T>(object control, Func<T> action)
        {
            if (control == null)
            {
                throw new ArgumentNullException(nameof(control));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            if (!(control is DispatcherObject dispatcherObject))
            {
                throw new ArgumentException(ExceptionMessages.ControlIsNotOfTypeDispatcherObject);
            }

            var dispatcher = dispatcherObject.Dispatcher;
            if (dispatcher == null || dispatcher.CheckAccess())
            {
                return action();
            }
            else
            {
                return dispatcher.Invoke(action, DispatcherPriority.Normal);
            }
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

            if (!(control is DispatcherObject dispatcherObject))
            {
                throw new ArgumentException(ExceptionMessages.ControlIsNotOfTypeDispatcherObject);
            }

            var dispatcher = dispatcherObject.Dispatcher;
            if (dispatcher == null || dispatcher.CheckAccess())
            {
                return await action().ConfigureAwait(false);
            }
            else
            {
                var task = await dispatcher.InvokeAsync(action, DispatcherPriority.Normal).Task.ConfigureAwait(false);
                return await task.ConfigureAwait(false);
            }
        }
    }
}