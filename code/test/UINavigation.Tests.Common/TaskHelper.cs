using System;
using System.Threading;
using System.Threading.Tasks;

namespace Egor92.UINavigation.Tests.Common
{
    public static class TaskHelper
    {
        public static Task StartTaskWithApartmentState(ApartmentState apartmentState, Action action)
        {
            return StartTaskWithApartmentState<object>(apartmentState, () =>
            {
                action();
                return null;
            });
        }

        public static Task<T> StartTaskWithApartmentState<T>(ApartmentState apartmentState, Func<T> func)
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
