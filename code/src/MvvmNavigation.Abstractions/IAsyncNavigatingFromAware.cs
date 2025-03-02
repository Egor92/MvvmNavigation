using System.Threading;
using System.Threading.Tasks;

namespace Egor92.MvvmNavigation.Abstractions
{
    public interface IAsyncNavigatingFromAware
    {
        Task OnNavigatingFromAsync(object arg, CancellationToken token = default);
    }
}