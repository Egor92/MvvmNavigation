using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using JetBrains.Annotations;

namespace Egor92.UINavigation.Wpf
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

        public void InvokeInDispatcher(object control, Action action)
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
    }
}
