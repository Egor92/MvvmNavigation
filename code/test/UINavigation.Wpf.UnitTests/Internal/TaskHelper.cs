using System;
using System.Threading;
using System.Threading.Tasks;

namespace Egor92.UINavigation.Wpf.UnitTests.Internal
{
    internal static class TaskHelper
    {
        internal static Task StartTaskWithApartmentState(ApartmentState apartmentState, Action action)
        {
            return StartTaskWithApartmentState<object>(apartmentState, () =>
            {
                action();
                return null;
            });
        }

        internal static Task<T> StartTaskWithApartmentState<T>(ApartmentState apartmentState, Func<T> func)
        {
            var tcs = new TaskCompletionSource<T>();
            Thread thread = new Thread(() =>
            {
                try
                {
                    tcs.SetResult(func());
                }
                catch (Exception e)
                {
                    tcs.SetException(e);
                }
            });
            thread.SetApartmentState(apartmentState);
            thread.Name = $"Thread({apartmentState})";
            thread.Start();
            return tcs.Task;
        }
    }
}
